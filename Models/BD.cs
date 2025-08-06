using Microsoft.Data.SqlClient;
using Dapper;

public static class BD
{
  private static string _connectionString = @"Server=localhost;DateBase=Tp06;Integrated Security=True;TrustServerCertificate=True;";
  public static List<Usuario> usuarios = new List<Usuario>(); 
public static List<Usuario> ObtenerUsuarios()
{
  using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Usuario";
            usuarios = connection.Query<Usuario>(query).ToList();
        }
        return usuarios;
}
   public static Usuario verificarCuenta(int id, string contraseña)
    {
        Usuario personaEncontrada = null;
        for(int i = 0; i < usuarios.Count; i++)
        {
            if(usuarios[i].id == id && usuarios[i].contraseña == contraseña)
            {
                personaEncontrada = usuarios[i];
            }
        }
        return personaEncontrada;
    }

}