using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenNet8.Entities
{
    [Table("SYS_User")]
    public class SYS_User
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(250)]
        public string Password { get; set; } = string.Empty;

        public string? PasswordHash { get; set; }

        public string? PasswordSalt { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? TokenCreated { get; set; }

        public DateTime? TokenExpires { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        public virtual ICollection<SYS_UserRefreshToken> UserRefreshToken { get; set; } = new List<SYS_UserRefreshToken>();
    }
}
