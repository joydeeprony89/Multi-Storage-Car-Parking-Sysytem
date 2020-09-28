using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Multi_Storage_Car_Parking_System
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("parking Lot System!");
        }

        //ParkingLot
        //ParkingSpot
        //Vehicle

        public enum ParkingType
        {
            Handicap,
            Large,
            Compact,
            Motorcycle
        }

        public class Vehicle
        {
            public string Number { get; set; }
        }

        public class ParkingSpace
        {
            public string Floor { get; set; }
            public int Number { get; set; }
            public ParkingType Type { get; set; }
            public Vehicle Vehicle { get; set; }
            public bool IsOccupied { get; set; }
            public int Distance { get; set; }
        }

        public interface IParking
        {
            bool Parkvehicle(Vehicle vehicle, ParkingType parkingType);
            bool ReleaseVehicle(Vehicle vehicle);
            bool IsParkingLotFull();
            bool IsParkingLotEmpty();
        }

        public class ParkingLot : IParking
        {
            public HashSet<ParkingSpace> AvailableSpaces = new HashSet<ParkingSpace>();
            public HashSet<ParkingSpace> UsedSpaces = new HashSet<ParkingSpace>();

            const int MAX_SLOT = 4;
            bool IsFull, IsEmpty;

            public bool Parkvehicle(Vehicle vehicle, ParkingType parkingType)
            {
                if (!IsFull)
                {
                    ParkingSpace availableSpace = AvailableSpaces.Where(p => p.Type == parkingType).OrderBy(p => p).FirstOrDefault();
                    if (availableSpace != null)
                    {
                        availableSpace.Vehicle = vehicle;
                        availableSpace.IsOccupied = true;
                        UsedSpaces.Add(availableSpace);
                        AvailableSpaces.Remove(availableSpace);
                        if (UsedSpaces.Count == MAX_SLOT)
                            IsFull = true;
                        IsEmpty = false;
                        return true;
                    } return false;
                }
                else
                {
                    Console.WriteLine("Parking is Full");
                    return false;
                }
            }

            public bool ReleaseVehicle(Vehicle vehicle)
            {
                if (!IsEmpty)
                {
                    var vehicleToBeReleased = UsedSpaces.Where(s => s.Vehicle == vehicle).FirstOrDefault();
                    if (vehicleToBeReleased != null)
                    {
                        vehicleToBeReleased.Vehicle = null;
                        vehicleToBeReleased.IsOccupied = false;
                        UsedSpaces.Remove(vehicleToBeReleased);
                        AvailableSpaces.Add(vehicleToBeReleased);
                        if (AvailableSpaces.Count == MAX_SLOT)
                            IsEmpty = true;
                        IsFull = false;

                        return true;
                    }
                    else return false;
                }
                else 
                {
                    Console.WriteLine("No vehicles are parked.");
                    return false; 
                }
            }

            public bool IsParkingLotFull()
            {
                return IsFull;
            }

            public bool IsParkingLotEmpty()
            {
                return IsEmpty;
            }
        }
    }
}
