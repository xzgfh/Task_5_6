using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_5_6
{

    public class  Order
    {
        private static uint _ID = 1;
        public uint ID { get; private set; }

        public OrderStatus OrderStatus { get; set; }
        public Car Car { get; set; }
        public Client Client { get; set; }
        public DateTime ReturnTime { get; set; }

        public double LeftToPay{ get; set; }
        public string OrderMessage { get; set; }


        public Order()
        {
            this.ID = _ID++;
        }
    }
}
