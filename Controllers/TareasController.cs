using Microsoft.AspNetCore.Mvc;
using Tp06.Models;

namespace Tp06.Controllers;

public class TareasController : Controller
{
    public IActionResult Index()
    {
        string usuarioStr = HttpContext.Session.GetString("integrante");
        if (string.IsNullOrEmpty(usuarioStr))
        {
            return RedirectToAction("Index", "Home");
        }

        Usuario usuario = Objeto.StringToObject<Usuario>(usuarioStr);
        List<Tarea> tareas = BD.LevantarTareasPorUsuario(usuario.id);

        ViewBag.Usuario = usuario;
        ViewBag.Tareas = tareas;

        return View();
    }

    public IActionResult Agregar()
    {
        string usuarioStr = HttpContext.Session.GetString("integrante");
        if (string.IsNullOrEmpty(usuarioStr))
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost]
    public IActionResult Agregar(string descripcion)
    {
        string usuarioStr = HttpContext.Session.GetString("integrante");
        if (string.IsNullOrEmpty(usuarioStr))
        {
            return RedirectToAction("Index", "Home");
        }

        Usuario usuario = Objeto.StringToObject<Usuario>(usuarioStr);
        if (usuario == null)
        {
            return RedirectToAction("Index", "Home");
        }

        Tarea nueva = new Tarea
        {
            descripcion = descripcion,
            idCreador = usuario.id,
            terminado = false
        };

        BD.AgregarTarea(nueva);

        List<Tarea> todas = BD.LevantarTarea();
        Tarea creada = todas.LastOrDefault(t => t.descripcion == nueva.descripcion && t.idCreador == usuario.id);
        if (creada != null)
        {
            BD.CompartirTarea(creada.id, usuario.id);
        }

        return RedirectToAction("Index");
    }

    public IActionResult Editar(int id)
    {
        string usuarioStr = HttpContext.Session.GetString("integrante");
        if (string.IsNullOrEmpty(usuarioStr))
        {
            return RedirectToAction("Index", "Home");
        }

        Tarea tarea = BD.LevantarTareaPorId(id);
        if (tarea == null)
        {
            return RedirectToAction("Index");
        }

        ViewBag.Tarea = tarea;
        return View();
    }

    [HttpPost]
    public IActionResult Editar(int id, string descripcion, bool terminado)
    {
        string usuarioStr = HttpContext.Session.GetString("integrante");
        if (string.IsNullOrEmpty(usuarioStr))
        {
            return RedirectToAction("Index", "Home");
        }

        Tarea tarea = BD.LevantarTareaPorId(id);
        if (tarea != null)
        {
            tarea.descripcion = descripcion;
            tarea.terminado = terminado;
            BD.ModificarTarea(tarea);
        }

        return RedirectToAction("Index");
    }

    public IActionResult Eliminar(int id)
    {
        string usuarioStr = HttpContext.Session.GetString("integrante");
        if (string.IsNullOrEmpty(usuarioStr))
        {
            return RedirectToAction("Index", "Home");
        }

        Tarea tarea = BD.LevantarTareaPorId(id);
        if (tarea != null)
        {
            string data = HttpContext.Session.GetString("integrante");
            Usuario usr = Objeto.StringToObject<Usuario>(data);
            if (usr != null && tarea.idCreador == usr.id)
            {
                BD.EliminarTarea(id);
            }
        }

        return RedirectToAction("Index");
    }

    public IActionResult Compartir(int id)
    {
        string usuarioStr = HttpContext.Session.GetString("integrante");
        if (string.IsNullOrEmpty(usuarioStr))
        {
            return RedirectToAction("Index", "Home");
        }

        Tarea tarea = BD.LevantarTareaPorId(id);
        if (tarea == null)
        {
            return RedirectToAction("Index");
        }

        ViewBag.Tarea = tarea;
        return View();
    }

    [HttpPost]
    public IActionResult Compartir(int idTarea, string emailUsuario)
    {
        string usuarioStr = HttpContext.Session.GetString("integrante");
        if (string.IsNullOrEmpty(usuarioStr))
        {
            return RedirectToAction("Index", "Home");
        }

        Usuario usuarioOrigen = Objeto.StringToObject<Usuario>(usuarioStr);
        if (usuarioOrigen == null)
        {
            return RedirectToAction("Index", "Home");
        }

        Tarea tarea = BD.LevantarTareaPorId(idTarea);
        if (tarea == null || tarea.idCreador != usuarioOrigen.id)
        {
            return RedirectToAction("Index");
        }

        Usuario usuarioDestino = BD.LevantarUsuarioPorEmail(emailUsuario);
        if (usuarioDestino != null)
        {
            BD.CompartirTarea(idTarea, usuarioDestino.id);
        }

        return RedirectToAction("Index");
    }
}