using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using shopapp.data.Abstract;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public class EfCoreCategoryRepository : EfCoreGenericRepository<Category, ShopContext>, ICategoryRepository
    {
        public void DeleteFromCategory(int productId, int categoryId)
        {
            using(var context = new ShopContext())
            {
                var cmd = "delete from productcategory where ProductId=@p0 and CategoryId=@p1";
                context.Database.ExecuteSqlRaw(cmd, productId, categoryId);
            }
        }

        public Category GetByIdWithProducts(int categorysId)
        {
            using(var context = new ShopContext())
            {
                return context.Categories
                    .Where(i=>i.CategoryId == categorysId)
                    .Include(i=>i.ProductCategories)
                    .ThenInclude(i=>i.Product)
                    .FirstOrDefault();
            }
        }

        public List<Product> GetPopularCategories()
        {
            throw new NotImplementedException();
        }
    }
}