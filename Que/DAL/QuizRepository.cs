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
            .Where(o => o.Question.QuizId == quizId)
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
}