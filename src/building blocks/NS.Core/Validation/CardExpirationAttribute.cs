using System;
using System.ComponentModel.DataAnnotations;

namespace NS.Core.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class CardExpirationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null) return false;

            var month = value.ToString().Split('/')[0];
            var year = $"20{value.ToString().Split('/')[1]}";

            if (int.TryParse(month, out var auxMonth) && int.TryParse(year, out var auxYear))
            {
                var dateTime = new DateTime(auxYear, auxMonth, 1);
                return dateTime > DateTime.UtcNow;
            }

            return false;
        }
    }
}
