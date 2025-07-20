using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using QuizApp.Models;
using QuizApp.Models;
using QuizApp.Services;

namespace QuizApp.Pages
{
  
    public class ScoreBoard : PageModel
    {
        public List<ScoreEntry> Entries { get; private set; }

        public void OnGet()
        {
            Entries = ScoreBoardStorage.Entries
                .OrderByDescending(e => e.Score)
                .ThenBy(e => e.Date)
                .ToList();
        }
    }

}

