using Microsoft.AspNetCore.Mvc;
using OnlineJobPortal.DAL;
using OnlineJobPortal.Models;

namespace OnlineJobPortal.Controllers
{
    public class AdminController : Controller
    {


        private readonly User_DAL userdal;

        public AdminController(User_DAL dal)
        {
            userdal = dal;
        }
        public IActionResult Adminindex()
        {
            ViewBag.Module = "Admin";
            return View();
        }
        
        public IActionResult Viewjobapplication()
        {
            ViewBag.Module = "Admin";
            var jobapplication= userdal.GetJobApplications();
            if (jobapplication.Count == 0)
            {
                TempData["InfoMessage"] = "Currently, no students are available in the database.";
            }
            return View(jobapplication);
        }
        public IActionResult Viewuserdetail()
        {
            ViewBag.Module = "Admin";

            
            var jobList = userdal.GetAllUsers();

            var filteredJobList = jobList.Where(user =>
                !user.Firstname.Equals("admin", StringComparison.OrdinalIgnoreCase)).ToList();

            if (filteredJobList.Count == 0)
            {
                TempData["InfoMessage"] = "Currently, no users are available in the database.";
            }

            return View(filteredJobList);
        }


        public IActionResult Viewjob()
        {
            ViewBag.Module = "Admin";
            var jobList = userdal.GetAllJobs();
            if (jobList.Count == 0)
            {
                TempData["InfoMessage"] = "Currently, no students are available in the database.";
            }
            return View(jobList);
        
        }

        [HttpGet]
        public IActionResult Insertjob()
        {
            ViewBag.Module = "Admin";
            return View();  
        }

       
        
        [HttpPost]
        public async Task<IActionResult> Insertjob(Job job, IFormFile Image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Image != null && Image.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await Image.CopyToAsync(memoryStream);
                            byte[] photoBytes = memoryStream.ToArray();
                            job.Image = Convert.ToBase64String(photoBytes);
                        }
                    }

                     userdal.InsertJob(job);
                    
                        TempData["SuccessMessage"] = "Job details saved successfully!";
                        return RedirectToAction("AdminIndex");
                    
                }
                else
                {
                    TempData["ErrorMessage"] = "Model is not valid";
                    return View(job);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: " + ex.Message;
            }

            return View(job);
        }
        [HttpGet]
        public ActionResult Delete()
        {
            return View();
        }
        [HttpPost]
        public IActionResult DeleteJob(int id)
        {
            try
            {
                string result = userdal.DeleteJob(id); 
                if (result.Contains("successfully"))
                {
                    TempData["SuccessMessage"] = result;
                }
                else
                {
                    TempData["ErrorMessage"] = result;
                }
                return RedirectToAction("Adminindex"); 
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Adminindex");
            }
        }
        [HttpGet]
        public ActionResult Updatejob(int id)
        {
            ViewBag.Module = "Admin";
            var job = userdal.GetJobById(id);
            if (job == null)
            {
                TempData["InfoMessage"] = "Job not available with ID " + id.ToString();
                return RedirectToAction("Viewjob");
            }
            return View(job);
        }

        
        [HttpPost, ActionName("Updatejob")]

        public ActionResult Updatejob(Job job, IFormFile Image)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Image != null && Image.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            Image.OpenReadStream().CopyTo(memoryStream);
                            byte[] photoBytes = memoryStream.ToArray();
                            job.Image = Convert.ToBase64String(photoBytes); // Convert to Base64
                        }
                    }

                    


                    userdal.UpdateJob(job);
                    
                        TempData["SuccessMessage"] = "Job details Updated successfully!";
                        return RedirectToAction("Viewjob");
                    
                }
                else
                {
                    TempData["ErrorMessage"] = "Model is invalid";
                    return RedirectToAction("Updatejob");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error: " + ex.Message;
            }

            return View(job);
        }

        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                string result = userdal.Deleteuser(id);
                if (result.Contains("successfully"))
                {
                    TempData["SuccessMessage"] = result;
                }
                else
                {
                    TempData["ErrorMessage"] = result;
                }
                return RedirectToAction("Viewuserdetail");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Viewuserdetail");
            }
        }

        [HttpGet]
        public IActionResult AdminViewUserProfile(int userId, int jobApplicationId)
        {
            ViewBag.Module = "User";

            try
            {
                TempData["jobApplicationId"] = jobApplicationId;

                Profile profile = userdal.GetProfileById(userId);

                if (profile == null)
                {
                    TempData["errorMessage"] = $"User details not found for ID: {userId}";
                    return RedirectToAction("UserIndex");
                }

                List<JobApplication> jobApplications = userdal.GetJobApplicationsById(jobApplicationId);

                var jobApplication = jobApplications.FirstOrDefault();

                if (jobApplication != null && jobApplication.Action == "Applied")
                {
                    userdal.Changestatustoviewed(jobApplicationId);
                }

                return View(profile);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("UserIndex");
            }
        }



        public IActionResult ChangeStatusToShortlisted()
        {
            try
            {
                int jobapplicationid = Convert.ToInt32(TempData["jobApplicationId"]);
                userdal.Changestatustoshortlisted(jobapplicationid);
                return RedirectToAction("Viewjobapplication");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Viewjobapplication");
            }
        }
        public IActionResult DeleteJobApplication(int id)
        {
            try
            {
                userdal.DeleteJobApplication(id);
                return RedirectToAction("Viewjobapplication");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Viewjobapplication");
            }
        }
        public IActionResult Viewmessage()
        {
            ViewBag.Module = "Admin";
            var contactus = userdal.GetAllMessage();
            if (contactus.Count == 0)
            {
                TempData["InfoMessage"] = "Currently, no messages are available in the database.";
            }
            return View(contactus);

        }

        public IActionResult Deletemessage(int id)
        {
            try
            {
                userdal.DeleteMessage(id);
                return RedirectToAction("Viewmessage");
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Viewmessage");
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            TempData["successMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Homeindex", "Home");
        }
    }
}
