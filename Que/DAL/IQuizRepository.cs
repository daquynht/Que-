using Que.Models;

namespace Que.DAL;

public interface IItemRepository
{
	Task<IEnumerable<Quiz>> GetAll();
    Task<Quiz?> GetQuizById(int id);
	Task Create(Quiz quiz);
    Task Update(Quiz quiz);
    Task<bool> Delete(int id);
}
