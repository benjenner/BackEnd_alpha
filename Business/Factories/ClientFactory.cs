using Data.Entities;
using Domain.Models;

namespace Business.Factories
{
    public class ClientFactory
    {
        // Mappas enligt Client(modell)
        public static Client Map(ClientEntity entity)
        {
            if (entity == null)
            {
                return null;
            }
            // Här instansieras en Client enligt modellen
            var client = new Client
            {
                Id = entity.Id,
                ClientName = entity.ClientName,
                Created = entity.Created,
                Modified = entity.Modified,
                IsActive = entity.IsActive,
                ClientContactInformation = new ClientContactInformation
                {
                    Email = entity?.ClientContactInformation.Email,
                    Phone = entity?.ClientContactInformation.Phone,
                    Reference = entity?.ClientContactInformation.Reference,
                },
                ClientAddress = new ClientAddress
                {
                    StreetName = entity?.Address.StreetName,
                    StreetNumber = entity?.Address.StreetNumber,
                    PostalCode = entity?.Address.PostalCode,
                    City = entity?.Address.City,
                },
            };

            return client;
        }
    }
}