using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using DefaultProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DefaultProject.Controllers
{
    public class StudentController : Controller
    {
        ExerciseContext _ORM = null;
        IHostingEnvironment _ENV = null;


        public StudentController(ExerciseContext ORM, IHostingEnvironment ENV)
        {
            _ORM = ORM;
            _ENV = ENV;
        }

        [HttpGet]
        public IActionResult RegisterStudent()
        {
          
            return View();
        }

        [HttpPost]
        public IActionResult RegisterStudent(Student S,IFormFile ProfilePicture,IFormFile Resume)
        {
            string wwwRoothPath = _ENV.WebRootPath;
            string FTPPathForPPs = wwwRoothPath + "/WebData/PPs";

            string UniqueName = Guid.NewGuid().ToString();
            string FileExtension = Path.GetExtension(ProfilePicture.FileName);

            FileStream FS = new FileStream(FTPPathForPPs+UniqueName+FileExtension,FileMode.Create);

            ProfilePicture.CopyTo(FS);
            FS.Close();


            S.ProfilePicture = "/WebData/PPs/" + UniqueName + FileExtension;


            string FTPPathForCVs = wwwRoothPath + "/WebData/CVs";
            string UniqueNames = Guid.NewGuid().ToString();
            string FileExtensions = Path.GetExtension(Resume.FileName);
            FileStream Fs = new FileStream(FTPPathForPPs + UniqueNames + FileExtensions, FileMode.Create);

            Resume.CopyTo(Fs);
            Fs.Close();


            S.ProfilePicture = "/WebData/PPs/" + UniqueName + FileExtension;

            _ORM.Student.Add(S);
            _ORM.SaveChanges();
            ViewBag.Message = "REGISTRATION DONE SUCCESSFULLY";

            
            return View();
        }
        [HttpGet]
        public IActionResult SendEmail()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendEmail(Student S,IFormFile ProfilePicture,IFormFile Resume)
        {
            
            string wwwRootPath = _ENV.WebRootPath;
            string FTPPathForPPs = wwwRootPath + "/WebData/PPs";

            string UniqueNames = Guid.NewGuid().ToString();
            string FileExtensions = Path.GetExtension(ProfilePicture.FileName);

            FileStream Fs = new FileStream(wwwRootPath + FTPPathForPPs+UniqueNames, FileMode.Create);
            ProfilePicture.CopyTo(Fs);
            Fs.Close();


            S.ProfilePicture = "/WebData/PPs/" + UniqueNames + FileExtensions;


            string CVPath = "/WebData/CVs/" + Guid.NewGuid().ToString() + Path.GetExtension(Resume.FileName);
            FileStream CVS = new FileStream(wwwRootPath + CVPath, FileMode.Create);
            Resume.CopyTo(CVS);
            CVS.Close();
            S.CV = CVPath;

            _ORM.Student.Add(S);
            _ORM.SaveChanges();
           
            // Email object

            MailMessage Email = new MailMessage();
            Email.From = new MailAddress("dazzlinglove98@gmail.com");
            Email.To.Add(new MailAddress(S.Email));
            Email.CC.Add(new MailAddress("dazzlinglove98@gmail.com"));
            Email.Subject = "Welcome to ABC";
            Email.Body = "Dear " + S.Name + ",<br><br>" +
                "Thanks for registering with ABC, We are glad to have you in our system." +
                "<br><br>" +
                "<b>Regards</b>,<br>ABC Team";
            Email.IsBodyHtml = true;

            if (!string.IsNullOrEmpty(S.ProfilePicture))
            {
                Email.Attachments.Add(new Attachment(wwwRootPath + S.CV));
            }
            if (!string.IsNullOrEmpty(S.CV))
            {
                Email.Attachments.Add(new Attachment(wwwRootPath + S.CV));
            }
            //
            //smtp object
            SmtpClient oSMTP = new SmtpClient();
            oSMTP.Host = "smtp.gmail.com";
            oSMTP.Port = 587; //465 //25
            oSMTP.EnableSsl = true;
            oSMTP.Credentials = new System.Net.NetworkCredential("dazzlinglove98@gmail.com", "fatimashakeel");

            try
            {
                oSMTP.Send(Email);
            }
            catch (Exception ex)
            {

            }

    //
            
            return View();
        }

        public IActionResult StudentInfo(int Id)
        {
            Student S = _ORM.Student.Where(m => m.Id == Id).FirstOrDefault<Student>();


            return View(S);
        }
        
        [HttpGet]
       public IActionResult StudentsList()
        {
            IList<Student> StudentsList = _ORM.Student.ToList<Student>();
            
            return View(StudentsList);
        }


        [HttpPost]
        public IActionResult AllStudents(string SearchByName, string SearchByRollNo,string SearchByDepartment)
        {

            IList<Student> AllStudents = _ORM.Student.Where(m => m.Name.Contains(SearchByName) || m.RollNo.Contains(SearchByName) || m.Department.Contains(SearchByName)).ToList<Student>();
            return View(AllStudents);
        }

        [HttpGet]
        public IActionResult EditInfo(int Id)
        {
            Student S = _ORM.Student.Where(m => m.Id == Id).FirstOrDefault<Student>();
            return View(S);
        }


        [HttpPost]
        public IActionResult EditInfo(Student S)
        {
            _ORM.Student.Update(S);
            _ORM.SaveChanges();
            
            return RedirectToAction("StudentsList");
        }

        public IActionResult DeleteStudent(Student S)
        {
            _ORM.Student.Remove(S);
            _ORM.SaveChanges();
            ViewBag.Message = "DELETED SUCCESSFULLY";
            return RedirectToAction("StudentsList");

           
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}