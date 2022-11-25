using RegistroLogin.Data;
using RegistroLogin.Models;
using System.Collections.Generic;
using System.Linq;

namespace RegistroLogin.Repositories
{
    public class RepositoryWeb
    {
        private WebContext context;

        public RepositoryWeb(WebContext context)
        {
            this.context = context;
        }

        private bool ExisteEmail(string email)
        {
            var consulta = from datos in this.context.Usuarios
                           where datos.Email == email
                           select datos;
            if (consulta.Count() > 0)
            {
                //El email existe en la base de datos
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RegistrarUsuario(string email, string password, string nombre, string apellidos)
        {
            bool ExisteEmail = this.ExisteEmail(email);
            if (ExisteEmail)
            {
                return false;
            }
            else
            {
                Usuario usuario = new Usuario();
                usuario.Email = email;
                usuario.Nombre = nombre;
                usuario.Apellidos = apellidos;
                usuario.Password = Helpers.HelperCryptography.SHA1(password);
                this.context.Usuarios.Add(usuario);
                this.context.SaveChanges();

                return true;
            }

        }

        public Usuario LogInUsuario(string email, string password)
        {
            Usuario usuario = this.context.Usuarios.SingleOrDefault(x => x.Email == email);
            if (usuario == null)
            {
                return null;
            }
            else
            {
                //Debemos comparar con la base de datos el password haciendo de nuevo el cifrado con cada salt de usuario
                string passUsuario = usuario.Password;
                //Ciframos de nuevo para comparar           
                string PassSHA1 = Helpers.HelperCryptography.SHA1(password);
                //Comparamos los arrays para comprobar si el cifrado es el mismo
                bool respuesta = string.Equals(passUsuario, PassSHA1);
                if (respuesta == true)
                {
                    return usuario;
                }
                else
                {
                    //Contraseña incorrecta
                    return null;
                }
            }
        }

        public List<Usuario> GetUsuarios()
        {
            var consulta = from datos in this.context.Usuarios
                           select datos;
            return consulta.ToList();
        }
    }
}
