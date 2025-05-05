namespace Cogni.Models;

public class QuestionModel
{
    public int IdMbtiQuestion { get; set; }

    public string? Question { get; set; }

    public QuestionModel(int idMbtiQuestion, string? question)
    {
        IdMbtiQuestion = idMbtiQuestion;
        Question = question;
    }
}
public class TestModel
{
    public List<QuestionModel> Questions { get; set; }
    public TestModel(List<QuestionModel> questions)
    {
        Questions = questions;
    }
}

public class TestResultModel
{
    public string? TypeMbti { get; set; }
    public TestResultModel(string? typeMbti)
    {
        TypeMbti = typeMbti;
    }
}

