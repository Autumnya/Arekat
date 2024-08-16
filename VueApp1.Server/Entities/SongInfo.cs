using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arekat.Server.Entities
{
    [Table("song_info")]
    public class SongInfo
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Artist { get; set; }

        public string Hashcode { get; set; }
    }
}
