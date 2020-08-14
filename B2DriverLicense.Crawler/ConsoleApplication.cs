using B2DriverLicense.Core.Entities;
using B2DriverLicense.Service;
using B2DriverLicense.Service.Repositories;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace B2DriverLicense.AppCrawler
{
    public class ConsoleApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuestionRepository _question;
        private readonly IHintRepository _hint;
        private readonly IAnswerRepository _answer;

        public ConsoleApplication(IUnitOfWork unitOfWork, IQuestionRepository question, IHintRepository hint, IAnswerRepository answer)
        {
            this._unitOfWork = unitOfWork;
            this._question = question;
            this._hint = hint;
            this._answer = answer;
        }

        public void Run()
        {
            var url = "http://daotaolaixehcm.vn/thi-bang-lai-xe/cac-khai-niem-p1/";

            var quests = GetQuestions(url);
            foreach (var item in quests)
            {
                Console.WriteLine($"Câu {item.Id}: {item.Content}");
            }
            Console.ReadKey();
        }

        public static List<Question> GetQuestions(string url)
        {
            var web = new HtmlWeb() { OverrideEncoding = Encoding.UTF8 };
            var doc = web.Load(url);

            var questionNodes = doc.DocumentNode.QuerySelectorAll("div.entry-content.article > h2 > strong > span");
            var answerNodes = doc.DocumentNode.QuerySelectorAll("div.entry-content.article > ol");
            var questions = new List<Question>();

            foreach (var (item, index) in questionNodes.WithIndex())
            {
                var splitedString = item.InnerText.Split(":");
                var id = splitedString[0].Replace("Câu ", "");
                var question = new Question() { Id = Convert.ToInt32(id), Content = splitedString[1] };
                questions.Add(question);

                var anss = answerNodes[index];
                foreach (var (ans, ansIndex) in anss.QuerySelectorAll("li").WithIndex())
                {
                    var answer = new Answer { Key = ansIndex, Content = ans.InnerText };
                }
            }
            return questions;
        }       
    }


}
