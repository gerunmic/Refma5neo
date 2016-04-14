using System;


namespace Refma5neo.Models
{
    public class Lang
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public string ImageSmall { get; set; }
        public string ImageBig { get; set; }

        public override string ToString()
        {
            return String.Format("{0} - {1}", this.Code, this.Name);
        }
    }
}