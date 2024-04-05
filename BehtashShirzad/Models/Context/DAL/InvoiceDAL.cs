using ElliotStore.Model.ApiModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Frozen;

namespace ElliotStore.Model.Context.DAL
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

        public static async Task<bool> CreateInvoice(InvoiceDto i)
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

                        return false;
                    }

                    var Invoice = new Invoice()
                    {
                        User = cn.Users.Find(i.userId),
                        Products = products

                    };
                    await cn.Invoices.AddAsync(Invoice);
                    await cn.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
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
