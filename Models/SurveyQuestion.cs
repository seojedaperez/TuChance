using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace TuChanceTest_ASP.Net_Core_3_1.Models
{
    public partial class SurveyQuestion
    {
        public SurveyQuestion()
        {
            QuestionAnswers = new HashSet<QuestionAnswer>();
        }

        [Display(Name = "Id Question")]
        public int IdQuestion { get; set; }
        [Display(Name = "Id Survey")]
        public int IdSurvey { get; set; }
        [Display(Name = "Question")]
        public string Question { get; set; }

        public virtual Survey IdSurveyNavigation { get; set; }
        public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; }

    }
}
