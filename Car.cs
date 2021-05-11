using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lufthavnsparkering
{
    public class Car
    {
        public string Color { get; }
        public string LicensePlate { get; }
        public Car(string color, string licensePlate)
        {
            Color = color;
            LicensePlate = licensePlate;
        }
    }
}
