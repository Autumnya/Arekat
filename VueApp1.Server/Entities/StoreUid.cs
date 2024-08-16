using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arekat.Server.Entities
{
    [Table("store_uid")]
    public class StoreUid
    {
        [Key]
        public int Id { get; set; }
    }
}
