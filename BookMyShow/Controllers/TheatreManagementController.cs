using System.Globalization;
using BookMyShow.Data;
using BookMyShow.Data.Entities;
using BookMyShow.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BookMyShow.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheatreManagementController : ControllerBase
    {
        private readonly BookMyShowDbContext dbContext;

        public TheatreManagementController(BookMyShowDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        [Route("Theatres")]
        public IActionResult GetAllTheatres()
        {
            var theatres = dbContext.Theatres
                .Include(t => t.TheatreOwner)
                .Include(t => t.Screens)
                .Select(t => new
                {
                    t.TheatreId,
                    t.TheatreName,
                    t.TheatreOwnerId,
                    t.TheatreOwner.TheatreOwnerName,
                    ScreenCount = t.Screens.Count,
                    Address = t.Street+", "+t.City
                })
                .ToList();
            return Ok(theatres);
        }

        [HttpGet]
        [Route("Screens/{theatreid:guid}")]
        public IActionResult GetAllScreens(Guid theatreid)
        {
            var screens = dbContext.Screens
                .Include(s => s.Theatre)
                    .ThenInclude(t => t.TheatreOwner)
                .Include(s => s.Shows)
                .Where(s => s.TheatreId == theatreid)
                .Select(s => new
                {
                    s.ScreenId,
                    s.ScreenNumber,
                    s.TheatreId,
                    s.Theatre.TheatreName,
                    Shows = s.Shows.Select(sh => new
                    {
                        sh.ShowId,
                        sh.MovieId,
                        ShowTime = DateTime.Today.Add(sh.ShowTime).ToString("hh:mm tt", CultureInfo.InvariantCulture),
                        ShowDate = sh.ShowDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        SeatsAvailableCount = sh.AvailableSeats.Count,
                        sh.TicketPrice
                    }).ToList()
                })
                .ToList();

            return Ok(screens);
        }

        [HttpGet]
        [Route("Shows/{screenid:guid}")]
        public IActionResult GetAllShows(Guid screenid)
        {
            var shows = dbContext.Shows
                .Include(s => s.Screen)
                    .ThenInclude(sc => sc.Theatre)
                        .ThenInclude(t => t.TheatreOwner)
                    .Where(s => s.ScreenId == screenid)
                .Select(s => new ShowResponseDto
                {
                    ShowId = s.ShowId,
                    ScreenId = s.ScreenId,
                    ScreenNumber = s.Screen.ScreenNumber,
                    TheatreId = s.Screen.TheatreId,
                    TheatreName = s.Screen.Theatre.TheatreName,
                    MovieId = s.MovieId,
                    ShowTime = DateTime.Today.Add(s.ShowTime).ToString("hh:mm tt", CultureInfo.InvariantCulture),
                    ShowDate = s.ShowDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    AvailableSeats = s.AvailableSeats,
                    TicketPrice = s.TicketPrice
                })
                .ToList();

            return Ok(shows);
        }

        [HttpPost]
        [Route("addScreen")]
        public IActionResult AddScreen(AddScreenDto addScreenDto)
        {
            var theatre = dbContext.Theatres
                .Include(t => t.TheatreOwner)
                .Include(t => t.Screens)
                .FirstOrDefault(t => t.TheatreId == addScreenDto.TheatreId);
            if (theatre == null)
            {
                return BadRequest("Theatre not found");
            }

            var screen = new Screen
            {
                ScreenNumber = addScreenDto.ScreenNumber,
                TheatreId = addScreenDto.TheatreId
            };

            dbContext.Screens.Add(screen);
            dbContext.SaveChanges();

            screen = dbContext.Screens
                .Include(s => s.Theatre)
                    .ThenInclude(t => t.TheatreOwner)
                .Include(s => s.Shows)
                .FirstOrDefault(s => s.ScreenId == screen.ScreenId)!;

            var response = new
            {
                screen.ScreenId,
                screen.ScreenNumber,
                screen.TheatreId,
                Theatre = new
                {
                    screen.Theatre.TheatreId,
                    screen.Theatre.TheatreName,
                    screen.Theatre.TheatreOwnerId,
                    TheatreOwner = new
                    {
                        screen.Theatre.TheatreOwner.TheatreOwnerId,
                        screen.Theatre.TheatreOwner.TheatreOwnerName
                    },
                    screen.Theatre.City,
                    screen.Theatre.Street
                },
                Shows = screen.Shows.Select(sh => new
                {
                    sh.ShowId,
                    sh.MovieId,
                    sh.ShowTime,
                    sh.ShowDate,
                    sh.AvailableSeats,
                    sh.TicketPrice
                }).ToList()
            };

            return Ok(JsonConvert.SerializeObject(response, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }

        [HttpDelete]
        [Route("deleteScreen/{screenId:guid}")]
        public IActionResult DeleteScreen(Guid screenId)
        {
            var screen = dbContext.Screens.Find(screenId);
            if (screen == null)
            {
                return NotFound();
            }

            dbContext.Screens.Remove(screen);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("updateScreen/{screenId:guid}")]
        public IActionResult UpdateScreen(Guid screenId, UpdateScreenDto updateScreenDto)
        {
            var screen = dbContext.Screens.Find(screenId);
            if (screen == null)
            {
                return NotFound();
            }

            screen.ScreenNumber = updateScreenDto.ScreenNumber;
            dbContext.SaveChanges();
            return Ok(JsonConvert.SerializeObject(screen, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }

        [HttpPost]
        [Route("addShow")]
        public IActionResult AddShow(AddShowDto addShowDto)
        {
            var screen = dbContext.Screens
                .Include(s => s.Theatre)
                .FirstOrDefault(s => s.ScreenId == addShowDto.ScreenId);

            if (screen == null)
            {
                return BadRequest("Screen not found");
            }

            List<int> AvailableSeats = new List<int>();
            for (int i = 1; i <= addShowDto.TotalSeats; i++)
                AvailableSeats.Add(i);

            var show = new Show
            {
                ScreenId = addShowDto.ScreenId,
                MovieId = addShowDto.MovieId,
                ShowTime = DateTime.ParseExact(addShowDto.ShowTime, "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay,
                ShowDate = DateTime.ParseExact(addShowDto.ShowDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                AvailableSeats = AvailableSeats,
                TicketPrice = addShowDto.TicketPrice
            };

            dbContext.Shows.Add(show);
            dbContext.SaveChanges();
            return Ok(new ShowResponseDto
            {
                ShowId = show.ShowId,
                ScreenId = show.ScreenId,
                ScreenNumber = screen.ScreenNumber,
                TheatreId = screen.TheatreId,
                TheatreName = screen.Theatre.TheatreName,
                MovieId = show.MovieId,
                ShowTime = show.ShowTime.ToString(@"hh\:mm tt", CultureInfo.InvariantCulture),
                ShowDate = show.ShowDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                AvailableSeats = show.AvailableSeats,
                TicketPrice = show.TicketPrice
            });
        }

        [HttpDelete]
        [Route("deleteShow/{showId:guid}")]
        public IActionResult DeleteShow(Guid showId)
        {
            var show = dbContext.Shows.Find(showId);
            if (show == null)
            {
                return NotFound();
            }

            dbContext.Shows.Remove(show);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("updateShow/{showId:guid}")]
        public IActionResult UpdateShow(Guid showId, UpdateShowDto updateShowDto)
        {
            var show = dbContext.Shows.Find(showId);
            if (show == null)
            {
                return NotFound();
            }

            show.MovieId = updateShowDto.MovieId;
            show.ShowTime = TimeSpan.ParseExact(updateShowDto.ShowTime, @"hh\:mm tt", CultureInfo.InvariantCulture);
            show.ShowDate = DateTime.ParseExact(updateShowDto.ShowDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            show.AvailableSeats = updateShowDto.AvailableSeats;
            show.TicketPrice = updateShowDto.TicketPrice;
            dbContext.SaveChanges();
            return Ok(JsonConvert.SerializeObject(show, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }));
        }

        [HttpDelete]
        [Route("{ownerId:guid}")]
        public IActionResult RemoveTheatreofTheatreOwner(Guid ownerId)
        {
            var theatreOwner = dbContext.TheatreOwners.Find(ownerId);
            if (theatreOwner is null)
            {
                return BadRequest("Theatre Owner not found.");
            }
            var theatreids = dbContext.Theatres.Where(th => th.TheatreOwnerId == ownerId);
            foreach (var theatre in theatreids)
            {
                dbContext.Theatres.Remove(theatre);
            }
            dbContext.SaveChanges();
            return Ok();
        }
    }
}
