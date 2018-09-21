﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DefaultProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DefaultProject.Controllers
{
    public class StudentController : Controller
    {
        public ExerciseContext _ORM = null;
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
        public IActionResult RegisterStudent(Student S, IFormFile Resume)
        {
            _ORM.Student.Add(S);
            _ORM.SaveChanges();
            ViewBag.Message = "REGISTRATION DONE SUCCESSFULLY";

            string wwwRootPath = _ENV.WebRootPath;

           
            string CVPath = "/WebData/CVs/" + Guid.NewGuid().ToString() + Path.GetExtension(Resume.FileName);
            FileStream CVS = new FileStream(wwwRootPath + CVPath, FileMode.Create);
            Resume.CopyTo(CVS);
            CVS.Close();
            S.CV = CVPath;


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