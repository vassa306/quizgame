using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using QuizApp.Models;
using QuizApp.Services;

namespace QuizApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<TriviaQuestion> Questions { get; set; }
        [BindProperty]
        public List<string> Answers { get; set; } = new();

        [BindProperty]
        public List<string> CorrectAnswers { get; set; } = new();

        [BindProperty]
        public string PlayerName { get; set; }

        public int? Score { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            using var client = new HttpClient();
            string url = "https://opentdb.com/api.php?amount=5&type=multiple";
            var response = await client.GetStringAsync(url);
            var trivia = JsonConvert.DeserializeObject<TriviaApiResponse>(response);
            Questions = trivia?.Results ?? new List<TriviaQuestion>();
            Questions.Insert(0, new TriviaQuestion
            {
                Question = "What is the capital of Middle-earth?",
                Correct_Answer = "Minas Tirith",
                Incorrect_Answers = new List<string>
                {
                    "Rivendell",
                    "Hobbiton",
                    "Gondor"
                }
            });
            foreach (var question in Questions)
            {
                question.PrepareAnswers();
            }
        }

        

        public void OnPostSubmitAnswers()
        {
            var encodedJson = Request.Form["SerializedQuestions"];
            var json = System.Net.WebUtility.HtmlDecode(encodedJson);
            Console.WriteLine($"json: {json}");
           

            if (string.IsNullOrWhiteSpace(json))
            {
                Questions = new();
                Score = null;
                ModelState.AddModelError("", "Missing quiz data. Please reload the page.");
                return;
            }

            try
            {
                Questions = JsonConvert.DeserializeObject<List<TriviaQuestion>>(json) ?? new();
                foreach (var question in Questions)
                {
                    question.PrepareAnswers();
                }

            }
            catch (Exception ex)
            {
                Questions = new();
                Score = null;
                ModelState.AddModelError("", "Invalid quiz data. Please reload the page.");
                return;
            }

            Score = 0;
            for (int i = 0; i < Questions.Count; i++)
            {
                var correct = Questions[i].Correct_Answer?.Trim();
                var answer = Answers.ElementAtOrDefault(i)?.Trim();

                if (string.Equals(correct, answer, StringComparison.OrdinalIgnoreCase))
                {
                    Score++;
                }
            }
            if (!string.IsNullOrWhiteSpace(PlayerName) && Score != null)
                {
                    ScoreBoardStorage.AddScore(new ScoreEntry
                    {
                        PlayerName = PlayerName,
                        Score = Score.Value,
                        Total = Questions?.Count ?? 0,
                        Date = DateTime.Now
                    });
                }
                
        }
    }
}
