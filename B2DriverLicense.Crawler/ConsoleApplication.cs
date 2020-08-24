using B2DriverLicense.AppCrawler.Models;
using B2DriverLicense.Core.Entities;
using B2DriverLicense.Service;
using B2DriverLicense.Service.Repositories;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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

        public async Task Run()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

            Console.WriteLine("Driver License Question Crawler in C#\r");
            Console.WriteLine("----------------------------------------\n");
            Console.WriteLine("Connection string: " + configuration.GetConnectionString("DefaultConnection"));
            Console.Write("Continue ?(y/n): ");
            var key = Console.ReadLine();

            if(key == "y")
            {
                foreach (var item in staticURL.Url_0)
                {
                    await InsertQuestionToDBAsync(item, 1);
                    Console.WriteLine($"Chapter 1 link: {item} - crawl completed !\r");
                    Console.WriteLine("----------------------------------------\n");
                }

                Console.WriteLine("Chapter 1 crawl completed !");
                Console.WriteLine("----------------------------------------\n");

                foreach (var item in staticURL.Url_1)
                {
                    await InsertQuestionToDBAsync(item, 1);
                    Console.WriteLine($"Chapter 1 link: {item} - crawl completed !\r");
                    Console.WriteLine("----------------------------------------\n");
                }

                Console.WriteLine("Chapter 1 crawl completed !");
                Console.WriteLine("----------------------------------------\n");

                foreach (var item in staticURL.Url_2)
                {
                    await InsertQuestionToDBAsync(item, 2);
                    Console.WriteLine($"Chapter 2 link: {item} - crawl completed !\r");
                    Console.WriteLine("----------------------------------------\n");
                }

                Console.WriteLine("Chapter 2 crawl completed !");
                Console.WriteLine("----------------------------------------\n");

                foreach (var item in staticURL.Url_3)
                {
                    await InsertQuestionToDBAsync(item, 3);
                    Console.WriteLine($"Chapter 3 link: {item} - crawl completed !\r");
                    Console.WriteLine("----------------------------------------\n");
                }

                Console.WriteLine("Chapter 3 crawl completed !");
                Console.WriteLine("----------------------------------------\n");

                foreach (var item in staticURL.Url_4)
                {
                    await InsertQuestionToDBAsync(item, 4);
                    Console.WriteLine($"Chapter 4 link: {item} - crawl completed !\r");
                    Console.WriteLine("----------------------------------------\n");
                }

                Console.WriteLine("Chapter 4 crawl completed !");
                Console.WriteLine("----------------------------------------\n");

                foreach (var item in staticURL.Url_5)
                {
                    await InsertQuestionToDBAsync(item, 5);
                    Console.WriteLine($"Chapter 5 link: {item} - crawl completed !\r");
                    Console.WriteLine("----------------------------------------\n");
                }

                Console.WriteLine("Chapter 5 crawl completed !");
                Console.WriteLine("----------------------------------------\n");

                foreach (var item in staticURL.Url_6)
                {
                    await InsertQuestionToDBAsync(item, 6);
                    Console.WriteLine($"Chapter 6 link: {item} - crawl completed !\r");
                    Console.WriteLine("----------------------------------------\n");
                }

                Console.WriteLine("Chapter 6 crawl completed !");
                Console.WriteLine("----------------------------------------\n");

                foreach (var item in staticURL.Url_7)
                {
                    await InsertQuestionToDBAsync(item, 7);
                    Console.WriteLine($"Chapter 7 link: {item} - crawl completed !\r");
                    Console.WriteLine("----------------------------------------\n");
                }

                Console.WriteLine("Chapter 7 crawl completed !");
                Console.WriteLine("----------------------------------------\n");

                foreach (var item in staticURL.Url_8)
                {
                    await InsertQuestionChapter8Async(item);
                    Console.WriteLine($"Chapter 8 link: {item} - crawl completed !\r");
                    Console.WriteLine("----------------------------------------\n");
                }

                Console.WriteLine("Chapter 8 crawl completed !");
                Console.WriteLine("----------------------------------------\n");

                try
                {
                    if (await _unitOfWork.SaveChangeAsync() > 0)
                    {
                        Console.WriteLine("Saved data to DB !");
                        Console.WriteLine("Done !");
                        Console.WriteLine("Press any key to end !");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Failed to save data to DB !");
                        Console.WriteLine("Press any key to end !");
                        Console.ReadKey();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to save data to DB !");
                    Console.WriteLine("Error: " + ex.Message);
                    Console.WriteLine("Press any key to end !");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Press any key to end !");
                Console.ReadKey();
            }
        }

        

        public async Task InsertQuestionToDBAsync(string url, int chapterId)
        {
            var web = new HtmlWeb() { OverrideEncoding = Encoding.UTF8 };
            var doc = web.Load(url);

            var strongNode = doc.DocumentNode.QuerySelectorAll("div.entry-content.article > h2 > strong").ToList();
            if (strongNode.Count() == 0) strongNode = doc.DocumentNode.QuerySelectorAll("div.entry-content.article > p > strong").ToList();

            var questionNodes = strongNode.GetListQuestionNodeFromStrongNode();

            var imgNodes = doc.DocumentNode.QuerySelectorAll("div.entry-content.article > p > img").ToList();
            var answerNodes = new List<HtmlNode>();

            var answerNodesTable = doc.DocumentNode.QuerySelectorAll("div.entry-content.article > table");
            var answerNodesOL = doc.DocumentNode.QuerySelectorAll("div.entry-content.article > ol");

            answerNodes.AddRange(answerNodesTable);
            answerNodes.AddRange(answerNodesOL);            

            var pNodes = doc.DocumentNode.QuerySelectorAll("div.entry-content.article > p").ToList();
            var correctAnswerNodes = pNodes.TakeCorrectAnswerNode();

            foreach (var (item, index) in questionNodes.WithIndex())
            {
                var splitedString = item.InnerText.Split(":");
                var id = new string(splitedString[0].Where(Char.IsDigit).ToArray());

                if (splitedString.Count() > 1)
                {
                    if (string.IsNullOrEmpty(splitedString[1]))
                    {
                        Console.WriteLine("It's not a question");
                        Console.WriteLine("---------------------------------------");
                    }
                    else if (string.IsNullOrEmpty(id))
                    {
                        Console.WriteLine("It's not a question");
                        Console.WriteLine("---------------------------------------");
                    }
                    else
                    {
                        int questNum = Convert.ToInt32(id);

                        var image = new ImageVM();

                        if(imgNodes.Count() > 1 && imgNodes.Last().Attributes["src"].Value != "http://daotaolaixehcm.vn/wp-content/uploads/2020/04/onthilaixe-610x1024.jpg")
                        {
                            var imgNode = answerNodes[index].CheckTableNode() ? answerNodes[index].GetImageNodeFromTable() : imgNodes.SearchImageNodeByQuestionNumber(questNum);

                            if (imgNode != null)
                            {
                                image = imgNode.GenerateImageFromNode();
                            }
                        }

                        var question = new Question()
                        {
                            ChapterId = chapterId,
                            Number = questNum,
                            Content = splitedString[1].Replace("&#8221;", "\"").Replace("&#8220;", "\""),
                            CorrectAnswer = correctAnswerNodes.Count() > 0 ? correctAnswerNodes[index].GetCorrectAnswerFromNode() : 0,
                            ImageTitle = image.ImageTitle,
                            ImageData = image.ImageData,
                        };

                        question.Answers = new List<Answer>(answerNodes[index].CheckTableNode() ? answerNodes[index].GetListAnswerFromTable() : answerNodes[index].GetListAnswerFromNode());

                        try
                        {
                           
                            await _question.AddAsync(question);
                            Console.WriteLine($"Added question: Cau {question.Number}");
                            Console.WriteLine("---------------------------------------");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to add question !");
                            Console.WriteLine("Error: " + ex.Message);
                            Console.WriteLine("-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-");
                        }
                    }

                }
            }

        }  
       
        public async Task InsertQuestionChapter8Async(string url)
        {
            var web = new HtmlWeb() { OverrideEncoding = Encoding.UTF8 };
            var doc = web.Load(url);

            var strongNode = doc.DocumentNode.QuerySelectorAll("div.entry-content.article > h2 > strong").ToList();
            if (strongNode.Count() == 0) strongNode = doc.DocumentNode.QuerySelectorAll("div.entry-content.article > p > strong").ToList();

            var questionNodes = strongNode.GetListQuestionNodeFromStrongNode();

            var imgNodes = doc.DocumentNode.QuerySelectorAll("div.entry-content.article > p > img").ToList();
            
            var answerNodes = doc.DocumentNode.QuerySelectorAll("div.entry-content.article > p > span").ToList();
            if (answerNodes.Count() < questionNodes.Count()) answerNodes = doc.DocumentNode.QuerySelectorAll("div.entry-content.article > h2 > span").ToList();

            var ansLst = answerNodes.GetListAnswerNodesChapter8();

            var pNodes = doc.DocumentNode.QuerySelectorAll("div.entry-content.article > p").ToList();
            var correctAnswerNodes = pNodes.TakeCorrectAnswerNode();

            foreach (var (item, index) in questionNodes.WithIndex())
            {
                var splitedString = item.InnerText.Split(":");
                var id = new string(splitedString[0].Where(Char.IsDigit).ToArray());

                if (splitedString.Count() > 1)
                {
                    if (string.IsNullOrEmpty(splitedString[1]))
                    {
                        Console.WriteLine("It's not a question");
                        Console.WriteLine("---------------------------------------");
                    }
                    else if (string.IsNullOrEmpty(id))
                    {
                        Console.WriteLine("It's not a question");
                        Console.WriteLine("---------------------------------------");
                    }
                    else
                    {
                        int questNum = Convert.ToInt32(id);

                        var image = new ImageVM();

                        if (imgNodes.Count() > 1 && imgNodes.Last().Attributes["src"].Value != "http://daotaolaixehcm.vn/wp-content/uploads/2020/04/onthilaixe-610x1024.jpg")
                        {
                            var imgNode = answerNodes[index].CheckTableNode() ? answerNodes[index].GetImageNodeFromTable() : imgNodes.SearchImageNodeByQuestionNumber(questNum);

                            if (imgNode != null)
                            {
                                image = imgNode.GenerateImageFromNode();
                            }
                        }

                        var question = new Question()
                        {
                            ChapterId = 8,
                            Number = questNum,
                            Content = splitedString[1].Replace("&#8221;", "\"").Replace("&#8220;", "\""),
                            CorrectAnswer = correctAnswerNodes.Count() > 0 ? correctAnswerNodes[index].GetCorrectAnswerFromNode() : 0,
                            ImageTitle = image.ImageTitle,
                            ImageData = image.ImageData,
                            Answers = new List<Answer>(ansLst[index].GetListAnswerFromNodes())
                        };

                        try
                        {

                            await _question.AddAsync(question);
                            Console.WriteLine($"Added question: Cau {question.Number}");
                            Console.WriteLine("---------------------------------------");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Failed to add question !");
                            Console.WriteLine("Error: " + ex.Message);
                            Console.WriteLine("-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-x-");
                        }
                    }

                }
            }
        }
        
    }


}
