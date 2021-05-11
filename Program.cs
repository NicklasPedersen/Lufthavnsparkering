using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lufthavnsparkering
{
    class Program
    {
        static void Main(string[] args)
        {
            ParkingLot[] parkingLots =
            {
                // Regular day-lots
                new ParkingLot("2", 100, 180, 24),
                new ParkingLot("13", 100, 180, 24),

                // Standard+ lots (walking distance to terminals)
                new ParkingLot("5", 800, 210, 24),
                new ParkingLot("9", 300, 210, 24),
                new ParkingLot("10", 800, 210, 24),
                new ParkingLot("12", 1000, 210, 24),
                new ParkingLot("16", 200, 210, 24),

                // Hop on/Drop off parking
                new ParkingLot("7", 800, 33, 0.5),
                new ParkingLot("8", 800, 33, 0.5),

                // Regular lots
                new ParkingLot("4", 1000, 40, 1),
                new ParkingLot("6", 1000, 40, 1),

                // Weekly lots
                new ParkingLot("15", 800, 360, 24 * 7),
                new ParkingLot("17", 800, 360, 24 * 7),
                new ParkingLot("19", 1000, 360, 24 * 7),

                new ParkingLot("Bx", 400, 50, 1),
            };

            // Simulation is thusly:
            // if you have business card => business lot
            // else if you want less than half an hour => drop off parking
            // else if you want less than 4 hours => regular lots
            // else if you want less than 2 days => day-lot/Standard+
            // else => weekly lots

            // We assume people with gold and platinum cards always want business if available
            // since they probably want to utilise the extra protection

            ParkingCentre centre = new ParkingCentre(parkingLots.ToList());
            int intervalMillis = 500;
            // 2 mins per second
            double simulationHoursPerRealtimeSecond = 0.05 * 60 / (1000 / 500);

            // seed the random object to produce reproducable behaviour
            Random random = new Random(123);
            double hours = 0;
            object _lock = new object();
            while (true)
            {
                // add 50 cars
                for (int i = 0; i < 100; i++)
                {
                    // scale it up to about 3 weeks
                    double hoursToStay = random.NextDouble() * 7 * 24 * 3;
                    // 10% chance of business class
                    bool businessClass = random.Next(10) == 0;

                    Car c = new CarFactory(random).GenerateNewCar();

                    ParkingLot parkingLot;
                    if (businessClass)
                    {
                        parkingLot = centre.FindFreeBusinessParkingLot();
                    }
                    else
                    {
                        parkingLot = centre.FindFreeRegularParkingLot();
                    }
                    if (parkingLot != null)
                    {
                        parkingLot.TryParkCar(c, hours);
                    }

                    new Thread(() =>
                    {
                        double realtimeSeconds = hoursToStay / simulationHoursPerRealtimeSecond;
                        Thread.Sleep((int)(realtimeSeconds * 1000));
                        lock (_lock)
                        {
                            centre.UnparkCarFromLot(c, parkingLot, hours);
                        }
                    }).Start();
                }
                hours += 0.05;
                // reset the cursor to avoid the blink that you get with console.clear
                Console.SetCursorPosition(0, 0);
                Console.WriteLine($"hours gone by: {hours}");
                Console.WriteLine($"money made: {centre.MoneyMade}");
                foreach (var lot in parkingLots)
                {
                    Console.WriteLine(lot);
                }
                Thread.Sleep(100);
            }
        }
    }
}
