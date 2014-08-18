﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ExpressQuiz.Models
{
    public class QuizResult : Entity
    {

   
        public virtual ICollection<UserAnswer> Answers { get; set; }

        public int QuizId { get; set; }
        public virtual Quiz Quiz { get; set; }

        [DisplayName("Time")]
        public int EllapsedTime { get; set; }
        public int Score { get; set; }
        public string UserId { get; set; }
       
    }
}