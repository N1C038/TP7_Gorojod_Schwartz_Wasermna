using Microsoft.AspNetCore.Mvc;
using TP7_Gorojod_Schwartz_Waserman.Models;

namespace TP7_Gorojod_Schwartz_Waserman.Controllers;

public class PublicacionController : Controller
{
    private BD bd = new BD();

    [HttpGet]
    public IActionResult Create()
    {
        if (HttpContext.Session.GetInt32("usuarioId") == null)
        {
            return RedirectToAction("Login", "Auth");
        }
        return View();
    }

    [HttpPost]
    public IActionResult Create(Publicacion publicacion, IFormFile imagen)
    {
        int? usuarioId = HttpContext.Session.GetInt32("usuarioId");

        if (usuarioId == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        if (string.IsNullOrEmpty(publicacion.titulo) || string.IsNullOrEmpty(publicacion.descripcion))
        {
            ViewBag.Error = "El título y descripción son requeridos";
            return View();
        }

        publicacion.idUsuario = usuarioId.Value;
        publicacion.fechaPublicacion = DateTime.Now;

        // Procesar imagen si se subió
        if (imagen != null && imagen.Length > 0)
        {
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + imagen.FileName;
            string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);
            
            // Crear directorio si no existe
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images"));
            
            using (var fileStream = new FileStream(uploadPath, FileMode.Create))
            {
                imagen.CopyTo(fileStream);
            }
            publicacion.imagen = "/images/" + uniqueFileName;
        }
        else
        {
            publicacion.imagen = "/images/default.jpg";
        }

        try
        {
            bd.crearPublicacion(publicacion);
            ViewBag.Success = "Publicación creada correctamente";
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Error al crear la publicación: " + ex.Message;
            return View();
        }
    }
}
