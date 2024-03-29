using Microsoft.EntityFrameworkCore;
using NS.WebAPI.Core.Identity.Security.Models;

namespace NS.WebAPI.Core.Identity.Security
{
    public interface ISecurityKeyContext
    {
        DbSet<SecurityKeyWithPrivate> SecurityKeys { get; set; }
    }
}
