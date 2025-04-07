using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Project
    {
        [Required]
        public int Id { get; set; }

        public string? Image { get; set; }
        public string ProjectName { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Budget { get; set; }

        public virtual DateTime Created { get; set; }

        // Implementerar Client- och Statusmodell
        public Client Client { get; set; } = null!;

        public Status Status { get; set; } = null!;
        public string UserId { get; set; } = null!;
    }
}