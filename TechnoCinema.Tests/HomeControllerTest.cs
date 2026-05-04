using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TechnoCinema.Data;
using TechnoCinema.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

public class HomeControllerTests
{
    private ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    private void SeedBanners(ApplicationDbContext context)
    {
        context.Banners.AddRange(
            new Banner
            {
                Id = 1,
                Name = "Banner1",
                Position = 2,
                ImageUrl = "img1.jpg",
                TargetUrl = "https://test1.com"
            },
            new Banner
            {
                Id = 2,
                Name = "Banner2",
                Position = 1,
                ImageUrl = "img2.jpg",
                TargetUrl = "https://test2.com"
            }
        );

        context.SaveChanges();
    }

    // ───────── INDEX ─────────

    [Fact]
    public void Index_ReturnsView_WithOrderedBanners()
    {
        var context = CreateContext();
        SeedBanners(context);

        var controller = new HomeController(context);

        var result = controller.Index() as ViewResult;
        var model = Assert.IsType<List<Banner>>(result.Model);

        Assert.Equal(2, model.Count);
        Assert.Equal(1, model[0].Position); // ordered
    }

    // ───────── BANNER (ASYNC) ─────────

    [Fact]
    public async Task Banner_ReturnsView_WithOrderedData()
    {
        var context = CreateContext();
        SeedBanners(context);

        var controller = new HomeController(context);

        var result = await controller.Banner() as ViewResult;
        var model = Assert.IsType<List<Banner>>(result.Model);

        Assert.Equal(2, model.Count);
        Assert.Equal(1, model.First().Position);
    }

    [Fact]
    public async Task Banner_Empty_ReturnsEmptyList()
    {
        var controller = new HomeController(CreateContext());

        var result = await controller.Banner() as ViewResult;
        var model = Assert.IsType<List<Banner>>(result.Model);

        Assert.Empty(model);
    }

    // ───────── EDIT GET ─────────

    [Fact]
    public async Task Edit_Get_ReturnsView_WhenExists()
    {
        var context = CreateContext();
        SeedBanners(context);

        var controller = new HomeController(context);

        var result = await controller.Edit(1);

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Edit_Get_ReturnsNotFound_WhenMissing()
    {
        var controller = new HomeController(CreateContext());

        var result = await controller.Edit(999);

        Assert.IsType<NotFoundResult>(result);
    }

    // ───────── EDIT POST ─────────

    [Fact]
    public async Task Edit_Post_Valid_Redirects()
    {
        var context = CreateContext();
        SeedBanners(context);

        var controller = new HomeController(context);

        var banner = new Banner
        {
            Id = 1,
            Name = "Updated",
        };

        var result = await controller.Edit(banner) as RedirectToActionResult;

        Assert.NotNull(result);
        Assert.Equal("Banner", result.ActionName);
    }

    [Fact]
    public async Task Edit_Post_Invalid_ReturnsView()
    {
        var context = CreateContext();
        var controller = new HomeController(context);

        controller.ModelState.AddModelError("Title", "Required");

        var banner = new Banner();

        var result = await controller.Edit(banner) as ViewResult;

        Assert.NotNull(result);
    }
}