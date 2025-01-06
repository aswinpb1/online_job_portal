using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OnlineJobPortal.Models;
using System.Data;

namespace OnlineJobPortal.DAL
{
    public class User_DAL
    {
        SqlConnection sqlconnection = null;
        SqlCommand sqlcommand = null;
        public static IConfiguration Configuration { get; set; }

        private string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            return Configuration.GetConnectionString("DefaultConnection");
        }
        
        // Home
        public void Insert(User user)
        {
            using (sqlconnection = new SqlConnection(GetConnectionString()))
            {
                sqlcommand = sqlconnection.CreateCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "InsertUsers";
                sqlcommand.Parameters.AddWithValue("@Firstname", user.Firstname);
                sqlcommand.Parameters.AddWithValue("@Lastname", user.Lastname);
                sqlcommand.Parameters.AddWithValue("@Dateofbirth", user.Dateofbirth);
                sqlcommand.Parameters.AddWithValue("@Gender", user.Gender);
                sqlcommand.Parameters.AddWithValue("@Phone", user.Phone);
                sqlcommand.Parameters.AddWithValue("@Email", user.Email);
                sqlcommand.Parameters.AddWithValue("@Address", user.Address);
                sqlcommand.Parameters.AddWithValue("@Username", user.Username);
                sqlcommand.Parameters.AddWithValue("@Password", user.Password);
                sqlconnection.Open();
                sqlcommand.ExecuteNonQuery();
                sqlconnection.Close();
            }    
        }

        
        public Login GetLoginDetails(string username, string password)
        {
            using (var sqlconnection = new SqlConnection(GetConnectionString()))
            {
                var sqlcommand = sqlconnection.CreateCommand();
                sqlcommand.CommandType = System.Data.CommandType.StoredProcedure;
                sqlcommand.CommandText = "GetLoginDetails"; 

                sqlcommand.Parameters.AddWithValue("@Username", username);
                sqlcommand.Parameters.AddWithValue("@Password", password);

                sqlconnection.Open();
                using (var reader = sqlcommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        
                        return new Login
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Username = reader["Username"].ToString(),
                            Password = reader["Password"].ToString() 
                        };
                    }
                }
            }

            return null; 
        }









        // User
        public ProfileUpdate GetUsersById(int id)
        {
            ProfileUpdate user = new ProfileUpdate();
            using (sqlconnection = new SqlConnection(GetConnectionString()))
            {
                sqlcommand = sqlconnection.CreateCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "GetUsersById";
                sqlcommand.Parameters.AddWithValue("@Id", id);
                sqlconnection.Open();
                SqlDataReader sqldatareader = sqlcommand.ExecuteReader();

                while (sqldatareader.Read())
                {
                    user.Id = Convert.ToInt32(sqldatareader["Id"]);
                    user.Firstname = sqldatareader["Firstname"].ToString();
                    user.Lastname = sqldatareader["Lastname"].ToString();
                    user.Dateofbirth = Convert.ToDateTime(sqldatareader["Dateofbirth"]).Date;
                    user.Gender = sqldatareader["Gender"].ToString();
                    user.Phone = sqldatareader["Phone"].ToString();
                    user.Email = sqldatareader["Email"].ToString();
                    user.Address = sqldatareader["Address"].ToString();
                    user.Username = sqldatareader["Username"].ToString();
                }

                    sqlconnection.Close();
            }
                return user;
            
        }



        public Profile GetProfileById(int id)
        {
            Profile profile = new Profile();

            using (SqlConnection sqlconnection = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand sqlcommand = sqlconnection.CreateCommand())
                {
                    sqlcommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlcommand.CommandText = "GetProfileById";
                    sqlcommand.Parameters.AddWithValue("@Id", id);

                    sqlconnection.Open();
                    using (SqlDataReader sqldatareader = sqlcommand.ExecuteReader())
                    {
                        while (sqldatareader.Read())
                        {
                            profile.Id = Convert.ToInt32(sqldatareader["Id"]);
                            profile.Firstname = sqldatareader["Firstname"].ToString();
                            profile.Lastname = sqldatareader["Lastname"].ToString();
                            profile.Dateofbirth = Convert.ToDateTime(sqldatareader["Dateofbirth"]).Date;
                            profile.Gender = sqldatareader["Gender"].ToString();
                            profile.Phone = sqldatareader["Phone"].ToString();
                            profile.Email = sqldatareader["Email"].ToString();
                            profile.Address = sqldatareader["Address"].ToString();
                            profile.Username = sqldatareader["Username"].ToString();
                            profile.EducationId = sqldatareader["EducationId"] != DBNull.Value ? Convert.ToInt32(sqldatareader["EducationId"]) : 0;
                            profile.Qualification = sqldatareader["Qualification"] != DBNull.Value ? sqldatareader["Qualification"].ToString() : string.Empty;
                            profile.Message = sqldatareader["Message"] != DBNull.Value ? sqldatareader["Message"].ToString() : string.Empty;
                            profile.Experience = sqldatareader["Experience"] != DBNull.Value ? sqldatareader["Experience"].ToString() : string.Empty;
                            profile.Passout = sqldatareader["Passout"] != DBNull.Value ? Convert.ToInt32(sqldatareader["Passout"]) : 0;
                            profile.Photo = sqldatareader["Photo"] != DBNull.Value ? sqldatareader["Photo"].ToString() : string.Empty;
                            profile.Resume = sqldatareader["Resume"] != DBNull.Value ? sqldatareader["Resume"].ToString() : string.Empty;
                        }
                    }
                }
            }

            return profile;
        }

        public void UpdateUser(ProfileUpdate profileupdate)
        {
            
            using (sqlconnection = new SqlConnection(GetConnectionString()))
            {
                sqlcommand = sqlconnection.CreateCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "UpdateUsers";
                sqlcommand.Parameters.AddWithValue("@Id", profileupdate.Id);
                sqlcommand.Parameters.AddWithValue("@Firstname", profileupdate.Firstname);
                sqlcommand.Parameters.AddWithValue("@Lastname", profileupdate.Lastname);
                sqlcommand.Parameters.AddWithValue("@Dateofbirth", profileupdate.Dateofbirth);
                sqlcommand.Parameters.AddWithValue("@Gender", profileupdate.Gender);
                sqlcommand.Parameters.AddWithValue("@Phone", profileupdate.Phone);
                sqlcommand.Parameters.AddWithValue("@Email", profileupdate.Email);
                sqlcommand.Parameters.AddWithValue("@Address", profileupdate.Address);
                sqlcommand.Parameters.AddWithValue("@Username", profileupdate.Username);
                sqlconnection.Open();
                sqlcommand.ExecuteNonQuery();
                sqlconnection.Close();
            }
            
        }



        //Admin 


        public List<Job> GetAllJobs()
        {
            List<Job> jobList = new List<Job>();

            using (sqlconnection = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlcommand = sqlconnection.CreateCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "GetAllJobs";
                SqlDataAdapter sqldataadapter = new SqlDataAdapter(sqlcommand);
                DataTable datatable = new DataTable();

                sqlconnection.Open();
                sqldataadapter.Fill(datatable);
                sqlconnection.Close();

                foreach (DataRow datarow in datatable.Rows)
                {
                    jobList.Add(new Job
                    {
                        JobID = Convert.ToInt32(datarow["JobID"]),
                        Jobname = datarow["Jobname"].ToString(),
                        Eligibility = datarow["Eligibility"].ToString(),
                        Experience = datarow["Experience"].ToString(),
                        Location = datarow["Location"].ToString(),
                        Salary = Convert.ToDecimal(datarow["Salary"]),
                        Positions = Convert.ToInt32(datarow["Positions"]),
                        Image = datarow["Image"].ToString(),
                       
                    });
                }
            }

            return jobList;
        }
        public void Applyjob(int jobId, int userId, Job job,Profile profile)
        {
            using (var sqlconnection = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlcommand = new SqlCommand("ApplyForJob", sqlconnection); 
                sqlcommand.CommandType = CommandType.StoredProcedure;

                sqlcommand.Parameters.AddWithValue("@JobID", jobId);
                sqlcommand.Parameters.AddWithValue("@UserID", userId);
                sqlcommand.Parameters.AddWithValue("@Jobname", job.Jobname);
                sqlcommand.Parameters.AddWithValue("@Firstname", profile.Firstname);
                sqlcommand.Parameters.AddWithValue("@Lastname", profile.Lastname);
               

                sqlconnection.Open();
                sqlcommand.ExecuteNonQuery();
                sqlconnection.Close();
            }
        }


        public void InsertJob(Job job)
        {
            
            using (sqlconnection = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlcommand = new SqlCommand("InsertJob", sqlconnection);
                sqlcommand.CommandType = CommandType.StoredProcedure;

                sqlcommand.Parameters.AddWithValue("@Jobname", job.Jobname);
                sqlcommand.Parameters.AddWithValue("@Eligibility", job.Eligibility);
                sqlcommand.Parameters.AddWithValue("@Experience", job.Experience);
                sqlcommand.Parameters.AddWithValue("@Location", job.Location);
                sqlcommand.Parameters.AddWithValue("@Salary", job.Salary);
                sqlcommand.Parameters.AddWithValue("@Positions", job.Positions);
                sqlcommand.Parameters.AddWithValue("@Image", job.Image);


                sqlconnection.Open();
                sqlcommand.ExecuteNonQuery();
                sqlconnection.Close();
            }
            
        }
        public string DeleteJob(int JobID)
        {
            string result = "";
            using (sqlconnection = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlcommand = new SqlCommand("DeleteJob", sqlconnection);
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.Parameters.AddWithValue("@JobID", JobID);
                sqlcommand.Parameters.Add("@OutputMessage", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;

                sqlconnection.Open();
                sqlcommand.ExecuteNonQuery();
                result = sqlcommand.Parameters["@OutputMessage"].Value.ToString();
                sqlconnection.Close();
            }
            return result;
        }
        public Job GetJobById(int JobID)
        {
            Job job = null;

            using (sqlconnection = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlcommand = sqlconnection.CreateCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "GetJobsById";
                sqlcommand.Parameters.AddWithValue("@JobID", JobID);

                SqlDataAdapter adp = new SqlDataAdapter(sqlcommand);
                DataTable datatable = new DataTable();

                sqlconnection.Open();
                adp.Fill(datatable);
                sqlconnection.Close();

                if (datatable.Rows.Count > 0)
                {
                    DataRow datarow = datatable.Rows[0];
                    job = new Job
                    {
                        JobID = Convert.ToInt32(datarow["JobID"]),
                        Jobname = datarow["Jobname"].ToString(),
                        Eligibility = datarow["Eligibility"].ToString(),
                        Experience = datarow["Experience"].ToString(),
                        Location = datarow["Location"].ToString(),
                        Salary = Convert.ToDecimal(datarow["Salary"]),
                        Positions = Convert.ToInt32(datarow["Positions"]),
                        Image = datarow ["Image"].ToString()
                    };
                }
            }

            return job;
        }

        
        public void UpdateJob(Job job)
        {
            try
            {
                using (sqlconnection = new SqlConnection(GetConnectionString()))
                {
                    SqlCommand sqlcommand = new SqlCommand("UpdateJobs", sqlconnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    sqlcommand.Parameters.AddWithValue("@JobID", job.JobID);
                    sqlcommand.Parameters.AddWithValue("@Jobname", job.Jobname);
                    sqlcommand.Parameters.AddWithValue("@Eligibility", job.Eligibility);
                    sqlcommand.Parameters.AddWithValue("@Experience", job.Experience);
                    sqlcommand.Parameters.AddWithValue("@Location", job.Location);
                    sqlcommand.Parameters.AddWithValue("@Salary", job.Salary);
                    sqlcommand.Parameters.AddWithValue("@Positions", job.Positions);
                    sqlcommand.Parameters.AddWithValue("@Image", job.Image);


                    sqlconnection.Open();
                    sqlcommand.ExecuteNonQuery();
                    sqlconnection.Close();

                    
                }
            }
            catch (Exception ex)
            {

                throw new Exception("Error while updating the job details: " + ex.Message);
            }
        }


        public List<ProfileUpdate> GetAllUsers()
        {
            List<ProfileUpdate> userList = new List<ProfileUpdate>();

            using (sqlconnection = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlcommand = sqlconnection.CreateCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "GetAllUsers";

                SqlDataAdapter sqldataadapter = new SqlDataAdapter(sqlcommand);
                DataTable datatable = new DataTable();

                sqlconnection.Open();
                sqldataadapter.Fill(datatable);
                sqlconnection.Close();

                foreach (DataRow datarow in datatable.Rows)
                {
                    userList.Add(new ProfileUpdate
                    {
                        Id = Convert.ToInt32(datarow["Id"]),
                        Firstname = datarow["Firstname"].ToString(),
                        Lastname = datarow["Lastname"].ToString(),
                        Dateofbirth = Convert.ToDateTime(datarow["Dateofbirth"]).Date,
                        Gender = datarow["Gender"].ToString(),
                        Phone = datarow["Phone"].ToString(),
                        Email = datarow["Email"].ToString(),
                        Address = datarow["Address"].ToString(),
                        
                    });
                }
            }

            return userList;
        }

        public string Deleteuser(int Id)
        {
            string result = "";
            using (sqlconnection = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlcommand = new SqlCommand("Deleteuser", sqlconnection);
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.Parameters.AddWithValue("@Id", Id);
                sqlcommand.Parameters.Add("@OutputMessage", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;

                sqlconnection.Open();
                sqlcommand.ExecuteNonQuery();
                result = sqlcommand.Parameters["@OutputMessage"].Value.ToString();
                sqlconnection.Close();
            }
            return result;
        }


        public void Addeducation(Education education)
        {
            try
            {
                using (var sqlconnection = new SqlConnection(GetConnectionString()))
                {
                    var sqlcommand = new SqlCommand("AddEducation", sqlconnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    sqlcommand.Parameters.AddWithValue("@Qualification", education.Qualification);
                    sqlcommand.Parameters.AddWithValue("@Passout", education.Passout);
                    sqlcommand.Parameters.AddWithValue("@Experience", education.Experience);
                    sqlcommand.Parameters.AddWithValue("@Message", education.Message);
                    sqlcommand.Parameters.AddWithValue("@Photo", education.Photo);
                    sqlcommand.Parameters.AddWithValue("@Resume", education.Resume);
                    sqlcommand.Parameters.AddWithValue("@userid", education.userid);

                    sqlconnection.Open();
                    sqlcommand.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error occurred: " + ex.Message, ex);
            }
        }

        public Education GetEducationById(int EducationId)
        {
            Education education = null;

            using (sqlconnection = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlcommand = sqlconnection.CreateCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "GetEducationById";
                sqlcommand.Parameters.AddWithValue("@EducationId", EducationId);

                SqlDataAdapter adp = new SqlDataAdapter(sqlcommand);
                DataTable datatable = new DataTable();

                sqlconnection.Open();
                adp.Fill(datatable);
                sqlconnection.Close();

                if (datatable.Rows.Count > 0)
                {
                    DataRow datarow = datatable.Rows[0];
                    education = new Education
                    {
                        EducationId = Convert.ToInt32(datarow["EducationId"]),
                        Qualification = datarow["Qualification"].ToString(),
                        Passout = Convert.ToInt32(datarow["Passout"]),
                        Experience= datarow["Experience"].ToString(),
                        Message = datarow["Message"].ToString(),
                        Photo = datarow["Photo"].ToString(),
                        Resume = datarow["Resume"].ToString()
                    };
                }
            }

            return education;
        }
        public void Updateeducation(Education education)
        {
            try
            {
                using (var sqlconnection = new SqlConnection(GetConnectionString()))
                {
                    var sqlcommand = new SqlCommand("UpdateEducation", sqlconnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    sqlcommand.Parameters.AddWithValue("@EducationId", education.EducationId);
                    sqlcommand.Parameters.AddWithValue("@Qualification", education.Qualification);
                    sqlcommand.Parameters.AddWithValue("@Passout", education.Passout);
                    sqlcommand.Parameters.AddWithValue("@Experience", education.Experience);
                    sqlcommand.Parameters.AddWithValue("@Message", education.Message);
                    sqlcommand.Parameters.AddWithValue("@Photo", education.Photo);
                    sqlcommand.Parameters.AddWithValue("@Resume", education.Resume);
                    //sqlcommand.Parameters.AddWithValue("@userid", education.userid);

                    sqlconnection.Open();
                    sqlcommand.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error occurred: " + ex.Message, ex);
            }
        }
        public List<JobApplication> GetJobApplicationsByUserId(int UserID)
        {
            List<JobApplication> jobApplications = new List<JobApplication>();

            using (SqlConnection sqlconnection = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlcommand = sqlconnection.CreateCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "GetJobApplicationsByUserId";
                sqlcommand.Parameters.AddWithValue("@UserID", UserID);

                SqlDataAdapter sqldataadapter = new SqlDataAdapter(sqlcommand);
                DataTable datatable = new DataTable();

                sqlconnection.Open();
                sqldataadapter.Fill(datatable);
                sqlconnection.Close();

                foreach (DataRow datarow in datatable.Rows)
                {
                    JobApplication jobApplication = new JobApplication
                    {
                        JobApplicationId = Convert.ToInt32(datarow["JobApplicationId"]),
                        JobID = Convert.ToInt32(datarow["JobID"]),
                        UserId = Convert.ToInt32(datarow["UserId"]),
                        Jobname = datarow["Jobname"].ToString(),
                        Firstname = datarow["Firstname"].ToString(),
                        Lastname = datarow["Lastname"].ToString(),
                        Action = datarow["Action"].ToString(),
                        Applieddate = DateOnly.FromDateTime(Convert.ToDateTime(datarow["Applieddate"]))
                    };

                    jobApplications.Add(jobApplication);
                }
            }

            return jobApplications;
        }
        public List<JobApplication> GetJobApplications()
        {
            List<JobApplication> jobApplications = new List<JobApplication>();

            using (SqlConnection sqlconnection = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlcommand = sqlconnection.CreateCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "GetJobApplications";

                SqlDataAdapter sqldataadapter = new SqlDataAdapter(sqlcommand);
                DataTable datatable = new DataTable();

                sqlconnection.Open();
                sqldataadapter.Fill(datatable);
                sqlconnection.Close();

                foreach (DataRow datarow in datatable.Rows)
                {
                    JobApplication jobApplication = new JobApplication
                    {
                        JobApplicationId = Convert.ToInt32(datarow["JobApplicationId"]),
                        JobID = Convert.ToInt32(datarow["JobID"]),
                        UserId = Convert.ToInt32(datarow["UserId"]),
                        Jobname = datarow["Jobname"].ToString(),
                        Firstname = datarow["Firstname"].ToString(),
                        Lastname = datarow["Lastname"].ToString(),
                        Action = datarow["Action"].ToString(),
                        Applieddate = DateOnly.FromDateTime(Convert.ToDateTime(datarow["Applieddate"]))
                    };

                    jobApplications.Add(jobApplication);
                }
            }

            return jobApplications;
        }
        public void DeleteJobApplication(int JobApplicationId)
        {
            using (sqlconnection = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlcommand = new SqlCommand("DeleteJobApplicationById", sqlconnection);
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.Parameters.AddWithValue("@JobApplicationId", JobApplicationId);

                sqlconnection.Open();
                sqlcommand.ExecuteNonQuery();
                sqlconnection.Close();
            }
            
        }
        public void Changestatustoviewed(int JobApplicationId)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    var sqlCommand = new SqlCommand("ChangeStatusToViewed", sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    sqlCommand.Parameters.AddWithValue("@JobApplicationId",JobApplicationId);

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error occurred: " + ex.Message, ex);
            }
        }

        public List<JobApplication> GetJobApplicationsById(int JobApplicationId)
        {
            List<JobApplication> jobapplications = new List<JobApplication>();

            using (SqlConnection sqlconnection = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlcommand = sqlconnection.CreateCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "GetJobApplicationsById";
                sqlcommand.Parameters.AddWithValue("@Jobapplicationid", JobApplicationId);

                SqlDataAdapter sqldataadapter = new SqlDataAdapter(sqlcommand);
                DataTable datatable = new DataTable();

                sqlconnection.Open();
                sqldataadapter.Fill(datatable);
                sqlconnection.Close();

                foreach (DataRow datarow in datatable.Rows)
                {
                    JobApplication jobapplication = new JobApplication
                    {
                        JobApplicationId = Convert.ToInt32(datarow["JobApplicationId"]),
                        Action = datarow["Action"].ToString(),
                    };

                    jobapplications.Add(jobapplication);
                }
            }

            return jobapplications;
        }
        public void Changestatustoshortlisted(int JobApplicationId)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    var sqlCommand = new SqlCommand("ChangeStatusToShortlisted", sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    sqlCommand.Parameters.AddWithValue("@JobApplicationId", JobApplicationId);

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error occurred: " + ex.Message, ex);
            }
        }

        public void Contactus(Contactus contactus)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(GetConnectionString()))
                {
                    var sqlCommand = new SqlCommand("Addcontactdetails", sqlConnection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    sqlCommand.Parameters.AddWithValue("@Fullname", contactus.Fullname);
                    sqlCommand.Parameters.AddWithValue("@Email", contactus.Email);
                    sqlCommand.Parameters.AddWithValue("@Company", contactus.Company); 
                    sqlCommand.Parameters.AddWithValue("@Message", contactus.Message); 

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error occurred: " + ex.Message, ex);
            }
        }

        public List<Contactus> GetAllMessage()
        {
            List<Contactus> contactus = new List<Contactus>();

            using (sqlconnection = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlcommand = sqlconnection.CreateCommand();
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.CommandText = "GetAllMessage";

                SqlDataAdapter sqldataadapter = new SqlDataAdapter(sqlcommand);
                DataTable datatable = new DataTable();

                sqlconnection.Open();
                sqldataadapter.Fill(datatable);
                sqlconnection.Close();

                foreach (DataRow datarow in datatable.Rows)
                {
                    contactus.Add(new Contactus
                    {
                        Contactusid = Convert.ToInt32(datarow["Contactusid"]),
                        Fullname = datarow["Fullname"].ToString(),
                        Email = datarow["Email"].ToString(),
                        Company = datarow["Company"].ToString(),
                        Message = datarow["Message"].ToString(),
                        
                    });
                }
            }

            return contactus;
        }
        public void DeleteMessage(int Contactusid)
        {
            using (sqlconnection = new SqlConnection(GetConnectionString()))
            {
                SqlCommand sqlcommand = new SqlCommand("Deletemessage", sqlconnection);
                sqlcommand.CommandType = CommandType.StoredProcedure;
                sqlcommand.Parameters.AddWithValue("@Contactusid", Contactusid);

                sqlconnection.Open();
                sqlcommand.ExecuteNonQuery();
                sqlconnection.Close();
            }

        }

    }
}
