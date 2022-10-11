using AutoTest.WebMvc.Models;
using Microsoft.Data.Sqlite;

namespace AutoTest.WebMvc.Repositories;

public class QuestionRepository
{
    private string ConnectionString = "DATA SOURCE=avtotest.Db";
    private SqliteConnection _connection;

    public QuestionRepository()
    {
        _connection = new SqliteConnection(ConnectionString);
    }

    public int GetQuestionsCount()
    {
        _connection.Open();
        var _command = _connection.CreateCommand();
         _command.CommandText = "SELECT COUNT(*) FROM Questions";
        var data = _command.ExecuteReader();

        while (data.Read())
        {
            var qCount = data.GetInt32(0);
            _connection.Close();
            return qCount;
        }

        _connection.Close();
        return 0;
    }

    public QuestionEntity GetQuestionById(int id)
    {
        _connection.Open();
        var _command = _connection.CreateCommand();
        var question = new QuestionEntity();
        _command.CommandText = "SELECT * FROM questions WHERE id = @id";
        _command.Parameters.AddWithValue("@id", id);
        _command.Prepare();

        var data = _command.ExecuteReader();

        while (data.Read())
        {
            question.Id = data.GetInt32(0);
            question.Question = data.GetString(1);
            question.Description = data.GetString(2);
            question.Image = data.IsDBNull(3) ? null : data.GetString(3);
        }
        question.Choices = GetQuestionChoices(id);

        _connection.Close();
        return question;
    }

    private List<Choice> GetQuestionChoices(int questionId)
    {
        _connection.Open();
        var _command = _connection.CreateCommand();
        var choices = new List<Choice>();

        _command.CommandText = "SELECT * FROM choices WHERE questionId = @id";
        _command.Parameters.AddWithValue("@id", questionId);
        _command.Prepare();

        var data = _command.ExecuteReader();

        while (data.Read())
        {
            var choice = new Choice();

            choice.Id = data.GetInt32(0);
            choice.Text = data.GetString(1);
            choice.Answer = data.GetBoolean(2);

            choices.Add(choice);
        }

        _connection.Close();
        return choices;
    }

    public List<QuestionEntity> GetQuestionsRange(int from, int count)
    {
        _connection.Open();
        var questions = new List<QuestionEntity>();
        for (int i = from; i < from + count; i++)
        {
            questions.Add(GetQuestionById(i));
        }

        _connection.Close();
        return questions;
    }
}
