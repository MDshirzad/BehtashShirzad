using BehtashShirzad.Model.ApiModels;
using BehtashShirzad.Models.DbModels;
using Logger;
using Microsoft.EntityFrameworkCore;
 
using SharedObjects;
using System.Collections.Frozen;
using System.Reflection;


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

                    return cn.Invoices.Include( _=>_.User).Include(_=>_.Products).ThenInclude(product => product.Category).ToFrozenSet();

                }
                catch (Exception ex)
                {
                    Log.CreateLog(new() { LogType = Constants.LogType.Error,  Description = ex.Message, Extra = ex.InnerException?.Message });
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

                    return cn.Invoices.Where(_=>_.User.Username==username).Include(_ => _.User).Include(_ => _.Products).ThenInclude(product => product.Category).ToFrozenSet();

                }
                catch (Exception ex)
                {

            Log.CreateLog(new() {LogType=Constants.LogType.Error, Description=ex.Message,Extra= ex.InnerException?.Message});
                    return Enumerable.Empty<Invoice>();
                }
            }

        }

        public static async Task<Constants.Status> CreateInvoice(Invoice i)
        {
            try
            {
                 
                using (var cn = new DbCommiter())
                {
                    var products = new List<Product>();
                    foreach (var item in i.Products)
                    {
                      var product= cn.Products?.Where(_=>_.Id==item.Id)?.FirstOrDefault();
                        if (product != null)
                        products.Add(product);

                    }

                    if( products.Count == 0){

                        return Constants.Status.Fail;
                    }
                    

                    var Invoice = new Invoice()
                    {
                        User = cn.Users.Find(i.User.Id),
                        Products = products

                    };
                    await cn.Invoices.AddAsync(Invoice);
                    await cn.SaveChangesAsync();
                    return Constants.Status.Success;
                }
            }
            catch (Exception ex)
            {
                Log.CreateLog(new() { LogType = Constants.LogType.Error,     Description = ex.Message, Extra = ex.InnerException?.Message });
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
            catch (Exception ex)
            {
                Log.CreateLog(new() { LogType = Constants.LogType.Error,   MethodName = MethodBase.GetCurrentMethod()?.Name ,   Description = ex.Message, Extra = ex.InnerException?.Message });
                return false;
            }
        }



    }
}
