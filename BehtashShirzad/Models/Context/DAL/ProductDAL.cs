using BehtashShirzad.Models.DbModels;
using BehtashShirzad.Model.ApiModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Frozen;
using Logger;
using SharedObjects;
using System.Reflection;

namespace BehtashShirzad.Model.Context.DAL
{
    public static class ProductDAL
    {

        public static IEnumerable<Product> GetProducts() {

            using (var cn = new DbCommiter())
            {
                try
                {

                    return cn.Products.Where(_=>_.IsVisible).ToFrozenSet();

                }
                catch (Exception ex)
                {
                    Log.CreateLog(new() { LogType = Constants.LogType.Error, Description = ex.Message, Extra = ex.InnerException?.Message });

                    return Enumerable.Empty<Product>();
                }
            }

        }

        public static async Task<IEnumerable<Product>> GetTopTenProducts()
        {

            using (var cn = new DbCommiter())
            {
                try
                {


                    var val = await cn.Products.Where(_ => _.IsVisible == true).OrderByDescending(_=>_.Id).ToListAsync();
                return val.TakeLast(10);
                }
                catch (Exception ex)
                {
                    Log.CreateLog(new() { LogType = Constants.LogType.Error, Description = ex.Message, Extra = ex.InnerException?.Message });

                    return Enumerable.Empty<Product>();
                }
            }

        }

        public static async Task<IEnumerable<Product>> GetTopThreeProducts()
        {

            using (var cn = new DbCommiter())
            {
                try
                {


                    var val = await cn.Products.Where(_ => _.IsVisible == true).OrderByDescending(_ => _.Id).ToListAsync();
                    return val.TakeLast(3);
                }
                catch (Exception ex)
                {
                    Log.CreateLog(new() { LogType = Constants.LogType.Error, Description = ex.Message, Extra = ex.InnerException?.Message });

                    return Enumerable.Empty<Product>();
                }
            }

        }

        public static async Task<bool> CreateProduct(ProductDto p) {
            try
            {
                var product = new Product() { Description=p.Description,Name=p.Name,Price=p.Price,image=p.Image};
                
            if (_IsExist(product)) { return false; }
            using(var cn = new DbCommiter())
            {
              await  cn.Products.AddAsync(product);
               await cn.SaveChangesAsync();
                return true;
            }
            }
            catch (Exception ex)
            {
                Log.CreateLog(new() { LogType = Constants.LogType.Error,  Description = ex.Message, Extra = ex.InnerException?.Message });
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
            catch (Exception ex)
            {
                Log.CreateLog(new() { LogType = Constants.LogType.Error,  Description = ex.Message, Extra = ex.InnerException?.Message });
                return false;
            }
        }

    }
}
