using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_5_6
{
    public enum CarStatus
        {
            None,
            Available,
            NotAvailable
        }
    public enum OrderStatus
        {
            None,
            Confirmed,
            Reject
        }
    public enum PaymentMethod
    {
        None,
        Visa,
        Cash
    }
}
