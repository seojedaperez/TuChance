using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace TuChanceTest_ASP.Net_Core_3_1.Models
{
    public partial class Survey
    {
        public Survey()
        {
            SurveyQuestions = new HashSet<SurveyQuestion>();
        }

        [Display(Name = "Id Survey")]
        public int IdSurvey { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }

        public virtual ICollection<SurveyQuestion> SurveyQuestions { get; set; }
    }
}
