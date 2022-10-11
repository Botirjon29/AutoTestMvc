using AutoTest.WebMvc.Models;
using Microsoft.Data.Sqlite;

namespace AutoTest.WebMvc.Repositories;

public class TicketsRepository
{
    private const string ConString = "DATA SOURCE=avtotest.db";
    private SqliteConnection _connection;

    public TicketsRepository()
    {
        _connection = new SqliteConnection(ConString);
    }

    public void CreateTicketTable()
    {
        _connection.Open();
        var cmd = _connection.CreateCommand();

        cmd.CommandText = "CREATE TABLE IF NOT EXISTS Tickets" +
            "(id INTEGER PRIMARY KEY AUTOINCREMENT," +
            "user_id INTEGER, from_index INTEGER," +
            "questions_count INTEGER, correct_count INTEGER)";
        cmd.ExecuteNonQuery();

        cmd.CommandText = "CREATE TABLE IF NOT EXISTS Tickets_Data" +
            "(id INTEGER PRIMARY KEY AUTOINCREMENT," +
            "ticket_id INTEGER, question_id INTEGER," +
            "choice_id INTEGER, answer BOOLEAN)";
        cmd.ExecuteNonQuery();
        _connection.Close();
    }

    public void InsertTicket(Ticket ticket)
    {
        _connection.Open();
        var cmd = _connection.CreateCommand();

        cmd.CommandText = "INSERT INTO Tickets" +
            "(user_id, from_index, questions_count, correct_count) " +
            "VALUES(@uId, @fIndex, @qCount, @cCount)";
        cmd.Parameters.AddWithValue("@uId", ticket.UserId);
        cmd.Parameters.AddWithValue("@fIndex", ticket.FromIndex);
        cmd.Parameters.AddWithValue("@qCount", ticket.QuestionsCount);
        cmd.Parameters.AddWithValue("@cCount", ticket.CorrectCount);
        cmd.Prepare();

        cmd.ExecuteNonQuery();
        _connection.Close();
    }

    public Ticket GetTicketById(int id, int userId)
    {
        _connection.Open();
        var cmd = _connection.CreateCommand();
        var ticket = new Ticket();
        cmd.CommandText = "SELECT * FROM tickets WHERE id = @id AND user_id = @uId";
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@uId", userId);
        cmd.Prepare();

        var data = cmd.ExecuteReader();

        while (data.Read())
        {
            ticket.Id = data.GetInt32(0);
            ticket.UserId = data.GetInt32(1);
            ticket.FromIndex = data.GetInt32(2);
            ticket.QuestionsCount = data.GetInt32(3);
            ticket.CorrectCount = data.GetInt32(4);
        }

        _connection.Close();
        return ticket;
    }

    public int GetLastRowId()
    {
        _connection.Open();
        int id = 0;
        var cmd = _connection.CreateCommand();

        cmd.CommandText = "SELECT id FROM Tickets ORDER BY id DESC LIMIT 1";

        var data = cmd.ExecuteReader();

        while (data.Read())
        {
            id = data.GetInt32(0);
        }

        _connection.Close();
        return id;
    }
}
