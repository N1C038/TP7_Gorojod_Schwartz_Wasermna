using Microsoft.Data.SqlClient;
using Dapper;
namespace TP7_Gorojod_Schwartz_Waserman.Models;

public class BD {

    private string connectionString = "Server=localhost;Database=DBRedSocial;Trusted_Connection=True;TrustServerCertificate=True;";
    
    public void registrarUsuario(Usuario usuario) {
        using (SqlConnection connection = new SqlConnection(connectionString)) {
            connection.Execute("INSERT INTO Usuarios (nombre, contraseña, apellido, nombreUsuario) VALUES (@nombre, @contraseña, @apellido, @nombreUsuario)", usuario);
        }
    }
    public Usuario loginUsuario(string nombreUsuario, string contraseña) {
        using (SqlConnection connection = new SqlConnection(connectionString)) {
            return connection.QueryFirstOrDefault<Usuario>("SELECT * FROM Usuarios WHERE nombreUsuario = @nombreUsuario AND contraseña = @contraseña", new { nombreUsuario, contraseña });
        }
    }
    public void logOutUsuario() {
        // Logout logic moved to controller where HttpContext is available
    }
    public Publicacion crearPublicacion(Publicacion publicacion) {
        using (SqlConnection connection = new SqlConnection(connectionString)) {
            connection.Execute("INSERT INTO Publicaciones (idUsuario, titulo, descripcion, fechaPublicacion, imagen) VALUES (@idUsuario, @titulo, @descripcion, @fechaPublicacion, @imagen)", publicacion);
            return publicacion;
        }
    }
    
    public List<Publicacion> obtenerPublicacionesRecientes() {
        using (SqlConnection connection = new SqlConnection(connectionString)) {
            return connection.Query<Publicacion>("SELECT TOP 10 * FROM Publicaciones ORDER BY fechaPublicacion DESC;").ToList();
        }
    }
}