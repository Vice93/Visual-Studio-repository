using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolrTestApi.Models
{
    public class Product
    {
        public string id { get; set; }
        public string name { get; set; }
        public string manu { get; set; }
        public string manu_id_s { get; set; }
        public List<String> cat { get; set; }
        public List<String> features { get; set; }
        public float weight { get; set; }
        public float price { get; set; }
        public string price_c { get; set; }
        public int popularity { get; set; }
        public Boolean inStock { get; set; }
        public string store { get; set; }
        public string manufacturedate_dt { get; set; }
    }
}
