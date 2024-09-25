using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Arekat.Server.Models
{
    [Index(nameof(Id))]
    [Index(nameof(Name))]
    [Table("BaseUser")]
    public class BaseUser
    {
        [Key]
        public Guid Guid { get; set; }

        public int Id { get; set; }

        [Length(4,32)]
        public required string Name { get; set; }

        //Encrypted with SHA3-256
        [MaxLength(64)]
        public required string Password {  get; set; }

        [MaxLength(64)]
        public string? Email { get; set; }

        [MaxLength(16777216)]
        public byte?[]? Avator { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime? LoginTime { get; set; }

        public string? Intro { get; set; }

        [DefaultValue(0)]
        public int Level { get; set; }

        [DefaultValue(0)]
        public int ChartDesignerLevel {  get; set; }

        [DefaultValue(0)]
        public int AdminLevel { get; set; }

        public List<Chart> Charts { get; set; } = new List<Chart>();

        public List<Notice> Notices { get; set; } = new List<Notice>();

        public List<Badge> Badges { get; set; } = new List<Badge>();

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        [DefaultValue(false)]
        public bool IsBanned { get; set; }
    }
}
