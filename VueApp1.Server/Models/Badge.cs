using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Arekat.Server.Models
{
    [Index(nameof(Id))]
    [Index(nameof(Name))]
    public class Badge
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(32)]
        public required string Name { get; set; }

        public string Description { get; set; } = "";

        public string ImgSrc { get; set; } = "";
        
        public List<BaseUser> Owners { get; set; } = new List<BaseUser>();
    }
}
