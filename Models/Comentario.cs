namespace TP7_Gorojod_Schwartz_Waserman.Models;

public class Comentario
{
    public int Id { get; set; }
    public string texto { get; set; }
    public DateTime fechaComentario { get; set; }
    public int publicacionId { get; set; }
    public int usuarioId { get; set; }

    public Comentario()
    {
        
    }
}