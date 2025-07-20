namespace QuizApp.Models
{
    public class ScoreEntry
    {
        public string PlayerName { get; set; } = String.Empty;
        public int Score { get; set; }
        public int Total { get; set; }
        public DateTime Date { get; set; }
    }
}
