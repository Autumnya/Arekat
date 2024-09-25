using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arekat.Server.Models
{
    public class EmailVerifyKey
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int QuestClass { get; set; }

        public DateTime QuestTime { get; set; }

        [MaxLength(64)]
        public required string ReceiverEmail { get; set; }

        [MaxLength(64)]
        public required string Key { get; set; }
    }
}
