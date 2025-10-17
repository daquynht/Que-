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
    // I QuizRepository.cs
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
    // 1. Hent den eksisterende quizen med alle relasjoner (Tracked)
    var existingQuiz = await GetQuizWithDetailsAsync(updatedQuiz.QuizId);

    if (existingQuiz == null) return false;

    // 2. Oppdater Quiz Metadata
    _db.Entry(existingQuiz).CurrentValues.SetValues(updatedQuiz);

    // 3. Synkroniser Questions (EF Core Power Feature)
    var existingQuestions = existingQuiz.Questions.ToList() ?? new List<Question>();
    
    // Fjern spørsmål som ikke lenger er i 'updatedQuiz'
    foreach (var existingQ in existingQuestions)
    {
        if (!(updatedQuiz.Questions ?? new List<Question>()).Any(q => q.QuestionId == existingQ.QuestionId && existingQ.QuestionId != 0))
        {
            _db.Questions.Remove(existingQ);
        }
    }
    
    // Oppdater og legg til nye spørsmål
    foreach (var updatedQ in updatedQuiz.Questions ?? new List<Question>())
    {
        var existingQ = existingQuestions.FirstOrDefault(q => q.QuestionId == updatedQ.QuestionId && updatedQ.QuestionId != 0);

        if (existingQ != null)
        {
            // Oppdatering av eksisterende spørsmål
            _db.Entry(existingQ).CurrentValues.SetValues(updatedQ);
            
            // Synkroniser Options for dette spørsmålet
            var existingOptions = existingQ.Options.ToList() ?? new List<Option>();

            // Fjern alternativer som er slettet
            foreach (var existingO in existingOptions)
            {
                 if (!updatedQ.Options.Any(o => o.OptionId == existingO.OptionId && existingO.OptionId != 0))
                 {
                     _db.Options.Remove(existingO);
                 }
            }

            // Oppdater og legg til nye alternativer
            foreach (var updatedO in updatedQ.Options ?? new List<Option>())
            {
                var existingO = existingOptions.FirstOrDefault(o => o.OptionId == updatedO.OptionId && updatedO.OptionId != 0);

                if (existingO != null)
                {
                    // Oppdatering av eksisterende alternativ
                    _db.Entry(existingO).CurrentValues.SetValues(updatedO);
                }
                else
                {
                    // Legg til nytt 
                    if (existingQ.Options == null) existingQ.Options = new List<Option>();
                    existingQ.Options.Add(updatedO);
                }
            }
        }
        else
        {
            // Legg til nytt spørsmål (QuestionId = 0)
            if (existingQuiz.Questions == null) existingQuiz.Questions = new List<Question>();
            existingQuiz.Questions.Add(updatedQ);
        }
    }

    // 4. Lagre alt i én transaksjon
    return await _db.SaveChangesAsync() > 0;
}
}