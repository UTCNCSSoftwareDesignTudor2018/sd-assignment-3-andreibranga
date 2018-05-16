using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Server.Models
{
    public class ArticleModel
    {
        [JsonProperty("ArticleId")]
        public int ArticleId { get; set; }
        [JsonProperty("Date")]
        public System.DateTime Date { get; set; }
        [JsonProperty("Title")]
        public string Title { get; set; }
        [JsonProperty("Abstract")]
        public string Abstract { get; set; }
        [JsonProperty("Author")]
        public string Author { get; set; }
        [JsonProperty("Body")]
        public string Body { get; set; }
    }
}
