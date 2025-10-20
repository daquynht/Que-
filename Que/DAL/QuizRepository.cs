using Microsoft.EntityFrameworkCore;
using Que.Models;
using Que.DAL;

namespace Que.DAL;

public class QuizRepository : IQuizRepository
{
    private readonly QuizDbContext _db;

    public QuizRepository(QuizDbContext context)
    {
        _db = context;
    }

    //QUIZ
    public async Task<Quiz?> GetQuizByIdAsync(int quizId)
    {
        return await _db.Quizes.FindAsync(quizId);
    }

    public async Task UpdateQuizAsync(Quiz quiz)
    {
        _db.Quizes.Update(quiz);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<Quiz>> GetAllQuizzesAsync()
    {
        return await _db.Quizes.ToListAsync();
    }

    public async Task AddQuizAsync(Quiz quiz)
    {
        await _db.Quizes.AddAsync(quiz);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteQuizAsync(int quizId)
    {
        var quiz = await _db.Quizes.FindAsync(quizId);
        if (quiz != null)
        {
            _db.Quizes.Remove(quiz);
            await _db.SaveChangesAsync();
        }
    }

    // QUESTIONS 
    public async Task<Question?> GetQuestionByIdAsync(int questionId)
    {
        return await _db.Questions.FindAsync(questionId);
    }

    public async Task UpdateQuestionAsync(Question question)
    {
        _db.Questions.Update(question);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<Question>> GetQuestionsByQuizIdAsync(int quizId)
    {
        return await _db.Questions
            .Where(q => q.QuizId == quizId)
            .ToListAsync();
    }

    // OPTIONS
    public async Task<Option?> GetOptionByIdAsync(int optionId)
    {
        return await _db.Options.FindAsync(optionId);
    }

    public async Task UpdateOptionAsync(Option option)
    {
        _db.Options.Update(option);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<Option>> GetOptionsByQuizId(int quizId)
    {
        return await _db.Options
            // Rettingspunkt #1: Korrekt null-sjekk og riktig bruk av parameteren 'quizId'
            .Where(o => o.Question != null && o.Question.QuizId == quizId) 
            .ToListAsync();
    }

    public async Task<Quiz?> GetQuizById(int quizId)
    {
        return await _db.Quizes
            .Include(q => q.Questions)
            .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(q => q.QuizId == quizId);
    }

    public async Task Create(Quiz quiz)
    {
        _db.Quizes.Add(quiz);
        await _db.SaveChangesAsync();
    }

    public async Task Update(Quiz quiz)
    {
        _db.Quizes.Update(quiz);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> Delete(int quizId)
    {
        var quiz = await _db.Quizes.FindAsync(quizId);
        if (quiz == null) return false;
        _db.Quizes.Remove(quiz);
        await _db.SaveChangesAsync();
        return true;
    }
    public async Task<List<Question>> GetQuestionsByQuizId(int quizId)
    {
        var quiz = await _db.Quizes
            .Include(q => q.Questions)
                .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(q => q.QuizId == quizId);

        return quiz?.Questions ?? new List<Question>();
    }
    public async Task<List<Quiz>> GetAll()
    {
        return await _db.Quizes
            .Include(q => q.Questions)
            .ThenInclude(q => q.Options)
            .ToListAsync();
    }

    public async Task<Quiz?> GetQuizWithDetailsAsync(int id)
    {
        // Bruk Eager Loading for å få med alt
        return await _db.Quizes
            .Include(q => q.Questions)
                .ThenInclude(q => q.Options)
            .FirstOrDefaultAsync(q => q.QuizId == id);
    }

    public async Task<bool> UpdateQuizFullAsync(Quiz updatedQuiz)
    {
        // Hent eksisterende quiz med spørsmål og alternativer
        var existingQuiz = await _db.Quizes
            .Include(q => q.Questions)
                .ThenInclude(qs => qs.Options)
            .FirstOrDefaultAsync(q => q.QuizId == updatedQuiz.QuizId);

        if (existingQuiz == null) return false;

        // --- Oppdater quiz-felt ---
        existingQuiz.Name = updatedQuiz.Name;
        existingQuiz.Description = updatedQuiz.Description;
        existingQuiz.Category = updatedQuiz.Category;
        existingQuiz.Difficulty = updatedQuiz.Difficulty;
        existingQuiz.TimeLimit = updatedQuiz.TimeLimit;

        // --- Håndter spørsmål ---
        var updatedQuestions = updatedQuiz.Questions ?? new List<Question>();
        var existingQuestions = existingQuiz.Questions.ToList();

        // Slett spørsmål som ikke lenger finnes
        foreach (var existingQ in existingQuestions)
        {
            if (!updatedQuestions.Any(uq => uq.QuestionId == existingQ.QuestionId && existingQ.QuestionId != 0))
            {
                _db.Questions.Remove(existingQ); // Cascade vil fjerne alternativer
            }
        }

        // Oppdater eller legg til spørsmål
        foreach (var updatedQ in updatedQuestions)
        {
            var existingQ = existingQuestions.FirstOrDefault(q => q.QuestionId == updatedQ.QuestionId && updatedQ.QuestionId != 0);

            if (existingQ != null)
            {
                // Oppdater spørsmålstekst og AllowMultipleAnswers
                existingQ.Text = updatedQ.Text;
                existingQ.AllowMultipleAnswers = updatedQ.AllowMultipleAnswers;

                // --- Håndter alternativer ---
                var updatedOptions = updatedQ.Options ?? new List<Option>();
                var existingOptions = existingQ.Options.ToList();

                // Slett alternativer som ikke finnes
                foreach (var existingO in existingOptions)
                {
                    if (!updatedOptions.Any(uo => uo.OptionId == existingO.OptionId && existingO.OptionId != 0))
                    {
                        _db.Options.Remove(existingO);
                    }
                }

                // Oppdater eller legg til alternativer
                foreach (var updatedO in updatedOptions)
                {
                    var existingO = existingOptions.FirstOrDefault(o => o.OptionId == updatedO.OptionId && updatedO.OptionId != 0);

                    if (existingO != null)
                    {
                        existingO.Text = updatedO.Text;
                        existingO.IsCorrect = updatedO.IsCorrect;
                    }
                    else
                    {
                        // Nytt alternativ
                        if (existingQ.Options == null) existingQ.Options = new List<Option>();
                        existingQ.Options.Add(new Option
                        {
                            Text = updatedO.Text,
                            IsCorrect = updatedO.IsCorrect
                        });
                    }
                }
            }
            else
            {
                // Nytt spørsmål
                if (existingQuiz.Questions == null) existingQuiz.Questions = new List<Question>();
                var newQuestion = new Question
                {
                    Text = updatedQ.Text,
                    AllowMultipleAnswers = updatedQ.AllowMultipleAnswers,
                    Options = updatedQ.Options?.Select(o => new Option
                    {
                        Text = o.Text,
                        IsCorrect = o.IsCorrect
                    }).ToList() ?? new List<Option>()
                };
                existingQuiz.Questions.Add(newQuestion);
            }
        }

        // Lagre endringer
        return await _db.SaveChangesAsync() > 0;
    }
}