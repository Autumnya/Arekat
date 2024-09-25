using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Arekat.Server.Models
{
    public class Notice
    {
        public int Id { get; set; }

        public DateTime PublishTime { get; set; }

        [MaxLength(50)]
        public string? Title { get; set; }

        [DefaultValue(0)]
        public int ReadTime { get; set; }

        public int ArthorId { get; set; }
        public BaseUser Arthor { get; set; } = null!;

        public string? HtmlText { get; set; }
    }
}
