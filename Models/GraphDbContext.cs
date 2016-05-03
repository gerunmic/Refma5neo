using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Refma5neo.Models
{
    public class GraphDbContext : IDisposable
    {

        private GraphClient client;
        public GraphDbContext()
        {
            client = new GraphClient(new Uri("http://localhost:7474/db/data"));
            client.Connect();
        }

        public void Dispose()
        {
            client.Dispose();
        }

        public void addWebArticle(WebArticle article) {

           
        }



        public WebArticle getWebArticle(int id)
        {
            var res = client.Cypher.Match("(l:Language)-[:has_article]->(w:WebArticle)")
                .Where("ID(w) = {wid}")
                .WithParam("wid", id)
                .Return(() => new WebArticle() {
                    id = Return.As<long>("ID(w)"),
                    plaintext = Return.As<string>("w.plaintext"),
                    langcode = Return.As<string>("l.code"),
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
            var res = client.Cypher.Match("(u:User)-[:reads_article]->(w:WebArticle)<-[:has_article]-(l:Language)")
                .Where("u.Id = {userid} and w.title <> '' and w.url <> '' and l.code = u.TargetLangCode")
                .WithParam("userid", userid)
                .Return(w => w.As<WebArticle>())
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