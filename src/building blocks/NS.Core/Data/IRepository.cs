using NS.Core.Data;
using System;

namespace NS.Core.DomainObjects
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
