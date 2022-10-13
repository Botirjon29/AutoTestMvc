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
            "questions_count INTEGER, correct_count INTEGER, is_training BOOLEAN)";
        cmd.ExecuteNonQuery();

        cmd.CommandText = "CREATE TABLE IF NOT EXISTS Tickets_Data" +
            "(id INTEGER PRIMARY KEY AUTOINCREMENT," +
            "ticket_id INTEGER, question_id INTEGER," +
            "choice_id INTEGER, answer BOOLEAN)";
        cmd.ExecuteNonQuery();
        _connection.Close();
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

    public void InsertTicket(Ticket ticket)
    {
        _connection.Open();
        var cmd = _connection.CreateCommand();

        cmd.CommandText = "INSERT INTO Tickets" +
            "(user_id, from_index, questions_count, correct_count, is_training) " +
            "VALUES(@uId, @fIndex, @qCount, @cCount, @isT)";
        cmd.Parameters.AddWithValue("@uId", ticket.UserId);
        cmd.Parameters.AddWithValue("@fIndex", ticket.FromIndex);
        cmd.Parameters.AddWithValue("@qCount", ticket.QuestionsCount);
        cmd.Parameters.AddWithValue("@cCount", ticket.CorrectCount);
        cmd.Parameters.AddWithValue("@isT", ticket.IsTraining);
        cmd.Prepare();

        cmd.ExecuteNonQuery();
        _connection.Close();
    }

    public void InsertTicketsData(TicketData ticketData)
    {
        _connection.Open();
        var cmd = _connection.CreateCommand();

        cmd.CommandText = "INSERT INTO Tickets_Data" +
            "(ticket_id, question_id, choice_id, answer) " +
            "VALUES(@t_i, @q_i, @ch_i, @a)";
        cmd.Parameters.AddWithValue("@t_i", ticketData.TicketId);
        cmd.Parameters.AddWithValue("@q_i", ticketData.QuestionId);
        cmd.Parameters.AddWithValue("@ch_i", ticketData.ChoiceId);
        cmd.Parameters.AddWithValue("@a", ticketData.Answer);
        cmd.Prepare();
        cmd.ExecuteNonQuery();

        _connection.Close();
    }

    public TicketData GetTicketDataByQuIdAndTicId(int ticketId, int questionId)
    {
        _connection.Open();
        var cmd = _connection.CreateCommand();

        cmd.CommandText = $"SELECT * FROM Tickets_Data WHERE ticket_id = {ticketId} AND question_id = {questionId}";

        var data = cmd.ExecuteReader();

        var ticketData = new TicketData();

        while (data.Read())
        {
            ticketData.Id = data.GetInt32(0);
            ticketData.TicketId = data.GetInt32(1);
            ticketData.QuestionId = data.GetInt32(2);
            ticketData.ChoiceId = data.GetInt32(3);
            ticketData.Answer = data.GetBoolean(4);
        }
        _connection.Close();
        if (ticketData.QuestionId == questionId)
        {
        return ticketData;
        }
        return null;
    }

    public int GetTicketAnswersCount(int ticketId)
    {
        _connection.Open();
        var cmd = _connection.CreateCommand();

        cmd.CommandText = $"SELECT COUNT(*) FROM tickets_data WHERE ticket_id = {ticketId}";
        var data = cmd.ExecuteReader();

        while (data.Read())
        {
            var count = data.GetInt32(0);
            _connection.Close();
            return count;
        }
        _connection.Close();
        return 0;
    }

    public void UpdateTicketQuestionsCorrectCount(int ticketId)
    {
        _connection.Open();
        var cmd = _connection.CreateCommand();

        cmd.CommandText = $"UPDATE Tickets SET correct_count = correct_count + 1 WHERE id = {ticketId}";
        cmd.ExecuteNonQuery();

        _connection.Close();
    }

    public List<TicketData> GetTicketDataById(int ticketId)
    {
        var ticketDatas = new List<TicketData>();
        _connection.Open();
        var cmd = _connection.CreateCommand();

        cmd.CommandText = $"SELECT question_id, choice_id, answer FROM Tickets_Data WHERE ticket_id = {ticketId}";
        var data = cmd.ExecuteReader();

        while (data.Read())
        {
            var ticketData = new TicketData()
            {
                QuestionId = data.GetInt32(0),
                ChoiceId = data.GetInt32(1),
                Answer = data.GetBoolean(2) 
            };

            ticketDatas.Add(ticketData);
        }

        _connection.Close();

        return ticketDatas;
    }

    public List<Ticket> GetTicketsByUserId(int userId)
    {
        var tickets = new List<Ticket>();

        _connection.Open();
        var cmd = _connection.CreateCommand();

        cmd.CommandText = $"SELECT id, from_index, questions_count, correct_count FROM Tickets WHERE user_id = {userId} AND is_training = true";
        var data = cmd.ExecuteReader();

        while (data.Read())
        {
            var ticket = new Ticket()
            {
                Id = data.GetInt32(0),
                FromIndex = data.GetInt32(1),
                QuestionsCount = data.GetInt32(2),
                CorrectCount = data.GetInt32(3),
                UserId = userId
            };

            tickets.Add(ticket);
        }

        _connection.Close();

        return tickets;
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

    public void InsertUserTrainingTickets(int userId, int TicketsCount, int ticketQuestionsCount)
    {
        for (int i = 0; i < TicketsCount; i++)
        {
            InsertTicket(new Ticket()
            {
                UserId = userId,
                CorrectCount = 0,
                IsTraining = true,
                FromIndex = i * ticketQuestionsCount + 1,
                QuestionsCount = ticketQuestionsCount
            });
        }
    }
}
