using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TuChanceTest_ASP.Net_Core_3_1.Models;
using Firebase.Auth;
using Microsoft.AspNetCore.Http;

namespace TuChanceTest_ASP.Net_Core_3_1.Controllers
{
    public class HomeController : MyBaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TuChanceTestContext _context;

        public HomeController(ILogger<HomeController> logger, TuChanceTestContext context)
        {
            _context = context;
            _logger = logger;
        }

        [Route("TakeSurvey/{IdSurvey}")]
        public IActionResult TakeSurvey(int IdSurvey)
        {
            Answers answersList = new Answers();
            answersList.AnswerList = (from s in _context.Surveys
                                   join q in _context.SurveyQuestions
                                   on s.IdSurvey equals q.IdSurvey
                                   select new AnswerViewModel()
                                   {
                                       IdSurvey = s.IdSurvey,
                                       Name = s.Name,
                                       IdQuestion = q.IdQuestion,
                                       Question = q.Question

                                   }).ToList();
            return View(answersList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Answers answersList)
        {
            var token = getToken();
            if (token != null && token != "") { 
                foreach (AnswerViewModel item in answersList.AnswerList) {
                    if (item.Answer != null && item.Answer != "") {
                        QuestionAnswer a = new QuestionAnswer() { IdQuestion = item.IdQuestion, Answer = item.Answer };
                        _context.QuestionAnswers.AddAsync(a);
                    }
                }
                _context.SaveChanges();
                TempData["successMessage"] = @"Answers upload successfully";
                return RedirectToAction("TakeSurvey", new {IdSurvey= answersList.AnswerList[0].IdSurvey });
            }else
            {
                return RedirectToAction("LogOut");
            }
        }

        public IActionResult Index()
        {
            var token = getToken();
            if (token != null && token != "")
            {
                ViewBag.SurveysAvailables = (from s in _context.Surveys
                                             join q in _context.SurveyQuestions
                                             on s.IdSurvey equals q.IdSurvey
                                             select new Survey()
                                             {
                                                 IdSurvey = s.IdSurvey,
                                                 Name = s.Name
                                             }).Distinct().ToList();

                Answers answersList = new Answers();
                answersList.AnswerList = (from s in _context.Surveys
                                          join q in _context.SurveyQuestions
                                          on s.IdSurvey equals q.IdSurvey
                                          join a in _context.QuestionAnswers
                                          on q.IdQuestion equals a.IdQuestion
                                          select new AnswerViewModel()
                                          {
                                              IdSurvey = s.IdSurvey,
                                              Name = s.Name,
                                              IdQuestion = q.IdQuestion,
                                              Question = q.Question,
                                              IdAnswer=a.IdAnswer,
                                              Answer=a.Answer
                                          }).ToList();
                return View(answersList);
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
                    var data = _context.QuestionAnswers.Find(id);
                    _context.QuestionAnswers.Remove(data);
                    _context.SaveChanges();
                }
                catch
                {
                    TempData["errorMessage"] = @"An unexpected error has occurred";
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("LogOut");
            }
        }

        public async Task<IActionResult> Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserModel userModel)
        {
            try
            {
                var fbAuthLink = await auth
                           .SignInWithEmailAndPasswordAsync(userModel.email, userModel.password);
                string token = fbAuthLink.FirebaseToken;

                if (token != null && token != "")
                {
                    HttpContext.Session.SetString("_UserToken", token);

                    Task<Firebase.Auth.User> user = auth.GetUserAsync(token);
                    user.Wait();

                    if (user.Result.IsEmailVerified == true)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.errorMessage = "Remember that you need to verify your email before accessing the system.";
                    }
                }
                else
                {
                    return View();
                }
            }
            catch (FirebaseAuthException error)
            {
                switch (error.Reason.ToString())
                {
                    case "InvalidEmailAddress":
                        ViewBag.errorMessage = "The email you entered is incorrect.";
                        break;
                    case "WrongPassword":
                        ViewBag.errorMessage = "Incorrect email and password combination.";
                        //ViewBag.errorMessage = "The Password entered is incorrect.";
                        break;
                    case "WeakPassword":
                        ViewBag.errorMessage = "Password must contain a minimum of 6 characters.";
                        break;
                    case "UserDisabled":
                        ViewBag.errorMessage = "The user has been disabled.";
                        break;
                    case "EmailExists":
                        ViewBag.errorMessage = "The email already exists in the system.";
                        break;
                    case "MissingEmail":
                        ViewBag.errorMessage = "Missing email address.";
                        break;
                    case "UnknownEmailAddress":
                        ViewBag.errorMessage = "Incorrect email and password combination.";//ViewBag.errorMessage = "Unknown email address.";
                        break;
                    case "MissingPassword":
                        ViewBag.errorMessage = "Missing Password.";
                        break;
                    case "SystemError":
                        ViewBag.errorMessage = "System error.";
                        break;
                    case "InvalidIDToken":
                        ViewBag.errorMessage = "Wrong Token ID.";
                        break;
                    case "Undefined":
                        ViewBag.errorMessage = "An unknown error has occurred.";
                        break;
                    case "InvalidProviderID":
                        ViewBag.errorMessage = "The provider ID is not valid.";
                        break;
                    case "LoginCredentialsTooOld":
                        ViewBag.errorMessage = "Login credentials are too old.";
                        break;
                    case "InvalidIdentifier":
                        ViewBag.errorMessage = "The handle is not valid.";
                        break;
                    case "MissingIdentifier":
                        ViewBag.errorMessage = "The handle is unknown.";
                        break;
                    case "InvalidAccessToken":
                        ViewBag.errorMessage = "The access token is not valid.";
                        break;
                    case "MissingRequestURI":
                        ViewBag.errorMessage = "Third Party Authentication Providers: Request does not contain a value for parameter: requestUri.";
                        break;
                    case "MissingRequestType":
                        ViewBag.errorMessage = "The Request does not contain a value for the parameter.";
                        break;
                    default:
                        ViewBag.errorMessage = "An unknown error has occurred - Error code: " + error.Reason.ToString();
                        return View();
                }

            }
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserModel userModel) 
        {
            try
            {
                var fbAuthLink = await auth
                           .CreateUserWithEmailAndPasswordAsync(userModel.email, userModel.password, "", true);
                ViewBag.errorMessage = "Se ha enviado un correo para verificar su cuenta.";
                return View();
            }
            catch (FirebaseAuthException error)
            {
                switch (error.Reason.ToString())
                {
                    case "InvalidEmailAddress":
                        ViewBag.errorMessage = "The email you entered is incorrect.";
                        break;
                    case "WrongPassword":
                        ViewBag.errorMessage = "Incorrect email and password combination.";
                        //ViewBag.errorMessage = "The Password entered is incorrect.";
                        break;
                    case "WeakPassword":
                        ViewBag.errorMessage = "Password must contain a minimum of 6 characters.";
                        break;
                    case "UserDisabled":
                        ViewBag.errorMessage = "The user has been disabled.";
                        break;
                    case "EmailExists":
                        ViewBag.errorMessage = "The email already exists in the system.";
                        break;
                    case "MissingEmail":
                        ViewBag.errorMessage = "Missing email address.";
                        break;
                    case "UnknownEmailAddress":
                        ViewBag.errorMessage = "Incorrect email and password combination.";//ViewBag.errorMessage = "Unknown email address.";
                        break;
                    case "MissingPassword":
                        ViewBag.errorMessage = "Missing Password.";
                        break;
                    case "SystemError":
                        ViewBag.errorMessage = "System error.";
                        break;
                    case "InvalidIDToken":
                        ViewBag.errorMessage = "Wrong Token ID.";
                        break;
                    case "Undefined":
                        ViewBag.errorMessage = "An unknown error has occurred.";
                        break;
                    case "InvalidProviderID":
                        ViewBag.errorMessage = "The provider ID is not valid.";
                        break;
                    case "LoginCredentialsTooOld":
                        ViewBag.errorMessage = "Login credentials are too old.";
                        break;
                    case "InvalidIdentifier":
                        ViewBag.errorMessage = "The handle is not valid.";
                        break;
                    case "MissingIdentifier":
                        ViewBag.errorMessage = "The handle is unknown.";
                        break;
                    case "InvalidAccessToken":
                        ViewBag.errorMessage = "The access token is not valid.";
                        break;
                    case "MissingRequestURI":
                        ViewBag.errorMessage = "Third Party Authentication Providers: Request does not contain a value for parameter: requestUri.";
                        break;
                    case "MissingRequestType":
                        ViewBag.errorMessage = "The Request does not contain a value for the parameter.";
                        break;
                    default:
                        ViewBag.errorMessage = "An unknown error has occurred - Error code: " + error.Reason.ToString();
                        return View();
                }
            }
            return View();
        }

    }
}
