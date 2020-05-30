using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace StackusingQueue
{
    public class InvoiceRepository 
    {

        public static void Main(String [] args)
        {
            Invoice inv = new Invoice();
            inv.Id = 23;
            InvoiceItem i1 = new InvoiceItem();
            i1.Price = 3;

            InvoiceItem i2 = new InvoiceItem();
            i2.Price = 5;
            IList<InvoiceItem> items1 = new List<InvoiceItem>();
            items1.Add(i1);
            items1.Add(i2);
            inv.InvoiceItems = items1;

            Invoice inv3 = new Invoice();
            inv3.Id = 33;

            InvoiceItem i31 = new InvoiceItem();
            i31.Price = 20;

            InvoiceItem i32 = new InvoiceItem();
            i32.Price = 30;
            IList<InvoiceItem> items31 = new List<InvoiceItem>();
            items31.Add(i31);
            items31.Add(i32);
            inv3.InvoiceItems = items31;

            IList<Invoice> invoices = new List<Invoice>();
            invoices.Add(inv);
            invoices.Add(inv3);
            InvoiceRepository repo = new InvoiceRepository(invoices.AsQueryable());
            Console.WriteLine(repo.GetTotal(23));
            Console.WriteLine("done");
            Console.ReadLine();
        }
        IQueryable<Invoice> invoices = null;
        public InvoiceRepository(IQueryable<Invoice> invoices)
        {
            this.invoices = invoices;
        }
        /// <summary>
        /// Should return a total value of an invoice with a given id. If an invoice does not exist null should be returned.
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        public decimal? GetTotal(int invoiceId)
        {
            var matchedInovice = invoices.Where(inv => inv.Id.Equals(invoiceId));
            if(matchedInovice.Any())
            {
                return matchedInovice.First().InvoiceItems.Select(invItem => invItem.Price).Sum();
            }
            return null;

        }
        /// <summary>
        /// Should return a total value of all unpaid invoices.
        /// </summary>
        /// <returns></returns>
        public decimal GetTotalOfUnpaid()
        {
           return invoices.Where(inv => inv.AcceptanceDate == null).Select(
                               inv => inv.InvoiceItems.Select(invItem => invItem.Price).Sum()).Sum();
        }
        /// <summary>
        /// Should return a dictionary where the name of an invoice item is a key and the number of bought items is a value.
        /// The number of bought items should be summed within a given period of time (from, to). Both the from date and the end date can be null.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public IReadOnlyDictionary<string, long> GetItemsReport(DateTime? from, DateTime? to)
        {
            Dictionary<string, long> itemsCount = new Dictionary<string, long>();
            IEnumerable<Invoice> matchedInvoices = invoices.Where(invoice => liesIntheRange(from, to, invoice.CreationDate)).ToList();

            foreach (Invoice inv in matchedInvoices)
            {
                foreach (InvoiceItem item in  inv.InvoiceItems)
                {
                    itemsCount[item.Name] = itemsCount.GetValueOrDefault(item.Name, 0);
                }
            }
            return itemsCount;

            //.ToDictionary(inv => inv.Description,

            //    inv => (long) inv.InvoiceItems.Count);
            
            
        }
        private static bool liesIntheRange(DateTime? from, DateTime? to, DateTime input)
        {
            if (from != null && from > input)
                return false;
            else if (to != null && to < input)
                return false;
            return true;
        }
    }
}

public class Invoice
{
    // A unique numerical identifier of an invoice (mandatory)
    public int Id { get; set; }
    // A short description of an invoice (optional).
    public string Description { get; set; }
    // A number of an invoice e.g. 134/10/2018 (mandatory).
    public string Number { get; set; }
    // An issuer of an invoice e.g. Metz-Anderson, 600  Hickman Street,Illinois (mandatory).
    public string Seller { get; set; }
    // A buyer of a service or a product e.g. John Smith, 4285  Deercove Drive, Dallas (mandatory).
    public string Buyer { get; set; }
    // A date when an invoice was issued (mandatory).
    public DateTime CreationDate { get; set; }
    // A date when an invoice was paid (optional).
    public DateTime? AcceptanceDate { get; set; }
    // A collection of invoice items for a given invoice (can be empty but is never null).
    public IList<InvoiceItem> InvoiceItems { get; set; }
    public Invoice()
    {
        InvoiceItems = new List<InvoiceItem>();
    }
}
public class InvoiceItem
{
    // A name of an item e.g. eggs.
    public string Name { get; set; }
    // A number of bought items e.g. 10.
    public uint Count { get; set; }
    // A price of an item e.g. 20.5.
    public decimal Price { get; set; }
}

