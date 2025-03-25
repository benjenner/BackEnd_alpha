using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public virtual DbSet<StatusEntity> ProjectStatuses { get; set; }
    public virtual DbSet<ClientEntity> Clients { get; set; }
    public virtual DbSet<ClientContactInformationEntity> ClientContactInformation { get; set; }
    public virtual DbSet<ClientAddressEntity> ClientAddresses { get; set; }
}