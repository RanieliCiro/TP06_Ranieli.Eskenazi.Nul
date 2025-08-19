using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Tp06.Models;

namespace Tp06.Controllers
{
    public class TareasController : Controller
    {
        public IActionResult Index()
        {
            string usuarioStr = HttpContext.Session.GetString("integrante");
            if (string.IsNullOrEmpty(usuarioStr)) return RedirectToAction("Index", "Home");

            Usuario usuario = Objeto.StringToObject<Usuario>(usuarioStr);
            var tareas = BD.LevantarTareasPorUsuario(usuario.id);

            ViewBag.Usuario = usuario;
            ViewBag.Tareas = tareas;
            return View();
        }

        public IActionResult Agregar()
        {
            string usuarioStr = HttpContext.Session.GetString("integrante");
            if (string.IsNullOrEmpty(usuarioStr)) return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult Agregar(string descripcion)
        {
            string usuarioStr = HttpContext.Session.GetString("integrante");
            if (string.IsNullOrEmpty(usuarioStr)) return RedirectToAction("Index", "Home");

            Usuario usuario = Objeto.StringToObject<Usuario>(usuarioStr);
            if (usuario == null) return RedirectToAction("Index", "Home");

            var nueva = new Tarea
            {
                descripcion = descripcion,
                idCreador = usuario.id,
                terminado = false
            };

            int idTarea = BD.CrearTarea(nueva);
            BD.CompartirTarea(idTarea, usuario.id);

            return RedirectToAction("Index");
        }

        public IActionResult Editar(int id)
        {
            string usuarioStr = HttpContext.Session.GetString("integrante");
            if (string.IsNullOrEmpty(usuarioStr)) return RedirectToAction("Index", "Home");

            Tarea tarea = BD.LevantarTareaPorId(id);
            if (tarea == null) return RedirectToAction("Index");

            ViewBag.Tarea = tarea;
            return View();
        }

        [HttpPost]
        public IActionResult Editar(int id, string descripcion, bool terminado)
        {
            string usuarioStr = HttpContext.Session.GetString("integrante");
            if (string.IsNullOrEmpty(usuarioStr)) return RedirectToAction("Index", "Home");

            Tarea tarea = BD.LevantarTareaPorId(id);
            if (tarea != null)
            {
                tarea.descripcion = descripcion;
                tarea.terminado = terminado;
                BD.ModificarTarea(tarea);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Eliminar(int idTarea)
        {
            string usuarioStr = HttpContext.Session.GetString("integrante");
            if (string.IsNullOrEmpty(usuarioStr)) return RedirectToAction("Index", "Home");

            Usuario actual = Objeto.StringToObject<Usuario>(usuarioStr);
            Tarea tarea = BD.LevantarTareaPorId(idTarea);
            if (tarea == null || tarea.idCreador != actual.id) return RedirectToAction("Index");

            BD.EliminarTarea(idTarea);
            return RedirectToAction("Index");
        }

        public IActionResult Compartir(int id)
        {
            string usuarioStr = HttpContext.Session.GetString("integrante");
            if (string.IsNullOrEmpty(usuarioStr)) return RedirectToAction("Index", "Home");

            Usuario actual = Objeto.StringToObject<Usuario>(usuarioStr);
            Tarea tarea = BD.LevantarTareaPorId(id);
            if (tarea == null || tarea.idCreador != actual.id) return RedirectToAction("Index");

            ViewBag.Tarea = tarea;
            return View();
        }

        [HttpPost]
        public IActionResult Compartir(int idTarea, string nombreUsuario)
        {
            string usuarioStr = HttpContext.Session.GetString("integrante");
            if (string.IsNullOrEmpty(usuarioStr)) return RedirectToAction("Index", "Home");

            Usuario origen = Objeto.StringToObject<Usuario>(usuarioStr);
            Tarea tarea = BD.LevantarTareaPorId(idTarea);
            if (tarea == null || tarea.idCreador != origen.id) return RedirectToAction("Index");

            if (!string.IsNullOrWhiteSpace(nombreUsuario))
            {
                var separadores = new[] { ',', ';', '\n', '\r' };
                var nombres = nombreUsuario
                                .Split(separadores, StringSplitOptions.RemoveEmptyEntries)
                                .Select(s => s.Trim())
                                .Where(s => s.Length > 0)
                                .Distinct(StringComparer.OrdinalIgnoreCase);

                foreach (string nombre in nombres)
                {
                    var destino = BD.LevantarUsuarioPorNombre(nombre);
                    if (destino != null)
                        BD.CompartirTarea(idTarea, destino.id);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
