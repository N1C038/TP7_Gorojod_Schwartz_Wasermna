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

    public IActionResult logOutUsuario(int idUsuario) {
        HttpContext.Session.Remove("usuarioId");
        HttpContext.Session.Remove("nombreUsuario");
        HttpContext.Session.Remove("nombre");
        return RedirectToAction("Index", "Home");
    }
}
