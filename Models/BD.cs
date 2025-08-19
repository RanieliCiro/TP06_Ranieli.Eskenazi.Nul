using Microsoft.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using System.Linq;

public class BD
{
    private static string _connectionString =@"Server=localhost;Database=Tp06;Integrated Security=True;TrustServerCertificate=True;";

    public static List<Tarea> LevantarTarea()
    {
        using var connection = new SqlConnection(_connectionString);
        const string query = "SELECT * FROM Tarea";
        return connection.Query<Tarea>(query).ToList();
    }

    public static Tarea LevantarTareaPorId(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        const string query = "SELECT * FROM Tarea WHERE id = @TId";
        return connection.QueryFirstOrDefault<Tarea>(query, new { TId = id });
    }

    public static int CrearTarea(Tarea nueva)
    {
        using var connection = new SqlConnection(_connectionString);
        int nuevoId = connection.QuerySingle<int>("SELECT ISNULL(MAX(id), 0) + 1 FROM Tarea");

        const string insert = @"INSERT INTO Tarea (id, descripcion, idCreador, terminado) VALUES (@id, @descripcion, @idCreador, 0);";
        connection.Execute(insert, new {id = nuevoId, descripcion = nueva.descripcion, idCreador = nueva.idCreador});
        return nuevoId;
    }

    public static void AgregarTarea(Tarea tarea)
    {
        CrearTarea(tarea);
    }

    public static void ModificarTarea(Tarea tarea)
    {
        const string query = @"UPDATE Tarea SET descripcion = @TDescripcion, terminado   = @TTerminado WHERE id = @TId";
        using var connection = new SqlConnection(_connectionString);
        connection.Execute(query, new {TDescripcion = tarea.descripcion, TTerminado = tarea.terminado, TId = tarea.id});
    }

    public static void EliminarTarea(int id)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Execute("DELETE FROM Usuario_Tarea WHERE idTarea = @TId", new { TId = id });
        connection.Execute("DELETE FROM Tarea WHERE id = @TId", new { TId = id });
    }

    public static List<Tarea> LevantarTareasPorUsuario(int idUsuario)
    {
        using var connection = new SqlConnection(_connectionString);
        const string query = @"SELECT t.* FROM Tarea t INNER JOIN Usuario_Tarea ut ON t.id = ut.idTarea WHERE ut.idUsuario = @TIdUsuario";
        return connection.Query<Tarea>(query, new { TIdUsuario = idUsuario }).ToList();
    }

    public static void CompartirTarea(int idTarea, int idUsuario)
    {
        using var cn = new SqlConnection(_connectionString);
        const string sql = @"
            IF NOT EXISTS (SELECT 1 FROM Usuario_Tarea WHERE idUsuario = @idUsuario AND idTarea = @idTarea)
                INSERT INTO Usuario_Tarea (idUsuario, idTarea) VALUES (@idUsuario, @idTarea);";
        cn.Execute(sql, new { idUsuario, idTarea });
    }

    public static Usuario LevantarUsuario(string usuario, string contraseña)
    {
        using var connection = new SqlConnection(_connectionString);
        const string query = "SELECT * FROM Usuario WHERE usuario = @UUsuario AND [contraseña] = @UContraseña";
        return connection.QueryFirstOrDefault<Usuario>(query, new { UUsuario = usuario, UContraseña = contraseña });
    }

    public static Usuario LevantarUsuarioPorNombre(string usuario)
    {
        using var connection = new SqlConnection(_connectionString);
        const string query = "SELECT * FROM Usuario WHERE usuario = @UUsuario";
        return connection.QueryFirstOrDefault<Usuario>(query, new { UUsuario = usuario });
    }

    public static Usuario LevantarUsuarioPorEmail(string Email)
    {
        using var connection = new SqlConnection(_connectionString);
        const string query = "SELECT * FROM Usuario WHERE usuario = @UUsuario";
        return connection.QueryFirstOrDefault<Usuario>(query, new { UUsuario = Email });
    }

    public static int AgregarUsuario(Usuario usuario)
    {
        using var connection = new SqlConnection(_connectionString);
        int nuevoId = connection.QuerySingle<int>("SELECT ISNULL(MAX(id), 0) + 1 FROM Usuario");

        const string sql = @"INSERT INTO Usuario (id, usuario, [contraseña]) VALUES (@id, @usuario, @contraseña)";
        connection.Execute(sql, new {id = nuevoId, usuario = usuario.usuario, contraseña = usuario.contraseña});
        return nuevoId;
    }
}
