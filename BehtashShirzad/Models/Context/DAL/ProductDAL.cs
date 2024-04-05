using ElliotStore.Model.ApiModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Frozen;

namespace ElliotStore.Model.Context.DAL
{
    public static class ProductDAL
    {
        
        public static  IEnumerable<Product> GetProducts() {
             
            using (var cn = new DbCommiter())
            { 
                try
                {

                   return cn.Products.ToFrozenSet();
                   
                }
                catch (Exception)
                {

                    return Enumerable.Empty<Product>();
                }
            }
             
        }
        public static async Task<bool> CreateProduct(ProductDto p) {
            try
            {
                var product = new Product() { Description=p.Description,Name=p.Name,Price=p.Price};
                
            if (_IsExist(product)) { return false; }
            using(var cn = new DbCommiter())
            {
              await  cn.Products.AddAsync(product);
               await cn.SaveChangesAsync();
                return true;
            }
            }
            catch (Exception)
            {

                    return false;
            }
        }

        static bool _IsExist(Product p ) {
        using(var cn = new DbCommiter())
                return  cn.Products.AnyAsync(_=>_.Name==p.Name).Result;
                
        
        }

        static public bool DeleteProduct(int id) {

            try
            {
                using (var db = new DbCommiter())
                { db.Products.Where(_=>_.Id==id).ExecuteDelete();
                    db.SaveChanges();
                    return true;
                        }
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
