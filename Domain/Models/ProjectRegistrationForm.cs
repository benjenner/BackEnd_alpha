using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ProjectRegistrationForm
    {
        public string? Image { get; set; }

        [Required]
        public string ProjectName { get; set; } = null!;

        public string? Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal? Budget { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
    }
}