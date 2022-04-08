using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TuChanceTest_ASP.Net_Core_3_1.Models;

namespace TuChanceTest_ASP.Net_Core_3_1.Controllers
{
    public class QuestionController : MyBaseController
    {
        private readonly TuChanceTestContext _context;

        public QuestionController(TuChanceTestContext context)
        {
            _context = context;
        }

        public ActionResult<IEnumerable<SurveyQuestion>> Detail()
        {
            var token = getToken();
            if (token != null && token != "")
            {
                return View(_context.SurveyQuestions.ToList());
            }
            else
            {
                return RedirectToAction("LogOut");
            }
        }


        public ActionResult Create()
        {
            var token = getToken();
            if (token != null && token != "")
            {
                ViewBag.Surveys = _context.Surveys.ToList();
                return View();
            }
            else
            {
                return RedirectToAction("LogOut");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SurveyQuestion surveyQuestion)
        {
            var token = getToken();
            if (token != null && token != "")
            {
                if (_context.SurveyQuestions.Where(x => x.IdSurvey == surveyQuestion.IdSurvey && x.Question == surveyQuestion.Question).Count() > 0) {
                    TempData["errorMessage"] = @"The question already exists in the system";
                } else { 
                    try
                    {
                        _context.SurveyQuestions.AddAsync(surveyQuestion);
                        _context.SaveChanges();
                        TempData["successMessage"] = @"Question created successfully";
                    }
                    catch
                    {
                        TempData["errorMessage"] = @"An unexpected error has occurred";
                    }
                }
                return RedirectToAction("Detail");
            }
            else
            {
                return RedirectToAction("LogOut");
            }
        }

        public ActionResult Edit(int id)
        {
            var token = getToken();
            if (token != null && token != "")
            {
                return View(_context.SurveyQuestions.Find(id));
            }
            else
            {
                return RedirectToAction("LogOut");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SurveyQuestion surveyQuestion, IFormCollection collection)
        {
            var token = getToken();
            if (token != null && token != "")
            {
                var data = _context.SurveyQuestions.Where(x => x.IdQuestion == surveyQuestion.IdQuestion).FirstOrDefault(); 
                if (data != null) { 
                    try
                    {
                        data.IdSurvey = surveyQuestion.IdSurvey;
                        data.Question = surveyQuestion.Question;
                        _context.SaveChanges();
                        TempData["successMessage"] = @"Question updated successfully";
                    }
                    catch
                    {
                        TempData["errorMessage"] = @"An unexpected error has occurred";
                    }
                }
                return RedirectToAction("Detail");
            }
            else
            {
                return RedirectToAction("LogOut");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var token = getToken();
            if (token != null && token != "")
            {
                try
                {
                    var data = _context.SurveyQuestions.Find(id);
                    _context.SurveyQuestions.Remove(data);
                    _context.SaveChanges();
                }
                catch
                {
                    TempData["errorMessage"] = @"An unexpected error has occurred";
                }
                return RedirectToAction("Detail");
            }
            else
            {
                return RedirectToAction("LogOut");
            }
        }
    }
}
