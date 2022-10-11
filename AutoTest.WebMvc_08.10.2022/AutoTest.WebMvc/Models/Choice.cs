namespace AutoTest.WebMvc.Models;

public class Choice
{
    public int Id { get; set; }
    public string? Text { get; set; }
    public bool Answer { get; set; }
    public int QuestionId { get; set; }
}
