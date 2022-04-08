using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Firebase.Auth;
using Microsoft.AspNetCore.Http;

namespace TuChanceTest_ASP.Net_Core_3_1.Controllers
{
    public class MyBaseController : Controller
    {
        public static FirebaseAuthProvider auth;

        private static string ApiKey = "AIzaSyAJsXW3D4dzBi40qgpi2c-m1DPb9B-In1w";

        public MyBaseController()
        {
            auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
        }

        public string getToken()
        {
            Dispose();
            try
            {
                var token = HttpContext.Session.GetString("_UserToken");
                Task<Firebase.Auth.User> user = auth.GetUserAsync(token);
                user.Wait();
                return token;
            }
            catch
            {

            }
            return "";
        }

        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("_UserToken");
            return RedirectToAction("Login");
        }

    }
}
