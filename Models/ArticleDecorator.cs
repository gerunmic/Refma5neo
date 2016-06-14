using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Refma5neo.Models
{
    public class ArticleDecorator
    {
        private Dictionary<String, Knowledge> dic = new Dictionary<string, Knowledge>(StringComparer.OrdinalIgnoreCase);

        public Dictionary<String, Knowledge> Dic
        {
            get { return dic; }
            set { dic = value; }
        }

        private WebArticle article = null;
        private string userid;

        public ArticleDecorator(WebArticle article, string userid, Boolean readFromDatabase = true)
        {
            this.article = article;
            this.userid = userid;

            if (readFromDatabase)
            {
                ReadArticleElementsFromDatabase(article);
            }

        }

        private void ReadArticleElementsFromDatabase(WebArticle article)
        {
            using (GraphDbContext db = new GraphDbContext())
            {

                var allElements = db.getWebArticleUserElements(article, userid);
       


                foreach (var e in allElements.ToList<ViewArticleElement>())
                {
                    try
                    {
                        dic.Add(e.Value, e.Knowledge);
                    }
                    catch (ArgumentException x)
                    {

                    }
                }

            }
        }

        public static string[] ExtractStringElements(string source)
        {
            string[] stringElements = Regex.Split(source, SpecialCharactersClass.getSplitPattern());
            return stringElements.Where(s => s != String.Empty).ToArray();
        }

        public List<ViewArticleElement> GetAllViewElements()
        {
            // returns all elements of the article with additional information for the view (such as if its a word, a special character, known etc.) the order must be exactly as it appears in the source article
            List<ViewArticleElement> viewElements = new List<ViewArticleElement>();
            string[] allStrings = ExtractStringElements(article.plaintext);
            foreach (var str in allStrings)
            {
                if (dic.ContainsKey(str))
                {
                    Knowledge k = Knowledge.Unknown;
                
                    dic.TryGetValue(str, out k);

                    ViewArticleElement ve = new ViewArticleElement() {  Value = str, Knowledge = k };
   

                    viewElements.Add(ve);
                }
                else
                {

                    bool isNotAWord = Regex.Match(str, SpecialCharactersClass.getNonLetterPattern()).Success;
                    viewElements.Add(new ViewArticleElement() { IsNotAWord = isNotAWord, Value = str });
                }
            }
            return viewElements;
        }
    }
}
