using Microsoft.Data.SqlClient;
using Dapper;

public static class BD
{
    private static string _connectionString = @"Server=localhost;Database=Tp06;Integrated Security=True;TrustServerCertificate=True;";
    public static List<Usuario> usuarios = new List<Usuario>();
    public static List<Usuario> ObtenerUsuarios()
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Usuario";
            usuarios = connection.Query<Usuario>(query).ToList();
        }
        return usuarios;
    }
    public static Usuario verificarCuenta(string usuario, string contraseña)
    {
        Usuario usuarioEncontrado = null;
        for (int i = 0; i < usuarios.Count; i++)
        {
            if (usuarios[i].usuario == usuario && usuarios[i].contraseña == contraseña)
            {
                usuarioEncontrado = usuarios[i];
            }
        }
        return usuarioEncontrado;
    }

}