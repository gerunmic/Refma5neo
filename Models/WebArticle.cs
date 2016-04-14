using System;
using Neo4j.AspNet.Identity;

namespace Refma5neo.Models { 
    public class WebArticle
    {
        public int ID { get; set; }
        public int LangId { get; set; }
        public virtual Lang Lang { get; set; }
        public String Title { get; set; }
        public String URL { get; set; }
        public String PlainText { get; set; }
        public string UserId { get; set; }
        public double? PercentageKnown { get; set; }
 
        public virtual ApplicationUser User { get; set; }
    }


}