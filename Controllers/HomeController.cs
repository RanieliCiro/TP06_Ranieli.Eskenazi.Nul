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
        string usuarioStr = HttpContext.Session.GetString("integrante");
        if (!string.IsNullOrEmpty(usuarioStr))
        {
            return RedirectToAction("Index", "Tareas");
        }
        return View();
    }

    [HttpPost]
    public IActionResult Login(string usuario, string contraseña)
    {
        Usuario usr = BD.LevantarUsuario(usuario, contraseña);
        if (usr != null)
        {
            string usuarioStr = Objeto.ObjectToString(usr);
            HttpContext.Session.SetString("integrante", usuarioStr);
            return RedirectToAction("Index", "Tareas");
        }
        ViewBag.Error = "Usuario o contraseña incorrectos";
        return View("Index");
    }

    [HttpPost]
    public IActionResult Registrarse(string usuario, string contraseña)
    {
        Usuario existente = BD.LevantarUsuarioPorEmail(usuario);
        if (existente != null)
        {
            ViewBag.Error = "Ya existe un usuario con ese nombre";
            return View("Index");
        }

        Usuario nuevo = new Usuario { usuario = usuario, contraseña = contraseña };
        BD.AgregarUsuario(nuevo);

        string usuarioStr = Objeto.ObjectToString(nuevo);
        HttpContext.Session.SetString("integrante", usuarioStr);


        return RedirectToAction("Index", "Tareas");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}