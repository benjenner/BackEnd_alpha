using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Data.Repositories;

// ClientRepository instansieras med DataContext. DataContext instansieras i sin tur med DbContext,
// som tillhandahåller databasanslutningen.

// ClientRepository ärver metoderna från BaseRepository med ClientEntity som datatyp.
// (** ClientRepository behöver INTE implementera metoderna från interfacet då BaseRepository gör detta **)
// BaseRepository instansieras med samma DataContext som ClientRepository

// Slutligen, ClientRepository implementerar IClientRepository vilket öppnar upp för att kunna använda dependency injection.
// Detta innebär att man i service-klassen jobbar mot interface och inte mot ClientRepository direkt.

public class ClientRepository(DataContext context) : BaseRepository<ClientEntity>(context), IClientRepository
{
}