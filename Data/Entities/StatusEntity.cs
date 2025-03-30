using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    // Detta gör att varje status blir unik i databasen
    [Index(nameof(StatusName), IsUnique = true)]
    public class StatusEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string StatusName { get; set; } = null!;

        public virtual ICollection<ProjectEntity>? Projects { get; set; }
    }
}