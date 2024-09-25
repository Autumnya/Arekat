using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arekat.Server.Models
{
    [Index(nameof(Id))]
    public class StoreUid
    {
        [Key]
        public Guid Guid { get; set; }

        public int Id { get; set; }
    }
}
