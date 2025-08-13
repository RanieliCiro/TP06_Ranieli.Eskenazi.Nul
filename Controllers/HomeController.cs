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

    public IActionResult Index()
    {
        List<Usuario> usuarios = BD.ObtenerUsuarios();
        return View();
    }

    [HttpPost]
    public IActionResult VerificarUsuario(string usuario, string contraseña)
    {
        Usuario usuarioEncontrado = BD.verificarCuenta(usuario, contraseña);
        if (usuarioEncontrado != null)
        {
            HttpContext.Session.SetString("usuario", Objeto.ObjectToString(usuarioEncontrado));
            return RedirectToAction("Logeado");
        }
        else
        {
            ViewBag.Error = "Usuario o contraseña incorrectos";
            return View("Index");
        }
    }
    public IActionResult Logeado()
    {
        return RedirectToAction("Index");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
}
