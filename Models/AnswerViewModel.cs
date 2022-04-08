using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace TuChanceTest_ASP.Net_Core_3_1.Models
{
    public partial class AnswerViewModel
    {
        [Display(Name = "Id Survey")]
        public int IdSurvey { get; set; }
        [Display(Name = "Survey Name")]
        public string Name { get; set; }
        [Display(Name = "Id Question")]
        public int IdQuestion { get; set; }
        [Display(Name = "Question")]
        public string Question { get; set; }
        [Display(Name = "Id Answer")]
        public int IdAnswer { get; set; }
        [Display(Name = "Answer")]
        public string Answer { get; set; }
    }
}
