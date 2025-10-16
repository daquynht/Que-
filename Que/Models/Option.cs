using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Que.Models
{
    public class Option
    {
        [Key]
        public int OptionId { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; } // Hvilket alternativ er riktig?

        // Foreign Key til Question
        //public int QuestionId { get; set; } 
        // Navigasjonsegenskap for å peke til foreldre-spørsmålet
        //public virtual Question? Question { get; set; } 

        [ForeignKey(nameof(Question))]
            public int QuestionId { get; set; }
            public virtual Question? Question { get; set; }
        
    }
}