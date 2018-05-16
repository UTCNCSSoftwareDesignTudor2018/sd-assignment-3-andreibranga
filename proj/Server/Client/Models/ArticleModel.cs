using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Models
{
    public class ArticleModel
    {
        public int ArticleId { get; set; }
        public System.DateTime Date { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Author { get; set; }
        public string Body { get; set; }
    }
}
