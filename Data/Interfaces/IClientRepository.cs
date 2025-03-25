using Data.Entities;

namespace Data.Interfaces;

// Interface som ärver från IBaseRepository med ClientEntity som generisk typparameter.
public interface IClientRepository : IBaseRepository<ClientEntity>
{
}