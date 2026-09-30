using Microsoft.Data.SqlClient;
using Dapper;
namespace TP7_Gorojod_Schwartz_Waserman.Models;

public class BD {

    string connectionString = "Server=localhost;Database=DBSocial;Trusted_Connection=True;TrustServerCertificate=True;";
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
        HttpContext.Session.Remove(idUsuario);
    }
    public Publicacion crearPublicacion(Publicacion publicacion) {
                //acá hay que validar si el usuario está logueado para que pueda crear una publicación
              using (SqlConnection connection = new SqlConnection(connectionString)) {
                connection.Execute("INSERT INTO Publicaciones (idUsuario, titulo, descripcion, fechaPublicacion, imagen) VALUES (@idUsuario, @titulo, @descripcion, @fechaPublicacion, @imagen)", publicacion);
        if (HttpContext.Session.GetInt32(idUsuario) == null) {
            throw new Exception("Usuario no logueado");
        }
        else{
            using (SqlConnection connection = new SqlConnection(connectionString)) {
            connection.Execute("INSERT INTO Publicaciones (idUsuario, titulo, descripcion, fechaPublicacion, imagen) VALUES (@idUsuario, @titulo, @descripcion, @fechaPublicacion, @imagen)", publicacion);
            return publicacion;
            }
        }
      }
    }
    
}