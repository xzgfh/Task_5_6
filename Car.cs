using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Task_5_6
{
  public class Car
    {
        private static uint _ID = 1;
        public uint ID { get; private set; }

        public string Number { get; private set; }
        public string Model { get; private set; }

        public Car(string model, string number)
        {
            this.Model = model;
            this.Number = number;
            this.ID = _ID++;
        }
    }
}
