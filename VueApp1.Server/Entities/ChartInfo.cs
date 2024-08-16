using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arekat.Server.Entities
{
    [Table("chart_info")]
    public class ChartInfo
    {
        [Key]
        public int ChartId { get; set; }

        public SongInfo Song { get; set; }

        public int Diffifulty { get; set; }

        public BaseMemberUser Designer { get; set; }

        public DateOnly PublishDate { get; set; }

        public int DownloadTime { get; set; }

        public int Stage {  get; set; }

        public string Hashcode { get; set; }
    }
}
