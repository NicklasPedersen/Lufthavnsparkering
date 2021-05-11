namespace Lufthavnsparkering
{
    public partial class ParkingLot
    {
        class CarTimestamp
        {
            public CarTimestamp(Car car, double timestamp)
            {
                Car = car;
                Timestamp = timestamp;
            }

            public Car Car { get; }
            public double Timestamp { get; }
        }
    }
}
