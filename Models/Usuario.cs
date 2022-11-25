using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroLogin.Models
{
    [Table("User")]
    public class Usuario
    {
        [Key]
        [Column("userid")]
        public int IdUsuario { get; set; }

        [Column("username")]
        public string Email { get; set; }

        [Column("first_name")]
        public string Nombre { get; set; }

        [Column("last_name")]
        public string Apellidos { get; set; }

        [Column("password")]
        public string Password { get; set; }

    }
}
