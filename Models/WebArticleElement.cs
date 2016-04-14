

namespace Refma5neo.Models
{
    public class WebArticleElement
    {

        public int WebArticleId { get; set; }

        public virtual WebArticle WebArticle { get; set; }


        public int LangElementId { get; set; }

        public virtual LangElement LangElement { get; set; }
    }
}
