
using System.ComponentModel.DataAnnotations;
using System;



namespace Refma5neo.Models
{
    public class LangElement
    {
        
        public int ID { get; set; }

        public int LangId { get; set; }
  
        public virtual Lang Lang { get; set; }
        [Required, MinLength(1), MaxLength(450)]
   
        public string Value { get; set; }


        public override bool Equals(object obj)
        {
            if (obj is LangElement)
            {
                LangElement compareObj = (LangElement)obj;
                return this.Value.Equals(compareObj.Value, StringComparison.OrdinalIgnoreCase) &&
                       this.LangId == compareObj.LangId;
                
            }
            return false;
        }

    }

}
