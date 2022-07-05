using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

for (int i = 0; i < 6; i++)
{
    Hotel.Rooms.Add(new Room($"Room{i+1}", i + 1, false));
}

for(int i = 0; i < 4; i++)
{
    Hotel.Clients.Add(new Client($"Client{i+1}", Convert.ToInt64(i)));
}

Reservation reservation1 = new Reservation(new DateTime(), 1, true, Hotel.GetClient(1), Hotel.GetRoom("Room1"));
Reservation reservation2 = new Reservation(new DateTime(), 2, true, Hotel.GetClient(1), Hotel.GetRoom("Room2"));
Reservation reservation3 = new Reservation(new DateTime(), 1, true, Hotel.GetClient(2), Hotel.GetRoom("Room3"));
Reservation reservation4 = new Reservation(new DateTime(), 2, true, Hotel.GetClient(3), Hotel.GetRoom("Room4"));

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

Console.WriteLine(Hotel.GetClient(1).Name);
Console.WriteLine(Hotel.GetRoom("Room1").RoomNumber);
Console.WriteLine("reservation ID: "+ Hotel.GetReservation(7).reservationID);

for(int i = 0; i < Hotel.GetVacantRooms().Count; i++)
{
    Console.WriteLine(Hotel.GetVacantRooms()[i].RoomNumber);
}

for(int i = 0; i < 3; i++)
{
    Console.WriteLine(Hotel.TopThreeClients()[i].Name);
}

Console.WriteLine(Hotel.AutomaticReservation(1,6).Room.RoomNumber);

static class Hotel
{
    public static ICollection<Room> rooms { get; set; } = new List<Room>();
    public static string Name { get; set; } = "premadeHotel";
    public static string Address { get; set; } = "Winnipeg";
    public static List<Room> Rooms { get; set; } = new List<Room>();
    public static List<Client> Clients { get; set; } = new List<Client>();
    public static List<Reservation> Reservations { get; set;} = new List<Reservation>();
    public static int ID { get; set; } = 1;

    public static Client GetClient(int clientID)
    {
        try
        {
            Client client = (from cli in Clients
                             where cli.clientID == clientID
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
                                        where re.reservationID == ID
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
            reservation = new Reservation(new DateTime(),occupants,true,GetClient(clientID),room);
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
    public int clientID { get; set; }
    public string Name { get; set; }
    public long CreditCard { get; set; }
    public List<Reservation> Reservations { get; set; }

    public Client(string name, long creditCard)
    {
        clientID = Hotel.ID++;
        Name = name;
        CreditCard = creditCard;
        Reservations = new List<Reservation>();
    }

}
class Reservation
{
    public int reservationID { get; set; }
    public DateTime Date { get; set; }
    public int Occupants { get; set; }
    public bool IsCurrent { get; set; }
    public Client Client { get; set; }
    public Room Room { get; set; }

    public Reservation(DateTime date, int occupants, bool isCurrents, Client client, Room room){
        reservationID = Hotel.ID++;
        Date = date;
        Occupants = occupants;
        IsCurrent = isCurrents;
        Client = client;
        Room = room;
    }
}

/*class VIPClient : Client
{
    public int VIPNumber { get; set; }
    public int VIPPoints { get; set; }
}

class PremiumRoom : Room
{
    public string AdditionalAmenities { get; set; }
    public int VIPValue { get; set; }
}*/

