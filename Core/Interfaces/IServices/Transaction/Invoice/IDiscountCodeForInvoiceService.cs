using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TLGames.Core.Interfaces.IServices.Transaction.Invoice
{
    internal interface IDiscountCodeForInvoiceService<T> : IBaseLinkingDataService<T> where T : class
    {
    }
}
