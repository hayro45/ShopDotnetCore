using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using shopapp.entity;

namespace shopapp.WebUI.Models
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur")]
        [StringLength(100, MinimumLength =5, ErrorMessage = "Kategori için 5-100 arasında değer giriniz.")]
        public string Name { get; set; }
        
        [Required(ErrorMessage="Url zorunludur.")]
        [StringLength(100,MinimumLength=5,ErrorMessage="Url için 5-100 arasında değer giriniz.")]    
        public string Url { get; set; }
        
        public List<Product> Products { get; set; }
    }
}