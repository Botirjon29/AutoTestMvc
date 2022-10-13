using AutoTest.WebMvc.Models;
using Microsoft.Data.Sqlite;

namespace AutoTest.WebMvc.Repositories;

public class UsersRepository
{

    private const string ConnectionString = "DATA SOURCE=Users.Db";
    private SqliteConnection _connection;
    private SqliteCommand _command;

    public UsersRepository()
    {
        OpenConnection();
        CreateUsersTable();
    }

    public void OpenConnection()
    {
        _connection = new SqliteConnection(ConnectionString);
        _connection.Open();
        _command = _connection.CreateCommand();
    }

    public void CreateUsersTable()
    {
        _command.CommandText = "CREATE TABLE IF NOT EXISTS Users(id INTEGER PRIMARY KEY AUTOINCREMENT," +
            "name TEXT, phone TEXT, password TEXT, image TEXT)";
        _command.ExecuteNonQuery();
    }

    public void InsertUser(User user)
    {
        _command.CommandText = "INSERT INTO Users(name, phone, password, image)" +
            "VALUES(@n, @ph, @p, @i)";
        _command.Parameters.AddWithValue("@n", user.Name);
        _command.Parameters.AddWithValue("@ph", user.Phone);
        _command.Parameters.AddWithValue("@p", user.Password);
        _command.Parameters.AddWithValue("@i", user.Image);
        _command.Prepare();

        
        _command.ExecuteNonQuery();
    }

    public List<User> GetUsers()
    {
        var users = new List<User>();

        _command.CommandText = "SELECT * FROM Users";
        var data = _command.ExecuteReader();

        while (data.Read())
        {
            var user = new User()
            {
                Id = data.GetInt32(0),
                Name = data.GetString(1),
                Phone = data.GetString(2),
                Image = data.GetString(4),
            };

            users.Add(user);
        }

        return users;
    }

    public User GetUserByIndex(int index)
    {
        var user = new User();

        _command.CommandText = $"SELECT * FROM Users WHERE id = {index}";
        var data = _command.ExecuteReader();

        while (data.Read())
        {
            user.Id = data.GetInt32(0);
            user.Name = data.GetString(1);
            user.Phone = data.GetString(2);
        }
        return user;
    }

    public User GetUserByPhone(string phoneNumber)
    {
        var user = new User();

        _command.CommandText = $"SELECT * FROM Users WHERE phone = '{phoneNumber}'";

        var data = _command.ExecuteReader();

        while (data.Read())
        {
            user.Id = data.GetInt32(0);
            user.Name = data.GetString(1);
            user.Phone = data.GetString(2);
            user.Password = data.GetString(3);
            user.Image = data.GetString(4);
        }
        return user;
    }


    public void DeleteUser(int id)
    {
        _command.CommandText = $"DELETE FROM Users WHERE id = {id}";
        _command.ExecuteNonQuery();
    }


    public void UpdateUser(User user)
    {
        _command.CommandText = "UPDATE Users SET name = @n, phone = @ph, password = @p, image = @im WHERE id = @i";
        _command.Parameters.AddWithValue("@n", user.Name);
        _command.Parameters.AddWithValue("@ph", user.Phone);
        _command.Parameters.AddWithValue("@p", user.Password);
        _command.Parameters.AddWithValue("@im", user.Image);
        _command.Parameters.AddWithValue("@i", user.Id); 
        _command.Prepare();

        _command.ExecuteNonQuery();
    }
}
