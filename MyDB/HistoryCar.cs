using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_5_6.MyDB
{
    public class HistoryCar
    {
        public Car Car { get; set; }
        public Center FromCenter { get; set; }
        public Center ToCenter { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Client Client { get; set; }
        public CarStatus Status { get; set; }
    }
}
