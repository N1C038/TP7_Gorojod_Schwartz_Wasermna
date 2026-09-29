namespace TP7_Gorojod_Schwartz_Waserman.Models;

public class Publicacion
{
    public int Id { get; set; }
    public int idUsuario { get; set; }
    public string titulo { get; set; }
    public string descripcion { get; set; }
    public DateTime fechaPublicacion { get; set; }
    public string imagen { get; set; }

    public Publicacion()
    {
       
    }
}