using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using TechnoCinema.Data;
using TechnoCinema.Models;

public class BuyControllerTests
{
    private ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private void SeedBasicData(ApplicationDbContext context)
    {
        var movie = new Movie { Id = 1, Title = "Avatar" };

        var seats = new List<Seat>
        {
            new Seat { Id = 1, Row = 0, Column = 1 }, // A1
            new Seat { Id = 2, Row = 0, Column = 2 }, // A2
            new Seat { Id = 3, Row = 1, Column = 1 }  // B1
        };

        var room = new Room
        {
            Id = 1,
            Seats = seats
        };

        var showtime = new Showtime
        {
            Id = 1,
            Film = "Avatar",
            Room = 1,
            ReleaseDate = DateTime.Now
        };

        context.Movies.Add(movie);
        context.Rooms.Add(room);
        context.Showtimes.Add(showtime);
        context.Seats.AddRange(seats);

        context.SaveChanges();
    }

    //  INDEX 

   

    [Fact]
    public void Index_ReturnsNotFound_WhenShowtimeMissing()
    {
        var controller = new BuyController(CreateContext());

        var result = controller.Index(999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Index_ReturnsNotFound_WhenRoomMissing()
    {
        var context = CreateContext();

        context.Showtimes.Add(new Showtime
        {
            Id = 1,
            Film = "Avatar",
            Room = 999
        });
        context.SaveChanges();

        var controller = new BuyController(context);

        var result = controller.Index(1);

        Assert.IsType<NotFoundResult>(result);
    }

    //  CONFIRM 


    [Fact]
    public void Confirm_ReturnsNotFound_WhenShowtimeMissing()
    {
        var controller = new BuyController(CreateContext());

        var result = controller.Confirm(999, new[] { "A1" });

        Assert.IsType<NotFoundResult>(result);
    }

    //  ADMIN RESET 

    [Fact]
    public void AdminReset_ReturnsNotFound_WhenShowtimeMissing()
    {
        var controller = new BuyController(CreateContext());

        var result = controller.AdminReset(999);

        Assert.IsType<NotFoundResult>(result);
    }
}