using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_5_6.MyDB;

namespace Task_5_6
{

    public class Database
    {
        public List<HistoryCar> HistoryCars;
        public List<AvailableCar> AvailableCars;
        public List<BusyCar> BusyCars;

        public Database()
        {
            HistoryCars = new List<HistoryCar>();
            AvailableCars = new List<AvailableCar>();
            BusyCars = new List<BusyCar>();
        }


    }

    


}
