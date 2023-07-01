using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreProductRepository : EfCoreGenericRepository<Product, ShopContext>, IProductRepository
    {
        public int GetCountByCategory(string category)
        {
            using(var context = new ShopContext())
            {
                var products = context
                                .Products
                                .Where(i=>i.IsApproved)
                                .AsQueryable(); 
                
                if(!string.IsNullOrEmpty(category)){
                    products = products
                                .Include(i=>i.ProductCategories)
                                .ThenInclude(i=>i.Category)
                                .Where(i => i.ProductCategories.Any(a=>a.Category.Url == category));
                }
            
                return products.Count();
            }
        }

        public List<Product> GetHomePageProducts()
        {
            using (var context = new ShopContext())
            {
                return context.Products.Where(i=>i.IsApproved && i.IsHome).ToList();
            }
        }

        public List<Product> GetSearchResult(string search)
        {
           using(var context = new ShopContext())
            {
                var products = context
                                .Products
                                .Where(i=>i.IsApproved && (i.Name.ToLower().Contains(search.ToLower()) || i.Description.ToLower().Contains(search.ToLower())))
                                .AsQueryable(); 
             
                return products.ToList();
            }
        }

        public Product GetProductDetails(string url)
        {
            using(var context = new ShopContext())
            {
                return context.Products
                                .Where(i=>i.Url==url)
                                .Include(i=>i.ProductCategories)
                                .ThenInclude(i=>i.Category)
                                .FirstOrDefault();
            }
        }

        public List<Product> GetProductsByCategory(string name, int page, int pageSize)
        {
            using(var context = new ShopContext())
            {
                var products = context
                                .Products
                                .Where(i=>i.IsApproved)
                                .AsQueryable();  
                
                if(!string.IsNullOrEmpty(name)){
                    products = products
                                .Include(i=>i.ProductCategories)
                                .ThenInclude(i=>i.Category)
                                .Where(i => i.ProductCategories.Any(a=>a.Category.Url == name));
                }
            
                return products.Skip((page-1)*pageSize).Take(pageSize).ToList();
            }
        }

        public Product GetByIdWithCategories(int id)
        {
            using(var context = new ShopContext())
            {
                return context.Products
                                .Where(i=>i.ProductId == id)
                                .Include(i=>i.ProductCategories)
                                .ThenInclude(i=>i.Category)
                                .FirstOrDefault();
            }
        }

        public void Update(Product model, int[] categoryId)
        {
            using(var context =new ShopContext())
            {
                var produt = context.Products
                                .Include(i=>i.ProductCategories)
                                .FirstOrDefault(i=>i.ProductId == model.ProductId);
            
                if(produt!=null) 
                {
                    produt.Name = model.Name;
                    produt.Price = model.Price;
                    produt.Description = model.Description;
                    produt.Url = model.Url;
                    produt.ImageUrl = model.ImageUrl;
                    produt.IsApproved = model.IsApproved;
                    produt.IsHome = model.IsHome;

                    produt.ProductCategories = categoryId.Select(catid => new ProductCategory()
                    {
                        ProductId = model.ProductId,
                        CategoryId = catid
                    }).ToList();
                   
                    context.SaveChanges();
                }
            }
        }
    }
}