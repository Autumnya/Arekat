using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Arekat.Server.Models
{
    [Index(nameof(Title))]
    public class Song
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(256)]
        [DefaultValue("?")]
        public string Title { get; set; } = "?";

        [DefaultValue("?")]
        public string Bpm {  get; set; } = "?";

        public List<Artist> Artist { get; set; } = new List<Artist>();

        public List<Chart> Charts { get; set; } = new List<Chart>();
    }
}
