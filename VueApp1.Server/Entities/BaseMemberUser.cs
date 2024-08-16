using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Arekat.Server.Entities
{
    [Table("base_member_user")]
    public class BaseMemberUser
    {
        [Key]
        public int Id { get; set; }

        [Length(4,32)]
        public string Name { get; set; }

        [Length(4, 32)]
        public string Password {  get; set; }

        [MaxLength(256)]
        public string Email { get; set; }

        [ForeignKey(nameof(Id))]
        public int InviterUid { get; set; }

        public DateOnly RegistrationDate { get; set; }

        public DateOnly? LoginTime { get; set; }

        [DefaultValue(0)]
        public int Level { get; set; }

        [DefaultValue(0)]
        public int ChartDesignerLevel {  get; set; }

        public ICollection<ChartInfo> ChartInfo { get; set; }

        [DefaultValue(false)]
        public bool IsAdmin { get; set; }
    }
}
