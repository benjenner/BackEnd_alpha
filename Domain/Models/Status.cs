using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Status
    {
        [Required]
        public int Id { get; set; }

        public string? StatusName { get; set; } = null!;

    }
}