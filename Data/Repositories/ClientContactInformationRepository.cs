using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class ClientContactInformationRepository(DataContext context) : BaseRepository<ClientContactInformationEntity>(context), IClientContactInformationRepository
{
}