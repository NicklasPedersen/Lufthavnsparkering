using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lufthavnsparkering
{
    public partial class ParkingLot
    {
        public string Name { get; }
        private List<CarTimestamp> CurrentyParkedCars { get; } = new List<CarTimestamp>();
        public int CurrentParking { get => CurrentyParkedCars.Count; }
        public int MaxParking { get; }
        public int CostPerInterval { get; }
        public double HourlyChargingInterval { get; }
        public ParkingLot(string name, int maxParking, int costPerInterval, double charginIntervalHours)
        {
            Name = name;
            MaxParking = maxParking;
            CostPerInterval = costPerInterval;
            HourlyChargingInterval = charginIntervalHours;
        }
        public bool HasFreeSpace()
        {
            return CurrentyParkedCars.Count < MaxParking;
        }
        public bool TryParkCar(Car car, double hourTimestamp)
        {
            if (!HasFreeSpace())
            {
                return false;
            }
            CurrentyParkedCars.Add(new CarTimestamp(car, hourTimestamp));
            return true;
        }
        public bool IsCarInLot(Car car)
        {
            return CurrentyParkedCars.Any(carTime => carTime.Car == car);
        }
        public double UnparkCar(Car car)
        {
            CarTimestamp carTimestamp = CurrentyParkedCars.FirstOrDefault(carTime => carTime.Car == car);
            CurrentyParkedCars.Remove(carTimestamp);
            return carTimestamp.Timestamp;
        }
        public override string ToString()
        {
            // To get the amount of leading 0 we want to display,
            // we have to get the amount of digits we maximally can have, we have to take
            // Log10 of the maximum + 1, we then format it with "D" option for decimal
            // followed by amount of digits
            int digits = (int)Math.Log10(MaxParking) + 1;
            return $"{Name}: {CurrentParking.ToString("D" + digits)}/{MaxParking} cars";
        }
    }
}
