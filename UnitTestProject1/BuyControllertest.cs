using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using TechnoCinema.Data;
using TechnoCinema.Models;
using Xunit;

public class BuyControllerTests
{
    private ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);

        // Seed test data
        var room = new Room
        {
            Id = 1,
            Seats = new List<Seat>
            {
                new Seat { Id = 1, Row = 0, Column = 1 }, // A1
                new Seat { Id = 2, Row = 0, Column = 2 }, // A2
                new Seat { Id = 3, Row = 1, Column = 1 }  // B1
            }
        };

        var movie = new Movie { Id = 1, Title = "TestMovie" };

        var showtime = new Showtime
        {
            Id = 1,
            Film = "TestMovie",
            Saal = 1
        };

        context.Rooms.Add(room);
        context.Movies.Add(movie);
        context.Showtimes.Add(showtime);

        context.SaveChanges();

        return context;
    }

    [Fact]
    public void Index_Returns_View_When_Data_Exists()
    {
        var context = GetDbContext();
        var controller = new BuyController(context);

        var result = controller.Index(1) as ViewResult;

        Assert.NotNull(result);
    }

    [Fact]
    public void Index_Returns_NotFound_When_Showtime_Not_Found()
    {
        var context = GetDbContext();
        var controller = new BuyController(context);

        var result = controller.Index(999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Confirm_Creates_Ticket_And_Redirects()
    {
        var context = GetDbContext();
        var controller = new BuyController(context);

        var selectedSeats = new[] { "A1", "A2" };

        var result = controller.Confirm(1, selectedSeats);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);

        var tickets = context.Tickets.ToList();
        Assert.Single(tickets);
        Assert.Equal(2, tickets[0].Seats.Count);
    }

    [Fact]
    public void Confirm_Returns_NotFound_When_Showtime_Invalid()
    {
        var context = GetDbContext();
        var controller = new BuyController(context);

        var result = controller.Confirm(999, new[] { "A1" });

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void AdminReset_Removes_Tickets()
    {
        var context = GetDbContext();

        // Add existing ticket
        var room = context.Rooms.Include(r => r.Seats).First();
        var movie = context.Movies.First();

        context.Tickets.Add(new Ticket
        {
            Movie = movie,
            Room = room,
            Seats = room.Seats.ToList(),
            CreatedAt = DateTime.UtcNow
        });

        context.SaveChanges();

        var controller = new BuyController(context);

        var result = controller.AdminReset(1);

        var json = Assert.IsType<JsonResult>(result);
        Assert.Equal(0, context.Tickets.Count());
    }
}