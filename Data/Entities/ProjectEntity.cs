using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class ProjectEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Client))]
        [Required]
        public int ClientId { get; set; }

        public virtual ClientEntity Client { get; set; } = null!;

        [ForeignKey(nameof(Status))]
        public int StatusId { get; set; }

        public virtual StatusEntity Status { get; set; } = null!;

        public string? Image { get; set; }

        public string ProjectName { get; set; } = null!;

        public string? Description { get; set; }

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        public virtual DateTime Created { get; set; }

        [Column(TypeName = "money")]
        public decimal? Budget { get; set; }

        // Project-owner
        public string UserId { get; set; } = null!;
    }
}