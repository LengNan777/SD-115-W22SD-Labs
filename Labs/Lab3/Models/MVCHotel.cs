namespace Lab3.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    static class Hotel
    {
        public static ICollection<Room> IRooms { get; set; } = new List<Room>();
        public static string Name { get; set; } = "premadeHotel";
        public static string Address { get; set; } = "Winnipeg";
        public static List<Room> Rooms { get; set; } = new List<Room>();
        public static List<Client> Clients { get; set; } = new List<Client>();
        public static List<Reservation> Reservations { get; set; } = new List<Reservation>();
        public static int ID { get; set; } = 1;

        static Hotel()
            {

                for (int i = 0; i < 6; i++)
                {
                    Hotel.Rooms.Add(new Room($"Room{i + 1}", i + 1, false));
                }

                for (int i = 0; i < 4; i++)
                {
                    Hotel.Clients.Add(new Client($"Client{i + 1}", Convert.ToInt64(i)));
                }

                Reservation reservation1 = new Reservation(new DateTime(), 1, true, Hotel.GetClient(1), Hotel.GetRoom("Room1"), new DateTime(2022, 7, 1, 18, 0, 0));
                Reservation reservation2 = new Reservation(new DateTime(), 2, true, Hotel.GetClient(1), Hotel.GetRoom("Room2"), new DateTime(2022, 7, 2, 18, 0, 0));
                Reservation reservation3 = new Reservation(new DateTime(), 1, true, Hotel.GetClient(2), Hotel.GetRoom("Room3"), new DateTime(2022, 7, 3, 19, 30, 0));
                Reservation reservation4 = new Reservation(new DateTime(), 2, true, Hotel.GetClient(3), Hotel.GetRoom("Room4"), new DateTime(2022, 7, 3, 20, 0, 0));

                Hotel.Reservations.Add(reservation1);
                Hotel.Reservations.Add(reservation2);
                Hotel.Reservations.Add(reservation3);
                Hotel.Reservations.Add(reservation4);

                Hotel.GetClient(1).Reservations.Add(reservation1);
                Hotel.GetClient(1).Reservations.Add(reservation2);
                Hotel.GetClient(2).Reservations.Add(reservation3);
                Hotel.GetClient(3).Reservations.Add(reservation4);

                Hotel.GetRoom("Room1").Reservations.Add(reservation1);
                Hotel.GetRoom("Room1").Occupied = true;
                Hotel.GetRoom("Room2").Reservations.Add(reservation2);
                Hotel.GetRoom("Room2").Occupied = true;
                Hotel.GetRoom("Room3").Reservations.Add(reservation3);
                Hotel.GetRoom("Room3").Occupied = true;
                Hotel.GetRoom("Room4").Reservations.Add(reservation4);
                Hotel.GetRoom("Room4").Occupied = true;

                Console.WriteLine("-----------------Lab1 test----------------------");
                Console.WriteLine();
                Console.WriteLine(Hotel.GetClient(1).Name);
                Console.WriteLine(Hotel.GetRoom("Room1").RoomNumber);
                Console.WriteLine("reservation ID: " + Hotel.GetReservation(7).ReservationID);

                for (int i = 0; i < Hotel.GetVacantRooms().Count; i++)
                {
                    Console.WriteLine(Hotel.GetVacantRooms()[i].RoomNumber);
                }

                for (int i = 0; i < 3; i++)
                {
                    Console.WriteLine(Hotel.TopThreeClients()[i].Name);
                }

                Console.WriteLine(Hotel.AutomaticReservation(1, 6).Room.RoomNumber);

                //Lab 2 test
                Console.WriteLine();
                Console.WriteLine("-----------------Lab2 test----------------------");
                Console.WriteLine();
                Console.WriteLine("Remaining: " + Hotel.TotalCapacityRemaining());
                Console.WriteLine("The percentage of occupy is " + Hotel.AverageOccupancyPercentage() + "%.");
                Hotel.CheckoutRoom("Client1");
                Console.WriteLine("Remaining: " + Hotel.TotalCapacityRemaining());
                Console.WriteLine("The percentage of occupy is " + Hotel.AverageOccupancyPercentage() + "%.");
                Hotel.Checkin("Client1");
                Console.WriteLine("Remaining: " + Hotel.TotalCapacityRemaining());
                Console.WriteLine("The percentage of occupy is " + Hotel.AverageOccupancyPercentage() + "%.");
            }

        public static Client GetClient(int clientID)
        {
            try
            {
                Client client = (from cli in Clients
                                 where cli.ClientID == clientID
                                 select cli).ToList()[0];
                return client;
            }
            catch
            {
                throw new Exception("Client with the client ID do not exist.");
            }
        }

        public static Reservation GetReservation(int ID)
        {
            try
            {
                Reservation reservation = (from re in Reservations
                                           where re.ReservationID == ID
                                           select re).ToList()[0];
                return reservation;
            }
            catch
            {
                throw new Exception("The reservation with this ID do not exist.");
            }
        }
        public static Room GetRoom(string RoomNumber)
        {
            try
            {
                Room room = (from ro in Rooms
                             where ro.RoomNumber == RoomNumber
                             select ro).ToList()[0];
                return room;
            }
            catch
            {
                throw new Exception("The room with this room number do not exist.");
            }
        }

        public static List<Room> GetVacantRooms()
        {
            try
            {
                List<Room> vacantRooms = (from ro in Rooms
                                          where ro.Occupied == false
                                          select ro).ToList();
                return vacantRooms;
            }
            catch
            {
                throw new Exception("Sorry. There is not vacant rooms.");
            }


            List<Room> rooms = (from ro in Rooms
                                where ro.Occupied == false
                                select ro).ToList();
            return rooms;
        }

        public static List<Client> TopThreeClients()
        {
            try
            {
                List<Client> topThreeClients = new List<Client>();
                topThreeClients = (from Client in Clients
                                   orderby Client.Reservations.Count descending
                                   select Client).ToList().GetRange(0, 3);
                return topThreeClients;
            }
            catch
            {
                throw new Exception("Sorry! There is no enough person.");
            }
        }

        public static Reservation AutomaticReservation(int clientID, int occupants)
        {
            try
            {
                Reservation reservation;
                Room room = (from ro in Rooms
                             where (ro.Occupied == false && ro.Capacity >= occupants)
                             select ro).ToList()[0];
                reservation = new Reservation(new DateTime(), occupants, true, GetClient(clientID), room, DateTime.Now);
                Hotel.GetClient(clientID).Reservations.Add(reservation);
                room.Reservations.Add(reservation);
                room.Occupied = true;
                Hotel.Reservations.Add(reservation);
                return reservation;
            }
            catch
            {
                throw new Exception("Sorry! All rooms are occupied.");
            }
        }

        public static Reservation ReserveRoom(int clientID, int occupants, DateTime dateTime)
        {
            try
            {
                Reservation reservation;
                Room room = (from ro in Rooms
                             where (ro.Occupied == false && ro.Capacity >= occupants)
                             select ro).ToList()[0];
                reservation = new Reservation(new DateTime(), occupants, true, GetClient(clientID), room, dateTime);
                Hotel.GetClient(clientID).Reservations.Add(reservation);
                room.Reservations.Add(reservation);
                room.Occupied = true;
                Hotel.Reservations.Add(reservation);
                return reservation;
            }
            catch
            {
                throw new Exception("Sorry! There is no room fill the need.");
            }
        }

        public static void Checkin(string clientName)
        {
            Client client = (from cli in Clients
                             where cli.Name == clientName
                             select cli).ToList()[0];
            Room room = (from ro in Rooms
                             /*There should be TimeSpan.FromHours, change it to FromDays for convenience of test.
                             There is a bug I can not solve. My original condition is: ro.Reservations[0].Client.Name == clientName && (DateTime.Now - ro.Reservations[0].StartDate) < TimeSpan.FromHours(240000).
                             These two condition statsments which check client name and date can not work when they are together. But work well seperately. */
                         where (ro.Occupied == true && (DateTime.Now - ro.Reservations[0].StartDate) < TimeSpan.FromDays(24))
                         select ro).ToList()[0];
            room.Occupied = true;
        }

        public static void CheckoutRoom(int roomNumber)
        {
            Room room = (from ro in Hotel.Rooms
                         where (ro.RoomNumber == roomNumber.ToString())
                         select ro).ToList()[0];
            room.Occupied = false;
            int reservationID = (from reservation in Hotel.Reservations
                                 where reservation.Room == room
                                 select reservation).ToList()[0].ReservationID;
            room.Reservations.Remove(GetReservation(reservationID));
            Client client = (from cli in Clients
                             where cli.Reservations[0].Room == room
                             select cli).ToList()[0];
            client.Reservations.Remove(GetReservation(reservationID));
        }
        public static void CheckoutRoom(string clientName)
        {

            Client client = (from cli in Clients
                             where cli.Name == clientName
                             select cli).ToList()[0];
            Room room = (from ro in Rooms
                         where (ro.Occupied == true && (DateTime.Now - ro.Reservations[0].StartDate) < TimeSpan.FromDays(24))
                         select ro).ToList()[0];
            int reservationID = (from reservation in Reservations
                                 where reservation.Room == room
                                 select reservation).ToList()[0].ReservationID;
            room.Occupied = false;
            room.Reservations[0].Occupants = 0;
            client.Reservations.Remove(GetReservation(reservationID));
        }

        public static int TotalCapacityRemaining()
        {
            int totalCapacityRemaining = 0;
            foreach (Room room in Hotel.Rooms)
            {
                totalCapacityRemaining += room.Capacity;
                if (room.Occupied == true)
                {
                    totalCapacityRemaining -= room.Reservations[0].Occupants;
                }
            }
            return totalCapacityRemaining;
        }

        public static int AverageOccupancyPercentage()
        {
            int totalCapacity = 0;
            int occupiedAmount = 0;
            foreach (Room room in Hotel.Rooms)
            {
                totalCapacity += room.Capacity;
                if (room.Occupied == true)
                {
                    occupiedAmount += room.Reservations[0].Occupants;
                }
            }
            return Decimal.ToInt32(Decimal.Round(Decimal.Divide(occupiedAmount, totalCapacity), 2) * 100);
        }
    }
    class Room
    {
        public string RoomNumber { get; set; }
        public int Capacity { get; set; }
        public Boolean Occupied { get; set; }
        public List<Reservation> Reservations { get; set; }

        public Room(string roomNumber, int capacity, bool occupied)
        {
            RoomNumber = roomNumber;
            Capacity = capacity;
            Occupied = occupied;
            Reservations = new List<Reservation>();
        }
    }

    class Client
    {
        public int ClientID { get; set; }
        public string Name { get; set; }
        public long CreditCard { get; set; }
        public List<Reservation> Reservations { get; set; }

        public Client(string name, long creditCard)
        {
            ClientID = Hotel.ID++;
            Name = name;
            CreditCard = creditCard;
            Reservations = new List<Reservation>();
        }
    }
    class Reservation
    {
        public int ReservationID { get; set; }
        public DateTime Date { get; set; }
        public int Occupants { get; set; }
        public bool IsCurrent { get; set; }
        public Client Client { get; set; }
        public Room Room { get; set; }
        public DateTime Created { get; set; }
        public DateTime StartDate { get; set; }

        public Reservation(DateTime date, int occupants, bool isCurrents, Client client, Room room, DateTime startDate)
        {
            ReservationID = Hotel.ID++;
            Date = date;
            Occupants = occupants;
            IsCurrent = isCurrents;
            Client = client;
            Room = room;
            Created = DateTime.Now;
            StartDate = startDate;
        }
    }



}
