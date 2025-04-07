using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Data.Repositories;

public class StatusRepository(DataContext context) : BaseRepository<StatusEntity>(context), IStatusRepository
{
}