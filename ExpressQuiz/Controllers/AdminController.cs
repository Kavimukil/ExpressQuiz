﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressQuiz.Migrations;
using ExpressQuiz.Models;
using ExpressQuiz.Repos;

namespace ExpressQuiz.Controllers
{
   // [Authorize(Roles = "Administrator")]
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public FileResult Export()
        {
            var quizRepo = new Repo<Quiz>(new QuizDbContext());
            var quizzes = from m in quizRepo.GetAll().OrderByDescending(x=> x.Created)
                          select m;
            DataProvider.Export(quizzes.ToList(), System.Web.HttpContext.Current.Server.MapPath("~/bin/App_Data/seeddata.xml"));
            return File(System.Web.HttpContext.Current.Server.MapPath("~/bin/App_Data/seeddata.xml"), "text/xml");
        }

     
        public ActionResult Import()
        {

            var ctx = new QuizDbContext();


            var quizzes = DataProvider.Import( System.Web.HttpContext.Current.Server.MapPath("~/bin/App_Data/seeddata.xml"));

            ctx.Quizzes.AddOrUpdate(i => i.Name,
                       quizzes.ToArray()
                  );
            try
            {
                ctx.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
           
            
            return View("Index");
        }
    }
}