using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Tp06.Models;

namespace Tp06.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult VerificarUsuario(string usuario, string contraseña)
    {
        Usuario usuarioEncontrado = BD.VerificarCuenta(usuario, contraseña);
        if (usuarioEncontrado != null)
        {
            HttpContext.Session.SetString("usuario", Objeto.ObjectToString(usuarioEncontrado));
            return RedirectToAction("Index", "Tareas");
        }
        else
        {
            ViewBag.Error = "Usuario o contraseña incorrectos";
            return View("Login");
        }
    }

    public IActionResult Registro()
    {
        return View();
    }

    [HttpPost]
    public IActionResult RegistrarUsuario(string usuario, string contraseña)
    {
        Usuario existe = BD.ObtenerUsuarios().FirstOrDefault(u => u.usuario == usuario);
        if (existe != null)
        {
            ViewBag.Error = "Ese usuario ya existe";
            return View("Registro");
        }

        BD.RegistrarUsuario(new Usuario { usuario = usuario, contraseña = contraseña });
        return RedirectToAction("Login");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}