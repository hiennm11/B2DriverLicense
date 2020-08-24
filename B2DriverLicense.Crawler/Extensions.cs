using B2DriverLicense.AppCrawler.Models;
using B2DriverLicense.Core.Entities;
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
    public static class Extensions
    {
        #region NodeExtensions
        public static bool HasHint(this HtmlNode node)
        {
            return node.InnerText.Contains("Gợi ý:");
        }

        public static List<HtmlNode> TakeHintNode(this List<HtmlNode> nodes)
        {
            return nodes.Where(x => x.InnerText.Contains("Gợi ý")).ToList();
        }

        public static List<HtmlNode> TakeCorrectAnswerNode(this List<HtmlNode> nodes)
        {
            return nodes.Where(x => x.InnerText.Contains("Đáp án")).ToList();
        }

        public static HtmlNode SearchImageNodeByQuestionNumber(this List<HtmlNode> nodes, int number)
        {
            return nodes.FirstOrDefault(x => x.GetQuestionNumberFromImage() == number);
        }

        public static int GetQuestionNumberFromImage(this HtmlNode node)
        {
            var src = node.Attributes["src"].Value;
            var name = Path.GetFileNameWithoutExtension(src);
            var number = new string(name.Where(Char.IsDigit).ToArray());
            return Convert.ToInt32(number);
        }

        public static bool CheckTableNode(this HtmlNode node)
        {
            var el = node.Descendants("tr").ToList();
            return el.Count() > 0;
        }

        public static HtmlNode GetImageNodeFromTable(this HtmlNode node)
        {
            var imgNode = node.QuerySelector("img");
            return imgNode;
        }

        public static List<HtmlNode> GetListQuestionNodeFromStrongNode(this List<HtmlNode> nodes)
        {
            List<HtmlNode> questionNodes = new List<HtmlNode>();
            foreach (var item in nodes)
            {
                var splitedString = item.InnerText.Split(":");
                var id = new string(splitedString[0].Where(Char.IsDigit).ToArray());
                if (splitedString.Count() > 1)
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        if (splitedString[0].Contains($"Câu {id}")) questionNodes.Add(item);
                    }
                }
            }

            return questionNodes;
        }

        public static List<Answer> GetListAnswerFromTable(this HtmlNode node)
        {
            var answers = new List<Answer>();

            var trNodes = node.QuerySelectorAll("tr");
            foreach (var (item, index) in trNodes.WithIndex())
            {
                if(index == 0)
                {
                    var ansNode = item.QuerySelector("td:nth-child(2)");
                    var text = ansNode.InnerText.Split(".");
                    var answer = new Answer() { Key = index + 1, Content = text[1] };
                    answers.Add(answer);
                }
                else
                {
                    var ansNode = item.QuerySelector("td");
                    var text = ansNode.InnerText.Split(".");
                    var answer = new Answer() { Key = index + 1, Content = text[1] };
                    answers.Add(answer);
                }
            }

            return answers;
        }

        public static List<Answer> GetListAnswerFromNode(this HtmlNode node)
        {
            var answers = new List<Answer>();

            foreach (var (ans, ansIndex) in node.QuerySelectorAll("li").WithIndex())
            {
                var answer = new Answer { Key = ansIndex + 1, Content = ans.InnerText };
                answers.Add(answer);
            }

            return answers;
        }

        public static List<Answer> GetListAnswerFromNodes(this List<HtmlNode> nodes)
        {
            var answers = new List<Answer>();

            foreach (var item in nodes)
            {
                var splited = item.InnerText.Split(".");
                var key = Convert.ToInt32(splited[0]);

                var answer = new Answer { Key = key, Content = splited[1] };
                answers.Add(answer);
            }

            return answers;
        }

        public static List<Answer> GetListAnswer(this HtmlNode node)
        {
            if (node.QuerySelectorAll("li").Count() > 0) return node.GetListAnswerFromNode();
            
            return node.GetListAnswerFromTable();
        }

        public static Dictionary<int, List<HtmlNode>> GetListAnswerNodesChapter8(this List<HtmlNode> nodes)
        {
            var result = new Dictionary<int, List<HtmlNode>>();
            int resultKey = 0;

            foreach (var item in nodes)
            {
                var splited = item.InnerText.Split(".");
                try
                {
                    var key = Convert.ToInt32(splited[0]);
                    if (key == 1)
                    {
                        var list = new List<HtmlNode>();
                        list.Add(item);
                        result.Add(resultKey, list);
                        resultKey++;
                    }
                    else
                    {
                        var list = result[resultKey - 1];
                        list.Add(item);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to add answer to list ");
                }
            }

            return result;
        }

        public static int GetCorrectAnswerFromNode(this HtmlNode node)
        {
            var ans = new string(node.InnerText.Where(Char.IsDigit).ToArray());
            if (string.IsNullOrEmpty(ans)) return 0;
            return Convert.ToInt32(ans);
        }

        public static ImageVM GenerateImageFromNode(this HtmlNode node)
        {
            var url = node.Attributes["src"].Value;
            var folderPath = Directory.GetCurrentDirectory() + "/images/";
            var filePath = Extensions.DownloadImg(url, folderPath);

            if (!string.IsNullOrEmpty(filePath))
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                var br = new BinaryReader(fs);

                var image = new ImageVM
                {
                    ImageData = br.ReadBytes((int)fs.Length),
                    ImageTitle = fileName
                };

                br.Close();
                fs.Close();
                return image;
            }

            return null;
        }
        #endregion

        #region QuestionExtensions

        public static Question AddCorrectAnswer(this Question question, HtmlNode node)
        {
            var ans = new string(node.InnerText.Where(Char.IsDigit).ToArray());
            question.CorrectAnswer = Convert.ToInt32(ans);

            return question;
        }

        public static Question AddImage(this Question question, HtmlNode node)
        {
            var url = node.Attributes["src"].Value;
            var folderPath = Directory.GetCurrentDirectory() + "/images/";
            var filePath = Extensions.DownloadImg(url, folderPath);

            if (!string.IsNullOrEmpty(filePath))
            {
                try
                {
                    var fileName = Path.GetFileNameWithoutExtension(filePath);
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

            return question;
        }
        #endregion

        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
                => self?.Select((item, index) => (item, index)) ?? new List<(T, int)>();
        
        public static string DownloadImg(string url, string path)
        {
            using (var client = new WebClient())
            {
                try
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    var fileName = Path.GetFileName(url);
                    var filePath = path + fileName;
                    client.DownloadFile(url, filePath);
                    Console.WriteLine($"Image: {fileName} has been downloaded.");
                    return filePath;
                }
                catch (Exception)
                {
                    Console.WriteLine("Failed to download image.");
                    return null;
                }
            }
        }

        
    }
}
