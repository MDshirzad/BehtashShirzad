using BehtashShirzad.Model.ApiModels;
using BehtashShirzad.Models.DbModels;
 
using Microsoft.EntityFrameworkCore;
 
using SharedObjects;
using System.Collections.Frozen;
 

namespace BehtashShirzad.Model.Context.DAL
{
    public class InvoiceDAL
    {

        public static IEnumerable<Invoice> GetInvoice()
        {

            using (var cn = new DbCommiter())
            {
                try
                { 

                    return cn.Invoices.Include( _=>_.User).Include(_=>_.Products).ToFrozenSet();

                }
                catch (Exception)
                {

                    return Enumerable.Empty<Invoice>();
                }
            }

        }

        public static IEnumerable<Invoice> GetInvoiceByuser(string username)
        {

            using (var cn = new DbCommiter())
            {
                try
                {

                    return cn.Invoices.Where(_=>_.User.Username==username).Include(_ => _.User).Include(_ => _.Products).ToFrozenSet();

                }
                catch (Exception)
                {

                    return Enumerable.Empty<Invoice>();
                }
            }

        }

        public static async Task<Constants.Status> CreateInvoice(InvoiceDto i)
        {
            try
            {
                 
                using (var cn = new DbCommiter())
                {
                    var products = new List<Product>();
                    foreach (var item in i.products)
                    {
                      var product= cn.Products?.Where(_=>_.Id==item)?.FirstOrDefault();
                        if (product != null)
                        products.Add(product);

                    }

                    if( products.Count == 0){

                        return Constants.Status.Fail;
                    }
                    

                    var Invoice = new Invoice()
                    {
                        User = cn.Users.Find(i.userId),
                        Products = products

                    };
                    await cn.Invoices.AddAsync(Invoice);
                    await cn.SaveChangesAsync();
                    return Constants.Status.Success;
                }
            }
            catch (Exception)
            {

                return Constants.Status.Fail;
            }
        }
 

        static public bool DeleteInvoice(int id)
        {

            try
            {
                using (var db = new DbCommiter())
                {
                    db.Invoices.Where(_ => _.Id == id).ExecuteDelete();
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
