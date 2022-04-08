using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TuChanceTest_ASP.Net_Core_3_1.Models;

namespace TuChanceTest_ASP.Net_Core_3_1.Controllers
{
    public class SurveyController : MyBaseController
    {
        private readonly TuChanceTestContext _context;

        public SurveyController(TuChanceTestContext context)
        {
            _context = context;
        }

        public ActionResult<IEnumerable<Survey>> Detail()
        {
            var token = getToken();
            if (token != null && token != "")
            {
                return View(_context.Surveys.ToList());
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
                return View();
            }
            else
            {
                return RedirectToAction("LogOut");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Survey survey)
        {
            var token = getToken();
            if (token != null && token != "")
            {
                if (_context.Surveys.Where(x =>x.Name == survey.Name).Count() > 0) {
                TempData["errorMessage"] = @"The survey already exists in the system";
                } else { 
                    try
                    {
                        _context.Surveys.AddAsync(survey);
                        _context.SaveChanges();
                        TempData["successMessage"] = @"Survey created successfully";
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
                return View(_context.Surveys.Find(id));
            }
            else
            {
                return RedirectToAction("LogOut");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Survey survey, IFormCollection collection)
        {
            var token = getToken();
            if (token != null && token != "")
            {
                var data = _context.Surveys.Find(survey.IdSurvey);
                try{ 
                    data.Name = survey.Name;
                    _context.SaveChanges();
                    TempData["successMessage"] = @"Survey updated successfully";
                }catch{
                    TempData["errorMessage"] = @"An unexpected error has occurred";
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
        public ActionResult Delete(int id)
        {
            var token = getToken();
            if (token != null && token != "")
            {
                try
                {
                    var data = _context.Surveys.Find(id);
                    _context.Surveys.Remove(data);
                    _context.SaveChanges();
                }
                catch
                {
                    ViewBag.errorMessage = @"An unexpected error has occurred";
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
