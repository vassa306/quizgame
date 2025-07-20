using QuizApp.Models;

namespace QuizApp.Services
{
        public static class ScoreBoardStorage
        {
            private static List<ScoreEntry> _entries = new List<ScoreEntry>();

            public static IReadOnlyList<ScoreEntry> Entries => _entries.AsReadOnly();

            public static void AddScore(ScoreEntry entry)
            {
                _entries.Add(entry);
            }
        }
    }


