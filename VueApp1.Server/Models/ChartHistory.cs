using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Arekat.Server.Models
{
    [Index(nameof(Id))]
    public class ChartHistory
    {
        [Key]
        public int Id { get; set; }

        public int ChartId { get; set; }
        public Chart Chart { get; set; }

        public int Stage { get; set; }

        [MaxLength(400)]
        public string? Discription { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
