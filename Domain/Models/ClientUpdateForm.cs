using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ClientUpdateForm
    {
        //public IFormFile? Image { get; set; }

        [Required]
        public int Id { get; set; }

        public string ClientName { get; set; } = null!;

        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Reference { get; set; } = null!;
        public string BillingAddress { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string City { get; set; } = null!;
    }
}