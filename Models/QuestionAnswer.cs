using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace TuChanceTest_ASP.Net_Core_3_1.Models
{
    public partial class QuestionAnswer
    {
        [Display(Name = "Id Answer")]
        public int IdAnswer { get; set; }
        [Display(Name = "Id Question")]
        public int IdQuestion { get; set; }
        [Display(Name = "Answer")]
        public string Answer { get; set; }

        public virtual SurveyQuestion IdQuestionNavigation { get; set; }
    }
}
