using Microsoft.AspNetCore.Mvc;
using Lab3.Models;

namespace Lab3.Controllers
{
    public class MVCHotelController : Controller
    {
        public IActionResult Index()
        {
            List<Room> rooms = Hotel.Rooms;
            return View(rooms);
        }

        public IActionResult Capacity()
        {
            ViewBag.Remaining = Hotel.GetVacantRooms().Count;
            IEnumerable<Room> rooms = Hotel.Rooms;
            return View(rooms);
        }

        public IActionResult Room(int occupants)
        {
            if(occupants > 6)
            {
                ViewBag.Message = "Sorry! There is no room available.";
                return View();
            }
            else
            {
                ViewBag.Message = null;
                Room room = (from ro in Hotel.Rooms
                             where (ro.Occupied == false & ro.Capacity >= occupants)
                             select ro).ToList()[0];
                return View(room);
            }         
        }
    }
}
