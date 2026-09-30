using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TP7_Gorojod_Schwartz_Waserman.Models;

namespace TP7_Gorojod_Schwartz_Waserman.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private BD bd = new BD();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        List<Publicacion> publicaciones = bd.obtenerPublicacionesRecientes();
        return View(publicaciones);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // ============ AUTH ==============
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string nombreUsuario, string contraseña)
    {
        if (string.IsNullOrEmpty(nombreUsuario) || string.IsNullOrEmpty(contraseña))
        {
            ViewBag.Error = "Por favor completa todos los campos";
            return View();
        }

        Usuario usuario = bd.loginUsuario(nombreUsuario, contraseña);

        if (usuario != null)
        {
            HttpContext.Session.SetInt32("usuarioId", usuario.Id);
            HttpContext.Session.SetString("nombreUsuario", usuario.nombreUsuario);
            HttpContext.Session.SetString("nombre", usuario.nombre);
            return RedirectToAction("Index");
        }
        else
        {
            ViewBag.Error = "Usuario o contraseña incorrectos";
            return View();
        }
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(Usuario usuario)
    {
        if (string.IsNullOrEmpty(usuario.nombreUsuario) || string.IsNullOrEmpty(usuario.contraseña) || 
            string.IsNullOrEmpty(usuario.nombre) || string.IsNullOrEmpty(usuario.apellido))
        {
            ViewBag.Error = "Por favor completa todos los campos";
            return View();
        }

        try
        {
            bd.registrarUsuario(usuario);
            ViewBag.Success = "Usuario registrado correctamente. Por favor inicia sesión.";
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Error al registrar usuario: " + ex.Message;
            return View();
        }
    }

    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

    // ============ PUBLICACIONES ==============
    [HttpGet]
    public IActionResult Create()
    {
        if (HttpContext.Session.GetInt32("usuarioId") == null)
        {
            return RedirectToAction("Login");
        }
        return View();
    }

    [HttpPost]
    public IActionResult Create(Publicacion publicacion, IFormFile imagen)
    {
        int? usuarioId = HttpContext.Session.GetInt32("usuarioId");

        if (usuarioId == null)
        {
            return RedirectToAction("Login");
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
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ViewBag.Error = "Error al crear la publicación: " + ex.Message;
            return View();
        }
    }
}
