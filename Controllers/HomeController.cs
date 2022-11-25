using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RegistroLogin.Filters;
using RegistroLogin.Models;
using RegistroLogin.Repositories;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace RegistroLogin.Controllers
{
    public class HomeController : Controller
    {
        private RepositoryWeb repo;

        public HomeController(RepositoryWeb repo)
        {
            this.repo = repo;
        }
        [AuthorizeUsers]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(string email, string password, string nombre, string apellidos)
        {

            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])([A-Za-z\d$@$!%*?&]|[^ ]){8,15}$";
            bool passIsValid = Regex.IsMatch(password, pattern);
            if (passIsValid)
            {
                bool registrado = this.repo.RegistrarUsuario(email, password, nombre, apellidos);

                if (registrado)
                {
                    ViewData["MENSAJE"] = "Usuario registrado con exito";
                    ViewData["ALERT_TYPE"] = "alert-primary";
                }
                else
                {
                    ViewData["MENSAJE"] = "Error al registrar el usuario";
                    ViewData["ALERT_TYPE"] = "alert-warning";
                }
            }
            else
            {
                ViewData["MENSAJE"] = "La contraseña debe tener mínimo 8 caracteres y máximo 15 sin espacios, debe tener al menos 1 número, 1 minúscula, 1 mayúscula y 1 caracter especial";
                ViewData["ALERT_TYPE"] = "alert-warning";
            }

            return View();
            //return RedirectToAction("LogIn", "Manage");
        }

        [AuthorizeUsers]
        public IActionResult Usuarios()
        {
            List<Usuario> usuarios = this.repo.GetUsuarios();
            return View(usuarios);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
