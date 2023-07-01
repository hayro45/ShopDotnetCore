using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shopapp.entity
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public double Price{ get; set; }
        public string Description { get; set; }
        public string ImageUrl  { get; set; }
        public bool IsApproved { get; set; }
        public bool IsHome { get; set; }
        public int CategoryId { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
    }
}