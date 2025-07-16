using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ResourceBookingSystem.Data;
using ResourceBookingSystem.Models;

namespace ResourceBookingSystem.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Resource)
                .ToListAsync();
            return View(bookings);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["ResourceId"] = new SelectList(_context.Resources.Where(r => r.IsAvailable), "Id", "Name");
            return View();
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ResourceId,StartTime,EndTime,BookedBy,Purpose")] Booking booking)
        {
            // Custom validation
            if (booking.EndTime <= booking.StartTime)
            {
                ModelState.AddModelError("EndTime", "End time must be after start time.");
            }

            if (booking.StartTime < DateTime.Now)
            {
                ModelState.AddModelError("StartTime", "Cannot create bookings in the past.");
            }

            // Check for booking conflicts
            var hasConflict = await _context.Bookings
                .AnyAsync(b => b.ResourceId == booking.ResourceId &&
                    ((booking.StartTime >= b.StartTime && booking.StartTime < b.EndTime) ||
                     (booking.EndTime > b.StartTime && booking.EndTime <= b.EndTime) ||
                     (booking.StartTime <= b.StartTime && booking.EndTime >= b.EndTime)));

            if (hasConflict)
            {
                ModelState.AddModelError(string.Empty, 
                    "This resource is already booked during the requested time. Please choose another slot or resource.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ResourceId"] = new SelectList(_context.Resources.Where(r => r.IsAvailable), "Id", "Name", booking.ResourceId);
            return View(booking);
        }

        // GET: Bookings for a specific resource
        public async Task<IActionResult> ByResource(int resourceId)
        {
            var bookings = await _context.Bookings
                .Where(b => b.ResourceId == resourceId)
                .Include(b => b.Resource)
                .ToListAsync();

            ViewData["ResourceName"] = bookings.FirstOrDefault()?.Resource?.Name ?? "Unknown Resource";
            return View(bookings);
        }
    }
}