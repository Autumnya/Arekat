using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Arekat.Server.Models
{
    [Index(nameof(Id))]
    public class Chart
    {
        [Key]
        public int Id { get; set; }

        public int SongId { get; set; }
        public Song Song { get; set; }

        public int IndexChartId { get; set; }

        [DefaultValue(0)]
        public int Game { get; set; }

        [DefaultValue(2)]
        public int RatingClass { get; set; }

        public List<BaseUser> Designer { get; set; } = new List<BaseUser>();

        [DefaultValue("")]
        public string Diffifulty { get; set; }

        public DateTime PublishDate { get; set; }

        [DefaultValue(0)]
        public int DownloadTime { get; set; }

        //审核阶段
        [DefaultValue(false)]
        public bool IsStable { get; set; }

        [DefaultValue(false)]
        public bool IsChecked { get; set; }

        public List<ChartHistory> History { get; set; } = new List<ChartHistory>();
    }
}
