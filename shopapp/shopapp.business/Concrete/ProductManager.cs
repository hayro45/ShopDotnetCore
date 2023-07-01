using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using shopapp.business.Abstract;
using shopapp.data.Abstract;
using shopapp.data.Concrete.EfCore;
using shopapp.entity;

namespace shopapp.business.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductRepository _productRepository;

        public string ErrorMessage { get; set ; }

        public ProductManager(IProductRepository productRepository)
        {
            this._productRepository=productRepository;
        }
        public bool Create(Product entity)
        {
            if(Validation(entity)){
                _productRepository.Create(entity); 
                return true;
            }
            return false;

        }

        public void Delete(Product entity)
        {
            _productRepository.Delete(entity);
        }

        public List<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public Product GetById(int id)
        {
            return _productRepository.GetById(id);
        }

        public List<Product> GetProductsByCategory(string name, int page, int pageSize)
        {
            return _productRepository.GetProductsByCategory(name, page, pageSize);
        }

        public Product GetProductDetails(string url)
        {
            
            return _productRepository.GetProductDetails(url);
        }

        public void Update(Product entity)
        {
            _productRepository.Update(entity);
        }

        public int GetCountByCategory(string category)
        {
            return _productRepository.GetCountByCategory(category);
        }

        public List<Product> GetHomePageProducts()
        {
            return _productRepository.GetHomePageProducts();
        }

        public List<Product> GetSearchResult(string search)
        {
            return _productRepository.GetSearchResult(search);
        }

        public Product GetByIdWithCategories(int id)
        {
            return _productRepository.GetByIdWithCategories(id);
        }

        public bool Update(Product entity, int[] categoryId)
        {
            if(Validation(entity))
            {
                if(categoryId.Length==0)
                {
                    ErrorMessage += "Ürün için en bir kategori seçmelisiniz.";
                    return false;
                }
                _productRepository.Update(entity, categoryId);  
                return true;
            }
            return false;
        }

        public bool Validation(Product entity)
        {
            var isValid = true;
            if(string.IsNullOrEmpty(entity.Name))
            {
                ErrorMessage += "ürün ismi girmelisiniz.\n"; 
                isValid=false;
            }
            if(string.IsNullOrEmpty(entity.Name))
            {
                ErrorMessage += "ürün fiyatı girmelisiniz.\n";
                isValid=false; 
            }
            if(entity.Price<0)
            {
                ErrorMessage += "ürün fiyatı negatif olamaz";
                isValid=false;
            }
            return isValid;
        }
    }
}