using Microsoft.Data.SqlClient;
using Dapper;

public class BD
{
    private static string _connectionString = @"Server=localhost;Database=TP06;Integrated Security=True;TrustServerCertificate=True;";
    
    public static List<Tarea> LevantarTarea(){
        List<Tarea> tareas = new List<Tarea>();
        using(SqlConnection connection = new SqlConnection(_connectionString)){
            string query = "SELECT * FROM Tarea";
            tareas = connection.Query<Tarea>(query).ToList();
        }
        return tareas;
    }

    public static Tarea LevantarTareaPorId(int id)
    {
        Tarea tarea = null;
        using(SqlConnection connection = new SqlConnection(_connectionString)){
            string query = "SELECT * FROM Tarea WHERE id = @TId";
            tarea = connection.QueryFirstOrDefault<Tarea>(query, new { TId = id });
        }
        return tarea;
    }

    public static void AgregarTarea(Tarea tarea)
    {
        string query = "INSERT INTO Tarea (descripcion, idCreador, terminado) VALUES (@TDescripcion, @TIdCreador, 0)";
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { TDescripcion = tarea.descripcion, TIdCreador = tarea.idCreador });
        }
    }

    public static void ModificarTarea(Tarea tarea)
    {
        string query = "UPDATE Tarea SET descripcion = @TDescripcion, terminado = @TTerminado WHERE id = @TId";
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { TDescripcion = tarea.descripcion, TTerminado = tarea.terminado, TId = tarea.id });
        }
    }

    public static void EliminarTarea(int id)
    {
        string query = "EXEC EliminarTarea @TId;";
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { TId = id });
        }
    }

    public static Usuario LevantarUsuario(string Email, string Password)
    {
        Usuario usuario = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Usuario WHERE usuario = @UUsuario AND [contraseña] = @UContraseña";
            usuario = connection.QueryFirstOrDefault<Usuario>(query, new { UUsuario = Email, UContraseña = Password });
        }
        return usuario;
    }

    public static Usuario LevantarUsuarioPorEmail(string Email)
    {
        Usuario usuario = null;
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Usuario WHERE usuario = @UUsuario";
            usuario = connection.QueryFirstOrDefault<Usuario>(query, new { UUsuario = Email });
        }
        return usuario;
    }

    public static void AgregarUsuario(Usuario usuario){
        string query = "INSERT INTO Usuario (usuario, [contraseña]) VALUES (@UUsuario, @UContraseña)";
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { UUsuario = usuario.usuario, UContraseña = usuario.contraseña });
        }
    }

    public static void CompartirTarea(int idTarea, int idUsuario)
    {
        string query = "INSERT INTO Usuario_Tarea (idUsuario, idTarea) VALUES (@TIdUsuario, @TIdTarea)";
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { TIdUsuario = idUsuario, TIdTarea = idTarea });
        }
    }

    public static List<Tarea> LevantarTareasPorUsuario(int idUsuario)
    {
        List<Tarea> tareas = new List<Tarea>();
        using(SqlConnection connection = new SqlConnection(_connectionString)){
            string query = @"SELECT t.* FROM Tarea t 
                           INNER JOIN Usuario_Tarea uxt ON t.id = uxt.idTarea 
                           WHERE uxt.idUsuario = @TIdUsuario";
            tareas = connection.Query<Tarea>(query, new { TIdUsuario = idUsuario }).ToList();
        }
        return tareas;
    }
}