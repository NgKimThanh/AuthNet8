using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenNet8.Entities
{
    [Table("SYS_UserRefreshToken")]
    public class SYS_UserRefreshToken
    {
        [Key]
        public int ID { get; set; }

        public int? UserID { get; set; }

        [Required]
        public required string Token { get; set; }

        [Required]
        public required string DeviceInfo { get; set; } // Lưu thông tin trình duyệt

        [ForeignKey("UserID")]
        public required SYS_User User { get; set; }
    }
}
