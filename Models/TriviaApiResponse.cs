namespace QuizApp.Models
{
    public class TriviaApiResponse
    {
        public int Response_Code { get; set; }
        public List<TriviaQuestion>? Results { get; set; }
    }
}
