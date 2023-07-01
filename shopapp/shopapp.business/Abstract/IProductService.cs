using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.entity;

namespace shopapp.business.Abstract
{
    public interface IProductService: IValidator<Product>
    {
        Product GetById(int id);
        Product GetByIdWithCategories(int id);
        Product GetProductDetails(string url);
        List <Product> GetProductsByCategory(string name, int page, int pageSize);
        List<Product> GetAll();
        List<Product> GetHomePageProducts();
        List<Product> GetSearchResult(string search);
        bool Create(Product entity);
        void Update(Product entity);
        void Delete(Product entity);
        int GetCountByCategory(string category);
        bool Update(Product entity, int[] categoryId);
    }
}