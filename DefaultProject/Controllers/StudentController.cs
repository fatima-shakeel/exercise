﻿using System;
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
        public IActionResult Index()
        {
            return View();
        }
    }
}