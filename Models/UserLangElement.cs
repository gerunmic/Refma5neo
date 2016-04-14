using Neo4j.AspNet.Identity;


namespace Refma5neo.Models
{
    public class UserLangElement
    {
  
        public string UserId {get; set;}
        public virtual ApplicationUser User {get;set;}

  
        public int LangElementId { get; set; }

        public virtual LangElement LangElement { get; set; }

        public Knowledge Knowledge { get; set; }
    }
}
