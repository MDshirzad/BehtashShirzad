using BehtashShirzad.Models.DbModels;
using BehtashShirzad.Model.ApiModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Frozen;
using Logger;
using SharedObjects;
using System.Reflection;
using BehtashShirzad.Models.Context.DAL;

namespace BehtashShirzad.Model.Context.DAL
{
    public static class ProductDAL
    {

        public static IEnumerable<Product> GetProducts() {

            using (var cn = new DbCommiter())
            {
                try
                {

                    return cn.Products.Where(_=>_.IsVisible).Include(_=>_.Category).ToFrozenSet();

                }
                catch (Exception ex)
                {
                    Log.CreateLog(new() { LogType = Constants.LogType.Error, Description = ex.Message, Extra = ex.InnerException?.Message });

                    return Enumerable.Empty<Product>();
                }
            }

        }
        public static IEnumerable<Product> GetProductsAdmin()
        {

            using (var cn = new DbCommiter())
            {
                try
                {

                    return cn.Products.Include(_ => _.Category).ToFrozenSet();

                }
                catch (Exception ex)
                {
                    Log.CreateLog(new() { LogType = Constants.LogType.Error, Description = ex.Message, Extra = ex.InnerException?.Message });

                    return Enumerable.Empty<Product>();
                }
            }

        }
        public static IEnumerable<Product> GetCourses()
        {

            using (var cn = new DbCommiter())
            {
                try
                {

                    return cn.Products.Where(_ => _.IsVisible).Include(_=>_.Category).Where(_=> _.Category.Name.ToLower()=="course").ToFrozenSet();

                }
                catch (Exception ex)
                {
                    Log.CreateLog(new() { LogType = Constants.LogType.Error, Description = ex.Message, Extra = ex.InnerException?.Message });

                    return Enumerable.Empty<Product>();
                }
            }

        }
        public static IEnumerable<Product> GetSources()
        {

            using (var cn = new DbCommiter())
            {
                try
                {

                    return cn.Products.Where(_ => _.IsVisible).Include(_ => _.Category).Where(_ => _.Category.Name.ToLower() == "source").ToFrozenSet();

                }
                catch (Exception ex)
                {
                    Log.CreateLog(new() { LogType = Constants.LogType.Error, Description = ex.Message, Extra = ex.InnerException?.Message });

                    return Enumerable.Empty<Product>();
                }
            }

        }
        public static IEnumerable<Product> GetTopThreeSources()
        {

            using (var cn = new DbCommiter())
            {
                try
                {

                    var res = cn.Products.Where(_ => _.IsVisible).Include(_ => _.Category).Where(_ => _.Category.Name.ToLower() == "source").OrderByDescending(_ => _.Id).ToList();
                    
                    if (res.Count < 3)
                    {
                        return res; // Return the entire list if there are less than 3 items.
                    }
                    else
                    {
                        return res.TakeLast(3); // Otherwise, return the last 3 items.
}
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
               
                    if (val.Count < 3)
                    {
                        return val; // Return the entire list if there are less than 3 items.
                    }
                    else
                    {
                        return val.TakeLast(10);
                    }
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
                  

                    if (val.Count < 3)
                    {
                        return val; // Return the entire list if there are less than 3 items.
                    }
                    else
                    {
                        return val.TakeLast(3); // Otherwise, return the last 3 items.
                    }
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
                var category = ProductCategoryDAL.GetProductCategoryByName(p.Category);
                var product = new Product() { Description=p.Description,Name=p.Name,Price=p.Price,image=p.Image,CategoryId= category.Id,IsVisible=p.IsVisible};
                
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


        public static async Task<object> GetProductById(int Id)
        {

            using (var cn = new DbCommiter())
            {
                try
                {
                    var res =   await cn.Products
                        .Where(_ => _.IsVisible==true && _.Id == Id)
                        .Select(_ => new {
                            Name = _.Name,
                            Price = _.Price,
                            Id=_.Id,
                            Image = _.image,
                            CategoryName = _.Category.Name
                        })
                        .FirstOrDefaultAsync();
                    return res;

                }
                catch (Exception ex)
                {
                    Log.CreateLog(new() { LogType = Constants.LogType.Error, Description = ex.Message, Extra = ex.InnerException?.Message });

                    return null;
                }
            }

        }


        public static async Task<bool> ChangeVisibility(string productName,bool Isvisible)
        {

            using (var cn = new DbCommiter())
            {
                try
                {
                    var res = await cn.Products
                        .Where(_ => _.Name == productName)
                        .FirstOrDefaultAsync();
                    res.IsVisible = Isvisible;
                    await cn.SaveChangesAsync();
                    return true;

                }
                catch (Exception ex)
                {
                    Log.CreateLog(new() { LogType = Constants.LogType.Error, Description = ex.Message, Extra = ex.InnerException?.Message });

                    return false;
                }
            }

        }

        public static async Task<Product> GetProductByName(string name)
        {

            using (var cn = new DbCommiter())
            {
                try
                {
                    var res = await cn.Products
                        .Where(_ => _.IsVisible == true && _.Name == name)
                        .FirstOrDefaultAsync();
                    return res;

                }
                catch (Exception ex)
                {
                    Log.CreateLog(new() { LogType = Constants.LogType.Error, Description = ex.Message, Extra = ex.InnerException?.Message });

                    return null;
                }
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
