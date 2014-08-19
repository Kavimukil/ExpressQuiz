﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpressQuiz.Core.Models
{
    public class Quiz : Entity
    {

        //[Required]
        //public string CreatorId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(1000)]
        public string Summary { get; set; }

         [DisplayName("Use timer")]
        public bool IsTimeable { get; set; }

         [DisplayName("Allow question points")]
        public bool AllowPoints { get; set; }

        //[DataType(DataType.DateTime)]
        [Column(TypeName = "DateTime2")]
        public DateTime Created { get; set; }

        [Required]
        public string CreatedBy { get; set; }

         [DisplayName("Locked")]
        public bool Locked { get; set; }

        public int QuizCategoryId { get; set; }
        public virtual QuizCategory Category { get; set; }
        public virtual ICollection<Question> Questions { get; set; }

        public Quiz()
        {
            Questions = new List<Question>();
        }
    }

  
}