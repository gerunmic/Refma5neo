using System;
using Neo4j.AspNet.Identity;

namespace Refma5neo.Models { 
    public class WebArticle
    {


        public long id  { get; set; }
    public String title { get; set; }
        public String url { get; set; }
        public String plaintext { get; set; }

        public String langcode { get; set; }

        public string[] tokens { get; set; }
    }


}