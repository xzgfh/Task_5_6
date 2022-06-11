using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Task_5_6.MyDB;

namespace Task_5_6
{
    public class Company 
    {
        public List<BusyCar> BusyCars { get; private set; }
        public List<Center> listCenters { get; private set; }
        public Company()
        {
            listCenters = new List<Center>();
            BusyCars = new List<BusyCar>();
        }
        public void OpenCompany()
        {
            listCenters.Add(new Center("West", this));
            listCenters.Add(new Center("East", this));

            List<AvailableCar> cars1 = new List<AvailableCar>()
            {
                new AvailableCar(){Car = new Car("BMW 5-series", "1234 KH-4"), Price = 100} ,
                new AvailableCar(){Car = new Car("Dodge Challenger SRT", "5678 BV-8"), Price = 200},
                new AvailableCar(){Car = new Car("Lada Vesta", "0645 GH-1"), Price = 300}
            };
            listCenters[0].OpenCenter(cars1);

            List<AvailableCar> cars2 = new List<AvailableCar>()
            {
                new AvailableCar(){Car = new Car("Nissan Skyline", "5231 FG-4"), Price = 100} ,
                new AvailableCar(){Car = new Car("Opel Astra", "1254 HG-3"), Price = 200},
                new AvailableCar(){Car = new Car("Audi A8", "0135 ZX-1"), Price = 300}
 
            };
            listCenters[1].OpenCenter(cars2);


            //listCenters[0].print();
            //Console.WriteLine();
            //listCenters[1].print();
        }

        public Center GetCenter(int ID)
        {
            uint _ID = (uint)ID;
            return listCenters.Find((i) => i.ID == ID);
        }

        public void ConfirmOrder(BusyCar car)
        {
            BusyCars.Add(car);
        }
        public void CloseOrder(Client client, Center toCenter)
        {
            int g_ind = BusyCars.FindIndex(i => i.Client.ID == client.ID);
            Center fromCenter = BusyCars[g_ind].FromCenter;
            fromCenter.CloseOrder(client, toCenter);
            BusyCars.RemoveAt(g_ind);
        }

        public List<Center> GetCenters()
        {
            return listCenters;
            
        }
        public System.Collections.IEnumerable GetCars()
        {
            foreach (var center in listCenters)
            {
                foreach (var car in center.AvailableCars)
                {
                    yield return car.Car + " " + center.Name;
                }
            }
        } 
    }
}
