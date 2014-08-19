﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Models;

namespace ExpressQuiz.ViewModels
{
    public class QuizzesViewModel
    {
        public List<QuizCategoryViewModel> QuizCategories { get; set; }

        public List<TopListItem> TopQuizzes { get; set; } 

        public List<Quiz> Quizzes { get; set; }

        public int SelectedFilter { get; set; }
        public IEnumerable<SelectListItem> Filters { get; set; }
        public QuizFilter Filter { get; set; }

        public int SelectedCategory { get; set; }
        
    }
}