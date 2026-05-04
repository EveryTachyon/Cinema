using Xunit;
using Microsoft.EntityFrameworkCore;
using TechnoCinema.Data;
using TechnoCinema.Models;
using TechnoCinema.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

public class MovieListControllerTests
{
    private ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private void SeedMovies(ApplicationDbContext context)
    {
        context.Movies.AddRange(
            new Movie
            {
                Title = "Avatar",
                Rating = 8.5,
                Genres = new List<string> { "Action", "Adventure" },
                DurationSeconds = 7200
            },
            new Movie
            {
                Title = "Batman",
                Rating = 7.5,
                Genres = new List<string> { "Action", "Crime" },
                DurationSeconds = 7000
            },
            new Movie
            {
                Title = "Notebook",
                Rating = 6.5,
                Genres = new List<string> { "Romance" },
                DurationSeconds = 6500
            }
        );

        context.SaveChanges();
    }

    //  INDEX 

    [Fact]
    public void Index_ReturnsView()
    {
        var controller = new MovieListController(CreateContext());

        var result = controller.Index(null, null, null);

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Index_Empty_ReturnsEmptyList()
    {
        var controller = new MovieListController(CreateContext());

        var result = controller.Index(null, null, null) as ViewResult;
        var model = Assert.IsType<List<Movie>>(result.Model);

        Assert.Empty(model);
    }

    [Fact]
    public void Index_Search_Works()
    {
        var context = CreateContext();
        SeedMovies(context);

        var controller = new MovieListController(context);

        var result = controller.Index("Ava", null, null) as ViewResult;
        var model = Assert.IsType<List<Movie>>(result.Model);

        Assert.Single(model);
        Assert.Equal("Avatar", model[0].Title);
    }
    [Fact]
    public void Index_Sort_RatingDesc_Works()
    {
        var context = CreateContext();
        SeedMovies(context);

        var controller = new MovieListController(context);

        var result = controller.Index(null, null, "RatingDesc") as ViewResult;
        var model = Assert.IsType<List<Movie>>(result.Model);

        Assert.Equal("Avatar", model.First().Title);
    }

    [Fact]
    public void Index_Sort_RatingAsc_Works()
    {
        var context = CreateContext();
        SeedMovies(context);

        var controller = new MovieListController(context);

        var result = controller.Index(null, null, "RatingAsc") as ViewResult;
        var model = Assert.IsType<List<Movie>>(result.Model);

        Assert.Equal("Notebook", model.First().Title);
    }

    
}