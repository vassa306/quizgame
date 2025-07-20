using System.Net;
using System.Text.RegularExpressions;

namespace QuizApp.Models
{
    public class TriviaQuestion
    {
        public string Category { get; set; } = "";
        public string Type { get; set; } = String.Empty;
        public string Difficulty { get; set; } = String.Empty;
        public string Question { get; set; } = String.Empty;
        public string Correct_Answer { get; set; } = String.Empty;
        public List<string> Incorrect_Answers { get; set; } = new List<string>();

        private List<string> _allAnswers = new();

        public List<string> AllAnswers => _allAnswers;

        public bool IsMultipleChoice =>
            _allAnswers != null && _allAnswers.Count == 4;

        public void PrepareAnswers()
        {
            var cleanedAnswers = new List<string>();

            if (Incorrect_Answers != null)
                cleanedAnswers.AddRange(Incorrect_Answers.Select(Normalize));

            cleanedAnswers.Add(Normalize(Correct_Answer));

            _allAnswers = cleanedAnswers
                .Where(a => !string.IsNullOrWhiteSpace(a))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(_ => Guid.NewGuid())
                .ToList();
        }

        private string Normalize(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "";

            string decoded = WebUtility.HtmlDecode(input);
            return Regex.Replace(decoded, @"\s+", " ").Trim();
        }
    }
}
