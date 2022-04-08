using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace TuChanceTest_ASP.Net_Core_3_1.Models
{
    public partial class Answers
    {
        public List<AnswerViewModel> AnswerList { get; set; }
    }
}
