using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lufthavnsparkering
{
    class ParkingCentre
    {
        private List<ParkingLot> Lots { get; }
        public int MoneyMade { get; private set; } = 0;
        public ParkingCentre(List<ParkingLot> lots)
        {
            Lots = lots;
        }

        /// <returns>
        /// <list type="bullet">
        /// <item>
        /// <description>A business lot if one is free, otherwise;</description>
        /// </item>
        /// <item>
        /// <description>A default lot if one is free, otherwise;</description>
        /// </item>
        /// <item>
        /// <description>null</description>
        /// </item>
        /// </list>
        /// </returns>
        public ParkingLot FindFreeBusinessParkingLot()
        {
            var freeBusinessLots = Lots.Where((lot) => IsLotBusiness(lot) && lot.HasFreeSpace());
            // free business lots?
            if (freeBusinessLots.Any())
            {
                return freeBusinessLots.OrderBy((lot) => lot.CurrentParking).First();
            }
            // no, default to free parking lot
            return FindFreeRegularParkingLot();
        }

        private bool IsLotBusiness(ParkingLot l)
        {
            return l.Name.StartsWith("B");
        }

        /// <returns>
        /// <list type="bullet">
        /// <item>
        /// <description>A default lot if one is free, otherwise;</description>
        /// </item>
        /// <item>
        /// <description>null</description>
        /// </item>
        /// </list>
        /// </returns>
        public ParkingLot FindFreeRegularParkingLot()
        {
            // All regular non-business lots
            var freeLots = Lots.Where((lot) => !IsLotBusiness(lot) && lot.HasFreeSpace());
            // if there isn't any free spaces at all, it returns null
            return freeLots.OrderBy((lot) => lot.CurrentParking).FirstOrDefault();
        }

        public void UnparkCarFromLot(Car car, ParkingLot lot, double currentHours)
        {
            if (!lot.IsCarInLot(car))
            {
                throw new Exception("unabled to unpark car from lot");
            }
            double timestamp = lot.UnparkCar(car);
            double timeDiff = currentHours - timestamp;
            // calculate the money based on the amount of intervals and cost per interval
            int parkedIntervals = (int)Math.Ceiling(timeDiff / lot.HourlyChargingInterval);
            MoneyMade += parkedIntervals * lot.CostPerInterval;
        }
    }
}
