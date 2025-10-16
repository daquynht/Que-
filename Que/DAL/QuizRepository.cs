using Microsoft.EntityFrameworkCore;
using Que.Models;
using Que.DAL;

namespace Que.DAL;

public class QuizRepository : IQuizRepository
{
    private readonly QuizDbContext _db;

    public QuizRepository(QuizDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Quiz>> GetAll()
    {
        return await _db.Quizes.ToListAsync();
    }

    public async Task<Quiz?> GetQuizById(int id)
    {
        return await _db.Quizes.FindAsync(id);
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

    public async Task<bool> Delete(int id)
    {
        var quiz = await _db.Quizes.FindAsync(id);
        if (quiz == null)
        {
            return false;
        }

        _db.Quizes.Remove(quiz);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Question>> GetQuestionsByQuizId(int quizId)
    {
        // Henter alle spørsmål hvor QuizId matcher den inngående parameteren
        return await _db.Questions
            .Where(q => q.QuizId == quizId)
            .ToListAsync();
    }

    public async Task AddQuestion(Question question)
    {
        _db.Questions.Add(question);
        await _db.SaveChangesAsync();

        // Alternativer legges til via Question.Options fordi de er navigasjonsegenskaper
        foreach (var option in question.Options)
        {
            option.QuestionId = question.QuestionId;
            _db.Options.Add(option);
        }
        await _db.SaveChangesAsync();
    }
}
