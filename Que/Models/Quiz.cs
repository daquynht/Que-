using System;
namespace Que.Models
{
	public class Quiz
	{
		public int QuizId { get; set; }
		public string Name { get; set; } = string.Empty;
		public string? Description { get; set; }
	}
}