using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using shopapp.entity;

namespace shopapp.data.Concrete.EfCore
{
    public static class SeedDatabase
    {
        public static void Seed()
        {
            var context = new ShopContext();
            if(context.Database.GetPendingMigrations().Count()==0)
            {
                if(context.Categories.Count()==0)
                {
                    context.Categories.AddRange(Categories);
                }
                if(context.Products.Count()==0)
                {
                    context.Products.AddRange(Products);
                    context.AddRange(ProductCategories);
                }
            } 
            context.SaveChanges() ;          
        }
        private static Category[] Categories= {
            new Category(){Name="Telefon", Url="telefon"},
            new Category(){Name="Bilgisayar", Url="bilgisayar"},
            new Category(){Name="Elektronik", Url="elektronik"}, 
            new Category(){Name="Beyaz Eşya", Url="beyaz-esya"}, 
        };
        private static Product[] Products= {
            new Product(){Name="Samsung s6", Url="samsung-s6", Price=12000, Description="İyi telefon",ImageUrl="1.jpg", IsApproved=true, IsHome=true, CategoryId=1 },
            new Product(){Name="Samsung s7", Url="samsung-s7", Price=13000, Description="İyi telefon",ImageUrl="1.jpg", IsApproved=true, IsHome=true, CategoryId=1 },
            new Product(){Name="Samsung s8", Url="samsung-s8", Price=14000, Description="İyi telefon",ImageUrl="1.jpg", IsApproved=true, IsHome=true, CategoryId=1 },
            new Product(){Name="Samsung galaxy s21", Url="samsung-s21", Price=20000, Description="Çok iyi telefon",ImageUrl="2.jpg", IsApproved=true, IsHome=true, CategoryId=2 },
            new Product(){Name="Samsung galaxy s22", Url="samsung-s22", Price=21000, Description="Çok iyi telefon",ImageUrl="2.jpg", IsApproved=true, IsHome=true, CategoryId=2 },
        };
        private static ProductCategory[] ProductCategories={
            new ProductCategory(){Product=Products[0], Category=Categories[0]},
            new ProductCategory(){Product=Products[0], Category=Categories[2]},
            new ProductCategory(){Product=Products[1], Category=Categories[0]},
            new ProductCategory(){Product=Products[1], Category=Categories[2]},
            new ProductCategory(){Product=Products[2], Category=Categories[0]},
            new ProductCategory(){Product=Products[2], Category=Categories[2]},
            new ProductCategory(){Product=Products[3], Category=Categories[0]},
            new ProductCategory(){Product=Products[3], Category=Categories[2]},
            new ProductCategory(){Product=Products[4], Category=Categories[0]},
            new ProductCategory(){Product=Products[4], Category=Categories[2]}
        };
    }
}