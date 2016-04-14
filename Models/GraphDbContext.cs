using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Refma5neo.Models
{
    public class GraphDbContext: IDisposable
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

    }
}