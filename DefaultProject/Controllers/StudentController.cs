using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DefaultProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace DefaultProject.Controllers
{
    public class StudentController : Controller
    {
        public ExerciseContext _ORM = null;

        public StudentController(ExerciseContext ORM)
        {
            _ORM = ORM;
        }

        [HttpGet]
        public IActionResult RegisterStudent()
        {
          
            return View();
        }

        [HttpPost]
        public IActionResult RegisterStudent(Student S)
        {
            _ORM.Student.Add(S);
            _ORM.SaveChanges();
            ViewBag.Message = "REGISTRATION DONE SUCCESSFULLY";
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