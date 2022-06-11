using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_5_6.MyDB
{
    public class BusyCar
    {
        public Car Car { get; set; }
        public DateTime StartTime { get; set; }
        public Client Client { get; set; }
        public Center FromCenter { get; set; }
    }
}
