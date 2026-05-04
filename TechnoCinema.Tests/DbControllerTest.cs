using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using TechnoCinema.Data;
using TechnoCinema.Models;

public class DbControllerTests
{
    private ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public void DbStatus_ReturnsConnectionAndCount()
    {
        var context = CreateContext();

        context.Showtimes.Add(new Showtime { Film = "Test", ReleaseDate = DateTime.Now });
        context.SaveChanges();

        var controller = new DbController(context);

        var result = controller.DbStatus() as ContentResult;

        Assert.NotNull(result);
        Assert.Contains("CanConnect=True", result.Content);
        Assert.Contains("Showtimes=1", result.Content);
    }

    [Fact]
    public void DbStatus_EmptyDatabase_ReturnsZero()
    {
        var controller = new DbController(CreateContext());

        var result = controller.DbStatus() as ContentResult;

        Assert.Contains("Showtimes=0", result.Content);
    }
}