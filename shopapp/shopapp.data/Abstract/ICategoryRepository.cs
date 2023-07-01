using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.entity;

namespace shopapp.data.Abstract
{
    public interface ICategoryRepository:IRepository<Category>
    {
        List<Product> GetPopularCategories();
        Category GetByIdWithProducts(int categorysId);
        void DeleteFromCategory(int productId, int categoryId);
    

    }
}