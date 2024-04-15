using BehtashShirzad.Models.DbModels;
using BehtashShirzad.Model.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BehtashShirzad.Models.Context.DAL
{
    public class ProductCategoryDAL  
    {

        public static IEnumerable<ProductCategory> GetProductCategory()
        {
            try
            {
                using (var db = new DbCommiter())
                {
                    return db.productCategories;

                }
                
            }
            catch (Exception ex)
            {

                return Enumerable.Empty<ProductCategory>();
            }

        }


        public static async Task<bool>  CreateProductCategory(ProductCategory productct)
        {
            try
            {
                if (_IsExist(productct)) { return false; }
                using (var db = new DbCommiter())
                {
                     await db.productCategories.AddAsync(productct);
                    await db.SaveChangesAsync();
                }
                return true;

            }
            catch (Exception ex)
            {

                return false;
            }

        }

        static bool _IsExist(ProductCategory p)
        {
            using (var cn = new DbCommiter())
                return cn.Products.AnyAsync(_ => _.Name == p.Name).Result;


        }

        public static   bool DeleteProductCategory(int id)
        {

            try
            {
                using (var db = new DbCommiter())
                {
                    db.productCategories.Where(_ => _.Id == id).ExecuteDelete();
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
