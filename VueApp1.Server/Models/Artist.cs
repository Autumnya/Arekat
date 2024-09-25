using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Arekat.Server.Models
{
    [Index(nameof(Id))]
    [Index(nameof(Name))]
    public class Artist
    {
        [Key]
        public int Id { get; set; }

        [Length(1,80)]
        [Required]
        public required string Name { get; set; }

        public List<Song> Songs { get; set; } = new List<Song>();
    }
}
