using Microsoft.AspNetCore.Mvc;
using OnlineJobPortal.DAL;
using OnlineJobPortal.Models;
using static System.Net.Mime.MediaTypeNames;

namespace OnlineJobPortal.Controllers
{
    public class UserController : Controller
    {

        private readonly User_DAL userdal;

        public UserController(User_DAL dal)
        {
            userdal = dal;
        }

        public IActionResult Userindex()
        {
            ViewBag.Module = "User";
            return View();
        }

        [HttpGet]
        public IActionResult Applyjob()
        { 
            ViewBag.Module = "User";
            var jobList = userdal.GetAllJobs();
            if (jobList.Count == 0)
            {
                TempData["InfoMessage"] = "Currently, no students are available in the database.";
            }
            return View(jobList);
            
        }
        [HttpPost, ActionName("Applyjob")]
        public IActionResult Applyjob(int id)
        {
            ViewBag.Module = "User";
            try
            {

                int? userId = HttpContext.Session.GetInt32("UserId");
                if (userId == null)
                {
                    TempData["errorMessage"] = "You must be logged in to apply for a job.";
                    return RedirectToAction("Applyjob");
                }
                var job = userdal.GetJobById(id);
                if (job == null)
                {
                    TempData["errorMessage"] = "Job not found.";
                    return RedirectToAction("Applyjob");
                }

                var profile = userdal.GetProfileById(userId.Value);
                if (profile == null)
                {
                    TempData["errorMessage"] = "Profile information is missing. Please complete your profile to apply for the job.";
                    return RedirectToAction("Profile", "Account");
                }
                userdal.Applyjob(id, userId.Value, job, profile);
                TempData["successMessage"] = "You have successfully applied for the job.";

                return RedirectToAction("Applyjob");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult Addeducation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Addeducation(Education education, IFormFile Photo, IFormFile Resume)
        {
            ViewBag.Module = "User";

            try
            {
                int? userId = HttpContext.Session.GetInt32("UserId");
                 
                    if (Photo != null && Photo.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await Photo.CopyToAsync(memoryStream);
                            education.Photo = Convert.ToBase64String(memoryStream.ToArray());
                        }
                    }

                    
                    if (Resume != null && Resume.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await Resume.CopyToAsync(memoryStream);
                            education.Resume = Convert.ToBase64String(memoryStream.ToArray());
                        }
                    }
                    education.userid = userId.Value;

                    userdal.Addeducation(education);

                    TempData["successMessage"] = "Education details added successfully!";
                    return RedirectToAction("Profile");
              
                 
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "An error occurred: " + ex.Message;
            }

            return View(education);
        }


        public IActionResult DownloadResume(int id)
        {
            var profile = userdal.GetProfileById(id);

            if (profile == null || string.IsNullOrEmpty(profile.Resume))
            {
                return NotFound("The requested resume does not exist.");
            }

            byte[] fileBytes = Convert.FromBase64String(profile.Resume);

            return File(fileBytes, "application/pdf", "Resume.pdf");
        }


        [HttpGet]
        public IActionResult Updateeducation(int id)
        {
            ViewBag.Module = "User";
            var education = userdal.GetEducationById(id);
            if (education == null)
            {
                TempData["InfoMessage"] = "Education details not available with ID " + id.ToString();
                return RedirectToAction("Adminindex");
            }
            return View(education);

        }
        [HttpPost,ActionName("Updateeducation")]
        public async Task<IActionResult> Updateeducation(Education education, IFormFile Photo, IFormFile Resume)
        {
            ViewBag.Module = "User";

            try
            {

                if (Photo != null && Photo.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await Photo.CopyToAsync(memoryStream);
                        education.Photo = Convert.ToBase64String(memoryStream.ToArray());
                    }
                }


                if (Resume != null && Resume.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await Resume.CopyToAsync(memoryStream);
                        education.Resume = Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
                

                userdal.Updateeducation(education);

                TempData["successMessage"] = "Education details updated successfully!";
                return RedirectToAction("Profile");


            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "An error occurred: " + ex.Message;
            }

            return View(education);
        }

        public IActionResult Status()
        {
            ViewBag.Module = "User";
            try
            {

                int? userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                {
                    TempData["errorMessage"] = "User session has expired. Please log in again.";
                    return RedirectToAction("Signin", "Home");
                }


                IEnumerable<JobApplication> jobapplications = userdal.GetJobApplicationsByUserId(userId.Value);
                if (jobapplications == null)
                {
                    TempData["errorMessage"] = $"User details not found for Id: {userId}";
                    return RedirectToAction("Userindex");
                }

                return View(jobapplications);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Userindex");
            }
            
        }

        public IActionResult Profile()
        {
            ViewBag.Module = "User";
            try
            {
                
                int? userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                {
                    TempData["errorMessage"] = "User session has expired. Please log in again.";
                    return RedirectToAction("Signin", "Home");
                }


                Profile profile = userdal.GetProfileById(userId.Value);

                if (profile == null)
                {
                    TempData["errorMessage"] = $"User details not found for Id: {userId}";
                    return RedirectToAction("Userindex");
                }

                return View(profile);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Userindex");
            }
        }
        [HttpGet]
        public IActionResult UpdateProfile()
        {
            ViewBag.Module = "User";
            try
            {

                int? userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                {
                    TempData["errorMessage"] = "User session has expired. Please log in again.";
                    return RedirectToAction("Signin", "Home");
                }


                ProfileUpdate user = userdal.GetUsersById(userId.Value);

                if (user == null)
                {
                    TempData["errorMessage"] = $"User details not found for Id: {userId}";
                    return RedirectToAction("Userindex");
                }

                return View(user);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Userindex");
            }
        }

        
        [HttpPost]
        public IActionResult UpdateProfile(ProfileUpdate profileupdate)
        {
            try
            {
                int? userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null)
                {
                    TempData["errorMessage"] = "User session has expired. Please log in again.";
                    return RedirectToAction("Signin", "Home");
                }

                profileupdate.Id = userId.Value;

                if (!ModelState.IsValid)
                {
                    TempData["errorMessage"] = "Model data is invalid";
                    return View(profileupdate);
                }

                userdal.UpdateUser(profileupdate);

                    TempData["successMessage"] = "Profile updated successfully.";
                    return RedirectToAction("Profile");
                               
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(profileupdate);
            }
        }

        public IActionResult Viewjobfromstatus(int id)
        {
            var job = userdal.GetJobById(id);
            if (job == null)
            {
                TempData["InfoMessage"] = "Job not available with ID " + id.ToString();
                return RedirectToAction("Viewjob");
            }
            return View(job);
        }

        public IActionResult DeleteJobApplication(int id)
        {
            try
            {
                userdal.DeleteJobApplication(id);
                return RedirectToAction("Status");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Userindex");
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            TempData["successMessage"] = "You have been logged out successfully.";
            return RedirectToAction("HomeIndex", "Home");
        }
    }
}
