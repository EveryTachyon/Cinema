using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TechnoCinema.Data;
using TechnoCinema.Models;
using System;
using System.Linq;
using System.Collections.Generic;

public class DetailsControllerTests
{
    private ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private void SeedShowtimes(ApplicationDbContext context)
    {
        context.Showtimes.AddRange(
            new Showtime
            {
                Id = 1,
                Film = "Avatar",
                KinoNimi = "Apollo",
                ReleaseDate = DateTime.Now.AddDays(1)
            },
            new Showtime
            {
                Id = 2,
                Film = "Avatar",
                KinoNimi = "Cinamon",
                ReleaseDate = DateTime.Now
            },
            new Showtime
            {
                Id = 3,
                Film = "Batman",
                KinoNimi = "Apollo",
                ReleaseDate = DateTime.Now.AddDays(2)
            }
        );

        context.SaveChanges();
    }

    // ───────── INDEX ─────────

    [Fact]
    public void Index_NoName_ReturnsAllOrdered()
    {
        var context = CreateContext();
        SeedShowtimes(context);

        var controller = new DetailsController(context);

        var result = controller.Index(null) as ViewResult;
        var model = Assert.IsType<List<Showtime>>(result.Model);

        Assert.Equal(3, model.Count);

        // ordered by ReleaseDate
        Assert.True(model[0].ReleaseDate <= model[1].ReleaseDate);
    }

    [Fact]
    public void Index_FilterByName_ReturnsMatching()
    {
        var context = CreateContext();
        SeedShowtimes(context);

        var controller = new DetailsController(context);

        var result = controller.Index("Avatar") as ViewResult;
        var model = Assert.IsType<List<Showtime>>(result.Model);

        Assert.Equal(2, model.Count);
        Assert.All(model, s => Assert.Equal("Avatar", s.Film));
    }

    [Fact]
    public void Index_Filter_IsCaseInsensitive()
    {
        var context = CreateContext();
        SeedShowtimes(context);

        var controller = new DetailsController(context);

        var result = controller.Index("avatar") as ViewResult;
        var model = Assert.IsType<List<Showtime>>(result.Model);

        Assert.Equal(2, model.Count);
    }

    [Fact]
    public void Index_NoMatch_ReturnsNotFound()
    {
        var context = CreateContext();
        SeedShowtimes(context);

        var controller = new DetailsController(context);

        var result = controller.Index("NonExisting");

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Index_EmptyDatabase_ReturnsNotFound()
    {
        var controller = new DetailsController(CreateContext());

        var result = controller.Index(null);

        Assert.IsType<NotFoundResult>(result);
    }
}