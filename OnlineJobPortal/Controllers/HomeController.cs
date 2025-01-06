using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OnlineJobPortal.DAL;
using OnlineJobPortal.Models;

namespace OnlineJobPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly User_DAL userdal;

        public HomeController(User_DAL dal)
        {
            userdal = dal;
        }

       
        public IActionResult Homeindex()
        {
            ViewBag.Module = "Home";
            return View();
        }

        
        public IActionResult Aboutus()
        {
            ViewBag.Module = "Home";
            return View();
        }

        
        [HttpGet]
        public IActionResult Signin()
        {
            ViewBag.Module = "Home";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Signin(Login login)
        {
            ViewBag.Module = "Home";

            try
            {
                if (ModelState.IsValid)
                {
                    var user = userdal.GetLoginDetails(login.Username, login.Password);

                    if (user != null)
                    {
                        HttpContext.Session.SetInt32("UserId", user.Id);
                        HttpContext.Session.SetString("Username", user.Username);

                        if (user.Username.ToLower() == "admin" && login.Password == "admin")
                        {
                            TempData["successMessage"] = $"Welcome Admin";
                            return RedirectToAction("Adminindex", "Admin");
                        }
                        else
                        {
                            TempData["successMessage"] = $"Welcome, {user.Username}";
                            return RedirectToAction("Userindex", "User");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid username or password.");
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Please ensure all fields are filled correctly.";
                    return View(login);
                }
            }
            catch (Exception ex)
            {
          
                TempData["errorMessage"] = ex.Message;
            }
            return View(login);
        }

        
        [HttpGet]
        public IActionResult Signup()
        {
            ViewBag.Module = "Home";
            return View();
        }

        [HttpPost]
        public IActionResult Signup(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    userdal.Insert(user);
                    
                        TempData["successMessage"] = "User details saved successfully!";
                        return RedirectToAction("Homeindex");
                    
                }
                else
                {
                    TempData["errorMessage"] = "Please ensure all fields are filled correctly.";
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return View(user);
        }
        [HttpGet]
        public IActionResult Contactus()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contactus(Contactus contactus)
        {
            ViewBag.Module = "Home";

            
                userdal.Contactus(contactus);
                TempData["SuccessMessage"] = "Your message has been sent successfully.";
            

            return View(contactus);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
