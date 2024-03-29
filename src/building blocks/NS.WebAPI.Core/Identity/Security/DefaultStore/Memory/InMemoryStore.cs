using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Options;
using NS.WebAPI.Core.Identity.Security.Interfaces;
using NS.WebAPI.Core.Identity.Security.Models;

namespace NS.WebAPI.Core.Identity.Security.DefaultStore.Memory
{
    public class InMemoryStore : IJsonWebKeyStore
    {
        private readonly IOptions<JwksOptions> _options;
        static readonly object lockObject = new object();
        private List<SecurityKeyWithPrivate> _store;
        private SecurityKeyWithPrivate _currentJws;
        private SecurityKeyWithPrivate _currentJwe;

        public InMemoryStore(IOptions<JwksOptions> options)
        {
            _options = options;
            _store = new List<SecurityKeyWithPrivate>();
        }

        public void Save(SecurityKeyWithPrivate securityParameters)
        {
            lock (lockObject)
                _store.Add(securityParameters);

            if (securityParameters.JwkType == JsonWebKeyType.Jws)
                _currentJws = securityParameters;
            else
                _currentJwe = securityParameters;
        }

        public bool NeedsUpdate(JsonWebKeyType jsonWebKeyType)
        {
            if (jsonWebKeyType == JsonWebKeyType.Jws)
            {
                return CheckJwsNeedsUpdate();
            }

            return CheckJweNeedsUpdate();
        }

        private bool CheckJweNeedsUpdate()
        {
            if (_currentJwe == null)
                return true;

            return _currentJwe.CreationDate.AddDays(_options.Value.DaysUntilExpire) < DateTime.UtcNow.Date;
        }

        private bool CheckJwsNeedsUpdate()
        {
            if (_currentJws == null)
                return true;

            return _currentJws.CreationDate.AddDays(_options.Value.DaysUntilExpire) < DateTime.UtcNow.Date;
        }

        public void Revoke(SecurityKeyWithPrivate securityKeyWithPrivate)
        {
            securityKeyWithPrivate.Revoke();
            var oldOne = _store.Find(f => f.Id == securityKeyWithPrivate.Id);
            if (oldOne != null)
            {
                var index = _store.FindIndex(f => f.Id == securityKeyWithPrivate.Id);
                Monitor.Enter(lockObject);
                _store.RemoveAt(index);
                _store.Insert(index, securityKeyWithPrivate);
                Monitor.Exit(lockObject);
            }
        }


        public SecurityKeyWithPrivate GetCurrentKey(JsonWebKeyType jwkType)
        {
            if (jwkType == JsonWebKeyType.Jws)
                return _currentJws;

            return _currentJwe;
        }

        public IReadOnlyCollection<SecurityKeyWithPrivate> Get(JsonWebKeyType jsonWebKeyType, int quantity = 5)
        {
            return
                _store
                    .Where(w => w.JwkType == jsonWebKeyType)
                    .OrderByDescending(s => s.CreationDate)
                    .Take(quantity).ToList().AsReadOnly();
        }

        public void Clear()
        {
            _currentJwe = null;
            _currentJws = null;
            _store.Clear();
        }
    }
}