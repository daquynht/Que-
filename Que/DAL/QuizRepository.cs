using Microsoft.EntityFrameworkCore;
using Que.Models;

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

    public async Task<Quiz?> GetItemById(int id)
    {
        return await _db.Quizes.FindAsync(id);
    }

    public async Task Create(Quiz item)
    {
        _db.Quizes.Add(item);
        await _db.SaveChangesAsync();
    }

    public async Task Update(Quiz item)
    {
        _db.Quizes.Update(item);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> Delete(int id)
    {
        var item = await _db.Quizes.FindAsync(id);
        if (item == null)
        {
            return false;
        }

        _db.Quizes.Remove(item);
        await _db.SaveChangesAsync();
        return true;
    }
}
