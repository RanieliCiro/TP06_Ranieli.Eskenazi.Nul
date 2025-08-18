using Microsoft.Data.SqlClient;
using Dapper;
using Tp06.Models;

public static class BD
{
    private static string _connectionString = @"Server=localhost;Database=Tp06;Integrated Security=True;TrustServerCertificate=True;";

    public static List<Usuario> ObtenerUsuarios()
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Usuario";
            return connection.Query<Usuario>(query).ToList();
        }
    }

    public static Usuario VerificarCuenta(string usuario, string contraseña)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Usuario WHERE usuario = @usuario AND contraseña = @contraseña";
            return connection.QueryFirstOrDefault<Usuario>(query, new { usuario, contraseña });
        }
    }

    public static void RegistrarUsuario(Usuario nuevo)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "INSERT INTO Usuario (usuario, contraseña) VALUES (@usuario, @contraseña)";
            connection.Execute(query, nuevo);
        }
    }

    public static List<Tarea> ObtenerTareasDeUsuario(int idUsuario)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = @"SELECT T.* FROM Tarea T INNER JOIN Usuario_Tarea UT ON T.id = UT.idTarea WHERE UT.idUsuario = @idUsuario AND T.fechaEliminacion IS NULL";
            return connection.Query<Tarea>(query, new { idUsuario }).ToList();
        }
    }

    public static int CrearTarea(Tarea nueva)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = @"INSERT INTO Tarea (descripcion, idCreador, terminado, fechaCreacion) VALUES (@descripcion, @idCreador, 0, GETDATE()); SELECT CAST(SCOPE_IDENTITY() as int)";
            return connection.QuerySingle<int>(query, nueva);
        }
    }

    public static void EditarTarea(Tarea t)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = @"UPDATE Tarea SET descripcion = @descripcion, fechaModificacion = GETDATE() WHERE id = @id";
            connection.Execute(query, t);
        }
    }

    public static void MarcarTerminada(int idTarea)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "UPDATE Tarea SET terminado = 1, fechaModificacion = GETDATE() WHERE id = @idTarea";
            connection.Execute(query, new { idTarea });
        }
    }
    
    public static void EliminarTarea(int idTarea)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "UPDATE Tarea SET fechaEliminacion = GETDATE() WHERE id = @idTarea";
            connection.Execute(query, new { idTarea });
        }
    }

    public static List<Tarea> ObtenerEliminadas(int idUsuario)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = @"SELECT T.* FROM Tarea T INNER JOIN Usuario_Tarea UT ON T.id = UT.idTarea WHERE UT.idUsuario = @idUsuario AND T.fechaEliminacion IS NOT NULL";
            return connection.Query<Tarea>(query, new { idUsuario }).ToList();
        }
    }

    public static void RecuperarTarea(int idTarea)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "UPDATE Tarea SET fechaEliminacion = NULL WHERE id = @idTarea";
            connection.Execute(query, new { idTarea });
        }
    }

    public static void CompartirTarea(int idTarea, int idUsuario)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "INSERT INTO Usuario_Tarea (idUsuario, idTarea) VALUES (@idUsuario, @idTarea)";
            connection.Execute(query, new { idUsuario, idTarea });
        }
    }
}
