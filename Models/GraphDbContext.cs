using Neo4jClient;
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

        public List<LangElement> getWebArticleElements(WebArticle article)
        {
            // todo: get all elements from an article
            return null;
        }

        public List<UserLangElement> getWebArticleUserElements(WebArticle article)
        {
            // todo: get all elements from an article from an user
            return null;
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


    }
}