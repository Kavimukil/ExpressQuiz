﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ExpressQuiz.Core.Models;
using ExpressQuiz.Core.Repos;
using ExpressQuiz.Core.Services;
using ExpressQuiz.Extensions;


namespace ExpressQuiz.ViewModels
{
    public static class ModelExtensions
    {
        public static QuizDetailsViewModel ToViewModel(this Quiz quiz, IService<Quiz> quizService, IRepo<QuizResult> quizResults, IRepo<QuizRating> quizRatings)
        {
            var vm = new QuizDetailsViewModel();

            vm.AvgLevel = quizService.GetAverageLevel(quiz);
            vm.AvgRating = quizService.GetAverageRating(quiz);
            vm.AvgScore = quizService.GetAverageScore(quiz);
            vm.AvgTime = quizService.GetAverageTime(quiz);
            vm.AvgTimePercent = quizService.GetAverageTimePercent(quiz);
            
            vm.Quiz = quiz;

            return vm;
        }

        public static List<QuizCategoryViewModel> ToViewModel(this IQueryable<QuizCategory> categories,IQueryable<Quiz> quizzes, int? catId)
        {
            var cats = (from c in categories
                                 orderby c.Name
                                 select new QuizCategoryViewModel()
                                 {
                                     Id = c.Id,
                                     Name = c.Name,
                                     IsSelected = catId.HasValue ? (c.Id == catId.Value) : false,
                                     QuizCount = quizzes.Count(x => x.Category.Id == c.Id)
                                 }).ToList();

            cats.Insert(0, new QuizCategoryViewModel()
            {
                Id = -1,
                Name = "All",
                IsSelected = catId.HasValue ? (-1 == catId.Value) : true,
                QuizCount = quizzes.Count()
            });

            return cats;
        }

        public static QuizzesViewModel ToViewModel(this IQueryable<Quiz> quizzes, 
            IService<Quiz> quizService,
            IRepo<QuizCategory> categories, 
            int? catId)
        {
            var vm = new QuizzesViewModel();

            vm.QuizCategories = categories.GetAll().ToViewModel(quizService.GetPublicQuizzes(), catId);

            vm.Filter = QuizFilter.Newest;
            vm.Quizzes = quizzes.ToList();

            vm.TopQuizzes = quizService.GetBy(QuizFilter.Rating, descending: true, count: 10).Select(x => new TopListItem()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            vm.SelectedCategory = catId.HasValue ? catId.Value : -1;

            return vm;
        }

        public static EditQuizViewModel ToViewModel(this Quiz quiz, IRepo<QuizCategory> categories)
        {
            var vm = new EditQuizViewModel();


            vm.Quiz = quiz;

            var sortedQuestions = quiz.Questions.AsQueryable().AsNoTracking().OrderBy(x => x.OrderId).Select(x => x.Id);
            vm.Order = string.Join(",", sortedQuestions);
            vm.Categories = categories.GetCategoriesAsSelectList();
            vm.SelectedCategory = quiz.Category.Id;

            vm.EstimatedTime = (uint)quiz.Questions.Sum(x => x.EstimatedTime);

            return vm;
        }

        public static EditQuestionViewModel ToViewModel(this Question question)
        {
            var vm = new EditQuestionViewModel();

            vm.Question = question;
            vm.Order = string.Join(",", question.Answers.AsQueryable().AsNoTracking().OrderBy(x => x.OrderId).Select(x => x.Id));
       

            return vm;
        }

        public static ActiveQuizViewModel ToViewModel(this Quiz quiz)
        {
            var vm = new ActiveQuizViewModel();
            vm.Quiz = quiz;
            vm.EstimatedTime = quiz.Questions.Sum(x => x.EstimatedTime);

            return vm;
        }

        public static QuestionReviewViewModel ToViewModel(this Question question,int resultId, int userAnswerId )
        {
            var vm = new QuestionReviewViewModel();
            vm.Question = question;
            vm.QuizResultId = resultId;
            vm.UserAnswerId = userAnswerId;
            return vm;
        }

        public static QuizReviewViewModel ToViewModel(this QuizResult quizResult, IService<Quiz> quizzes, IRepo<Answer> answers  )
        {
            var vm = new QuizReviewViewModel();
            var quiz = quizzes.Get(quizResult.QuizId);
            var questions = quiz.Questions.Where(x => x.QuizId == quizResult.QuizId);



            var qDetails = new List<QuizReviewItem>();
            foreach (var userAnswer in quizResult.Answers)
            {
                var answer = answers.GetAll().FirstOrDefault(x => x.Id == userAnswer.AnswerId);
                var isAnswerCorrect = answer != null ? answer.IsCorrect : false;
                var questionText = questions.First(x => x.Id == userAnswer.QuestionId).Text;

                qDetails.Add(new QuizReviewItem(isAnswerCorrect, questionText, userAnswer.QuestionId));


            }

           
            var allowPoints = quiz.AllowPoints;
            string scoreText;
            if (allowPoints)
            {
                var totalPoints = questions.Sum(x => x.Points);
                var scoredPoints = quizResult.Score*totalPoints/100;
                scoreText = scoredPoints + " / " + totalPoints + " points";
            }
            else
            {
                scoreText = quizResult.Score + "%";
            }

            vm.Items = qDetails;
            vm.Result = quizResult;
            vm.ScoreText = scoreText;
            vm.EllapsedTimePercent = (int)((double)quizResult.EllapsedTime/(double)questions.Sum(x => x.EstimatedTime)*100);
            vm.QuizId = quizResult.QuizId;

            return vm;
        }


        public static CreateQuizViewModel ToViewModel(this Quiz quiz, IRepo<QuizCategory> categories, string userName)
        {
            var vm = new CreateQuizViewModel();
            vm.Categories = categories.GetCategoriesAsSelectList();
            vm.Quiz = new Quiz();
            vm.Quiz.CreatedBy = userName;

            return vm;
        }
    }
}