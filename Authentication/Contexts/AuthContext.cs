using Authentication.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Contexts;

public class AuthContext(DbContextOptions<AuthContext> options) : IdentityDbContext<AppUserEntity>(options)
{
    public DbSet<AppUserAddressEntity> UserAddresses { get; set; }
}