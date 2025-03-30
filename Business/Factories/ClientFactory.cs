using Data.Entities;
using Domain.Models;

namespace Business.Factories
{
    public class ClientFactory
    {
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
                    BillingAddress = entity.Address.BillingAddress,
                    PostalCode = entity?.Address.PostalCode,
                    City = entity?.Address.City,
                },
            };

            return client;
        }

        public static ClientEntity Map(ClientRegistrationForm form)
        {
            if (form == null)
            {
                return null;
            }
            // Här instansieras en Client enligt modellen
            var clientEntity = new ClientEntity
            {
                ClientName = form.ClientName,
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow,
                IsActive = true,
                ClientContactInformation = new ClientContactInformationEntity
                {
                    Email = form.Email,
                    Phone = form?.Phone,
                    Reference = form?.Reference,
                },
                Address = new ClientAddressEntity
                {
                    BillingAddress = form.BillingAddress,
                    PostalCode = form.PostalCode,
                    City = form.City,
                },
            };

            return clientEntity;
        }

        public static ClientEntity Map(ClientUpdateForm form, ClientEntity entity)
        {
            if (form == null || entity == null)
            {
                return null;
            }

            entity.ClientName = form.ClientName;
            entity.Modified = DateTime.UtcNow;

            entity.ClientContactInformation.Email = form.Email;
            entity.ClientContactInformation.Phone = form.Phone;
            entity.ClientContactInformation.Reference = form.Reference;

            entity.Address.BillingAddress = form.BillingAddress;
            entity.Address.PostalCode = form.PostalCode;
            entity.Address.City = form.City;

            return entity;
        }
    }
}