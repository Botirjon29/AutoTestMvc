using AutoTestJsonToDbConverter.Models;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;

var connection = new SqliteConnection("DATA SOURCE=AutoTest.Db");
connection.Open();

var cmd = connection.CreateCommand();

CreateTable();

void CreateTable()
{
    cmd.CommandText = "CREATE TABLE IF NOT EXISTS Questions" +
        "(id INTEGER PRIMARY KEY AUTOINCREMENT, " +
        "question_text TEXT, description TEXT, image TEXT)";
    cmd.ExecuteNonQuery();

    cmd.CommandText = "CREATE TABLE IF NOT EXISTS Choices(id INTEGER PRIMARY KEY AUTOINCREMENT, " +
        "text TEXT, answer BOOLEAN, questionId INTEGER)";
    cmd.ExecuteNonQuery();
}

var jsonData = File.ReadAllText("JsonData/uzlotin.json");
var questions = JsonConvert.DeserializeObject<List<QuestionEntity>>(jsonData);

Console.WriteLine(questions.Count);

if (questions == null)
{
    Console.WriteLine("Questions null");
    return;
}

foreach (var question in questions)
{
    AddQuestions(question);
}

void AddQuestions(QuestionEntity question)
{
    if (question.Media?.Name == null)
    {
        cmd.CommandText = "INSERT INTO Questions" +
            "(question_text, description) " +
            $"VALUES(\"{question.Question}\", \"{question.Description}\")";
        cmd.ExecuteNonQuery();
    }
    else
    {
        cmd.CommandText = "INSERT INTO Questions(question_text, description, image) " +
            $"VALUES(\"{question.Question}\", \"{question.Description}\", \"{question.Media.Name}\")";
        cmd.ExecuteNonQuery();
    }

    AddChoices(question.Choices!, question.Id);
}

void AddChoices(List<Choice> questionChoices, int questionId)
{
    foreach (var choices in questionChoices)
    {
        var command = connection.CreateCommand();

        command.CommandText = "INSERT INTO Choices(text, answer, questionId)" +
            "VALUES(@t, @a, @i)";
        command.Parameters.AddWithValue("@t", choices.Text);
        command.Parameters.AddWithValue("@a", choices.Answer);
        command.Parameters.AddWithValue("@i", questionId);
        command.Prepare();

        command.ExecuteNonQuery();
    }
}
