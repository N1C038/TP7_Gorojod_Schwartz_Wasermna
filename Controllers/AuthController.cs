using Microsoft.AspNetCore.Mvc;
using TP7_Gorojod_Schwartz_Waserman.Models;

namespace TP7_Gorojod_Schwartz_Waserman.Controllers;

public class AuthController : Controller
{
    private BD bd = new BD();

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
            return RedirectToAction("Index", "Home");
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
        return RedirectToAction("Index", "Home");
    }
}