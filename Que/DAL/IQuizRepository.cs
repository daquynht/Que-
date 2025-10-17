using Que.Models;

namespace Que.DAL;

public interface IQuizRepository
{
	/* Task<IEnumerable<Quiz>> GetAll();
	Task Create(Quiz quiz);
    Task Update(Quiz quiz);
    Task<bool> Delete(int id);

    Task UpdateQuizAsync(Quiz quiz);

    Task<IEnumerable<Question>> GetQuestionsByQuizId(int quizId);
    Task AddQuestion(Question question);
    Task UpdateQuestionAsync(Question question);

    Task<IEnumerable<Option>> GetOptionsByQuizId(int quizId);
    Task AddOption(Option option);
    Task UpdateOptionAsync(Option option); */  
    // Quiz
    Task<Quiz?> GetQuizByIdAsync(int quizId);
    Task UpdateQuizAsync(Quiz quiz);

    // Questions
    Task<Question?> GetQuestionByIdAsync(int questionId);
    Task UpdateQuestionAsync(Question question);
    Task<IEnumerable<Question>> GetQuestionsByQuizIdAsync(int quizId);

    // Options
    Task<Option?> GetOptionByIdAsync(int optionId);
    Task UpdateOptionAsync(Option option);
    Task<IEnumerable<Option>> GetOptionsByQuizId(int quizId);

    // Andre metoder du allerede har
    Task<IEnumerable<Quiz>> GetAllQuizzesAsync();
    Task AddQuizAsync(Quiz quiz);
    Task DeleteQuizAsync(int quizId);
    Task<Quiz?> GetQuizById(int quizId);
    Task Create(Quiz quiz);
    Task Update(Quiz quiz);
    Task<bool> Delete(int id);
    Task<List<Question>> GetQuestionsByQuizId(int quizId);
    Task<List<Quiz>> GetAll();

    //Oppdatere quiz metoden etter å ha lagt inn spørsmål:
    // I IQuizRepository.cs
    Task<Quiz?> GetQuizWithDetailsAsync(int id); // Må hente Questions og Options
    Task<bool> UpdateQuizFullAsync(Quiz updatedQuiz);
}
