using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Collections.Generic;

namespace Refma5neo.Models
{
    public class GraphDbContext : IDisposable
    {

        private GraphClient client;
        public GraphDbContext()
        {
            client = new Neo4jClient.GraphClient(new Uri("http://refmadb.sb11.stations.graphenedb.com:24789/db/data/"), "refmadb", "QWacePxs3Xxof1QWszF2");
            client.Connect();
        }

        public void Dispose()
        {
            client.Dispose();
        }

        public List<Lang> getLanguages()
        {
            
            var cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
             List< Lang > langs = new List<Lang>();
            foreach (var item in cultures)
            {
                if (item.TwoLetterISOLanguageName == null) continue;
                langs.Add(new Lang() { Code = item.TwoLetterISOLanguageName, Name = item.DisplayName });
            }
            return langs;


        }

        public void addWebArticle(WebArticle article, string userid) {
            client.Cypher
                 .Merge("(w:WebArticle:" + article.langcode + " { url: {url}, id: {id}, title: {title}, plaintext: {plaintext} })")
                 .WithParams(new
                 {
                     url = article.url,
                     id = article.id,
                     title = article.title,
                     plaintext = article.plaintext
                 })
                 .Merge("(u:User {Id: {userid}})")
                 .WithParam("userid", userid)
                 .CreateUnique("(u)-[:reads]->(w)")
                 .ExecuteWithoutResults();
        }

        public void addWebArticleWords(WebArticle article)
        {
            article.tokens = article.tokens.Distinct().ToArray<string>();
            foreach (var e in article.tokens)
            {
                client.Cypher.Merge("(e:Word:"+ article.langcode +" {value: {value}})")
                .WithParam("value", e)
                .ExecuteWithoutResults();
            }

        }


        public WebArticle getWebArticle(int id)
        {
            var res = client.Cypher.Match("(w:WebArticle)")
                .Where("ID(w) = {wid}")
                .WithParam("wid", id)
                .Return(() => new WebArticle() {
                    id = Return.As<long>("ID(w)"),
                    plaintext = Return.As<string>("w.plaintext"),
                    langcode = Return.As<string>("Labels(w)[1]"),
                    title = Return.As<String>("w.title"),
                    url = Return.As<String>("w.url")
                })
                .Results;
            return res.FirstOrDefault();
        }

        public List<ViewArticleElement> getWebArticleUserElements(WebArticle article, string userid)
        {
           var res = client.Cypher.Match("(w: WebArticle) -[:uses]->(e: Word)")
                .Where("ID(w) = {wid}").WithParam("wid", article.id)
                .OptionalMatch("(u: User) -[r: rates]->(e)")
                .Where("u.Id = {userid}")
                .WithParam("userid", userid)
                .Return(() => new ViewArticleElement()
                {
                    Value = Return.As<String>("e.value"),
                    Knowledge = Return.As<Knowledge>("r.rating")
                }).Results;

            return res.ToList();
        }



        public List<WebArticle> getWebArticles(string userid)
        {
            var res = client.Cypher.Match("(u:User)-[:reads]->(w:WebArticle)")
                .Where("u.Id = {userid}")
                .WithParam("userid", userid)
                .Return(() => new WebArticle()
                {
                    id = Return.As<long>("ID(w)"),
                    plaintext = Return.As<string>("w.plaintext"),
                    langcode = Return.As<string>("Labels(w)[1]"),
                    title = Return.As<String>("w.title"),
                    url = Return.As<String>("w.url")
                })
                .Results;
            return res.ToList();


                    

        }

        internal void updateRating(string value, string langcode, int knowledge, string currentUserId)
        {
            client.Cypher.Match("(u: User),(w:" + langcode + ")")
                .Where("u.Id = {userid}").WithParam("userid", currentUserId)
                .AndWhere("w.value = { word}").WithParam("word", value)
                .Merge("(u) -[r: rates]->(w)")
                .Set("r.rating = {newRating}").WithParam("newRating", knowledge)
                .ExecuteWithoutResultsAsync();
        }

        public List<ViewArticleElement> getWords(string langcode, string userid) {

            var res = client.Cypher.Match("(u:User)-[r:rates]->(w:Word)")
                .Where("u.Id={userid}").WithParam("userid", userid)
                //.AndWhere("Labels(w) = {langLabel}").WithParam("langLabel", langcode)
                .Return(() => new ViewArticleElement() {
                       Value = Return.As<String>("w.value"),
                       Knowledge = Return.As<Knowledge>("r.rating")
                })
                .Limit(100)
                .Results;

            return res.ToList();
            }
    }
}