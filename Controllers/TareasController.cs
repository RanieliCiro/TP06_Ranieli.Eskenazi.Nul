using Microsoft.AspNetCore.Mvc;
using Tp06.Models;

namespace Tp06.Controllers;

public class TareasController : Controller
{
    public IActionResult Index()
    {
        Usuario logueado = Objeto.StringToObject<Usuario>(HttpContext.Session.GetString("usuario"));
        if (logueado == null) return RedirectToAction("Login", "Auth");

        List<Tarea> tareas = BD.ObtenerTareasDeUsuario(logueado.id);
        return View(tareas);
    }

    public IActionResult Crear()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Crear(string descripcion)
    {
        Usuario logueado = Objeto.StringToObject<Usuario>(HttpContext.Session.GetString("usuario"));
        if (logueado == null) return RedirectToAction("Login", "Auth");

        Tarea nueva = new Tarea { descripcion = descripcion, idCreador = logueado.id };
        int idTarea = BD.CrearTarea(nueva);

        BD.CompartirTarea(idTarea, logueado.id);

        return RedirectToAction("Index");
    }

    public IActionResult MarcarTerminada(int idTarea)
    {
        BD.MarcarTerminada(idTarea);
        return RedirectToAction("Index");
    }

    public IActionResult Compartir(int idTarea, int idUsuario)
    {
        BD.CompartirTarea(idTarea, idUsuario);
        return RedirectToAction("Index");
    }

    public IActionResult Eliminar(int idTarea)
    {
        BD.EliminarTarea(idTarea); 
        return RedirectToAction("Index");
    }
}