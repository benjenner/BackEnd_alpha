using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ProjectUpdateForm
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string ProjectName { get; set; } = null!;

        public IFormFile? NewImage { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
        public decimal? Budget { get; set; }

        [Required]
        public int ClientId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int StatusId { get; set; }
    }
}