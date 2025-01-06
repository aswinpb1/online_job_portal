using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineJobPortal.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        public DateTime Dateofbirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
       
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$",
    ErrorMessage = "Password must be at least 6 characters and include at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; }


        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string Confirmpassword { get; set; }
    }
    public class Login
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
    public class Profile
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        public DateTime Dateofbirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string Phone { get; set; }

        

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }
        public string Username { get; set; }
        public int EducationId { get; set; }
        public string Qualification {  get; set; }
        public string Message { get; set; }
        public string Experience { get; set; }
        public int Passout { get; set; }
        public string Photo {  get; set; }
        public string Resume { get; set; }
    }
    public class Job
    {
        [Key]
        public int JobID { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Job name cannot exceed 100 characters.")]
        public string Jobname { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Eligibility cannot exceed 100 characters.")]
        public string Eligibility { get; set; }

        [Required]
        public string Experience { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Location cannot exceed 100 characters.")]
        public string Location { get; set; }

        [Required]
        [Range(0, 99999999.99, ErrorMessage = "Salary must be a valid decimal number.")]
        public decimal Salary { get; set; }

        [Required]
        public int Positions { get; set; }

        public string? Image { get; set; }

    }
    public class ProfileUpdate
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        public DateTime Dateofbirth { get; set; }

        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string Phone { get; set; }



        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }
        public string Username { get; set; }
    }
    public class Education
    {
        [Key]
        public int EducationId { get; set; }
        [Required]
        public string Qualification { get; set; }
        public string Message { get; set; }
        public string Experience { get; set; }
        [Required]
        public int Passout { get; set; }
        public string Photo { get; set; }
        public string Resume { get; set; }
        public int userid {  get; set; }
    }
    public class JobApplication
    {
        public int JobApplicationId { get; set; }
        public int JobID { get; set; }
        public int UserId { get; set; }
        public string Jobname { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Action { get; set; }
        public DateOnly Applieddate { get; set; }
    }

    public class Contactus
    {
        public int Contactusid { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Message { get; set; }
    }


}
