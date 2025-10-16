using Que.Models;

namespace Que.DAL;

public interface IQuizRepository
{
	Task<IEnumerable<Quiz>> GetAll();
    Task<Quiz?> GetQuizById(int id);
	Task Create(Quiz quiz);
    Task Update(Quiz quiz);
    Task<bool> Delete(int id);
    Task<IEnumerable<Question>> GetQuestionsByQuizId(int quizId);
    Task AddQuestion(Question question);
}
