using B2DriverLicense.Core.Entities;
using B2DriverLicense.Service;
using B2DriverLicense.Service.Repositories;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace B2DriverLicense.AppCrawler
{
    public class ConsoleApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuestionRepository _question;
        private readonly IHintRepository _hint;
        private readonly IAnswerRepository _answer;

        private static StaticURL staticURL = new StaticURL();

        public ConsoleApplication(IUnitOfWork unitOfWork, IQuestionRepository question, IHintRepository hint, IAnswerRepository answer)
        {
            this._unitOfWork = unitOfWork;
            this._question = question;
            this._hint = hint;
            this._answer = answer;
        }

        public void Run()
        {
            
            InsertQuestionToDB(staticURL.Url_1[0], 1);
           
            Console.ReadKey();
        }

        public void InsertQuestionToDB(string url, int chapterId)
        {
            var web = new HtmlWeb() { OverrideEncoding = Encoding.UTF8 };
            var doc = web.Load(url);

            var questionNodes = doc.DocumentNode.QuerySelectorAll("div.entry-content.article > h2 > strong > span");
            if(questionNodes.Count() == 0) questionNodes = doc.DocumentNode.QuerySelectorAll("div.entry-content.article > p > strong > span");
            
            var answerNodes = doc.DocumentNode.QuerySelectorAll("div.entry-content.article > ol");
            var hintNodes = doc.DocumentNode.QuerySelectorAll("div.entry-content.article > p > em > span").ToList();
            //var imgNodes = doc.DocumentNode.QuerySelectorAll("div.entry-content.article > p > img").ToList();

            foreach (var (item, index) in questionNodes.WithIndex())
            {
                var splitedString = item.InnerText.Split(":");
                var id = new string(splitedString[0].Where(Char.IsDigit).ToArray());

                if (splitedString.Count() > 1)
                {
                    if (string.IsNullOrEmpty(splitedString[1]))
                    {
                        Console.WriteLine("It's not a question");
                    }
                    if (string.IsNullOrEmpty(id))
                    {
                        Console.WriteLine("It's not a question");
                    }
                    else
                    {
                        var question = new Question() { ChapterId = chapterId, Number = Convert.ToInt32(id), Content = splitedString[1].Replace("&#8221;", "\"").Replace("&#8220;", "\"") };
                        var questToAdd = AddCorrectAnswerToQuestion(hintNodes[index], question);
                        try
                        {
                            _question.Add(questToAdd);
                            Console.WriteLine($"Add question: Cau {question.Number}");
                            var answerlst = answerNodes[index];
                            InsertAnswer(answerlst, question.Id);
                            //InsertHint(hintNodes, question.Id, index);
                        }
                        catch (Exception)
                        {

                            Console.WriteLine($"Failed to add question: Cau {question.Number}");
                        }
                    }
                    
                }
            }
          
        }  
        
        public List<Hint> GetHints(string url)
        {
            var hints = new List<Hint>();
            var web = new HtmlWeb() { OverrideEncoding = Encoding.UTF8 };
            var doc = web.Load(url);

            var answerNodes = doc.DocumentNode.QuerySelectorAll("div.entry-content.article > p > em > span").ToList();

            for (int i = 0; i < answerNodes.Count(); i+=2)
            {
                var hint = new Hint { Id = i + 1, Content = answerNodes[i].InnerText.Replace("Gợi ý: ", "") };
                hints.Add(hint);

            }

            return hints;
        }

        public void InsertAnswer(HtmlNode node, int questionId)
        {
            foreach (var (ans, ansIndex) in node.QuerySelectorAll("li").WithIndex())
            {
                var answer = new Answer { Key = ansIndex + 1, Content = ans.InnerText, QuestionId = questionId };
                try
                {
                    _answer.Add(answer);
                    Console.WriteLine($"Added Answer: {answer.Content}");
                }
                catch
                {
                    Console.WriteLine($"Failed to add answer: {answer.Content}");
                }

            }
        }

        public void InsertHint(List<HtmlNode> nodes, int questionId, int index)
        {
            var hint = new Hint { Content = nodes[index * 2].InnerText.Replace("Gợi ý: ", ""), QuestionId = questionId };
            try
            {
                _hint.Add(hint);
                Console.WriteLine($"Added Hint {hint.Content}");
            }
            catch (Exception)
            {
                Console.WriteLine($"Failed to add hint: {hint.Content}");
            }
        }

        public void AddImageToQuestion(HtmlNode node)
        {
            var url = node.Attributes["src"].Value;
            var folderPath = Directory.GetCurrentDirectory() + "/images/";
            var filePath = Extensions.DownloadImg(url, folderPath);

            if (!string.IsNullOrEmpty(filePath))
            {
                try
                {
                    var fileName = Path.GetFileNameWithoutExtension(filePath);
                    var questionNumber = Convert.ToInt32(new string(fileName.Where(Char.IsDigit).ToArray()));
                    var question = _question.GetQuestionByNumber(questionNumber);
                    var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    var br = new BinaryReader(fs);
                    question.ImageData = br.ReadBytes((int)fs.Length);
                    question.ImageTitle = fileName;
                    br.Close();
                    fs.Close();
                    Console.WriteLine($"Image: {fileName} has been addded to question Cau {question.Number}.");
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to add image.");
                }
            }           
        }

        public Question AddCorrectAnswerToQuestion(List<HtmlNode> nodes, Question question, int index)
        {
            var ans = new string(nodes[index * 2 + 1].InnerText.Where(Char.IsDigit).ToArray());
            question.CorrectAnswer = Convert.ToInt32(ans);

            return question;
        }

        public Question AddCorrectAnswerToQuestion(HtmlNode node, Question question)
        {
            var ans = new string(node.InnerText.Where(Char.IsDigit).ToArray());
            question.CorrectAnswer = Convert.ToInt32(ans);

            return question;
        }
    }


}
