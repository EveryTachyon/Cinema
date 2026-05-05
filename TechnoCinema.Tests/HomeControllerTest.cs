using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnoCinema.Controllers;
using TechnoCinema.Data;
using TechnoCinema.Models;

public class HomeControllerTests
{
    private ApplicationDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    private Banner MakeBanner(int id, int position) => new Banner
    {
        Id = id,
        Position = position,
        Name = "Test Banner",
        ImageUrl = "/img/test.jpg",
        TargetUrl = "/test"
    };

    [Fact]
    public async Task Banner_ReturnsViewWithBannersOrderedByPosition()
    {
        var db = GetDbContext();
        db.Banners.AddRange(
            MakeBanner(1, 3),
            MakeBanner(2, 1),
            MakeBanner(3, 2)
        );
        await db.SaveChangesAsync();

        var controller = new HomeController(db);
        var result = await controller.Banner() as ViewResult;
        var model = result!.Model as List<Banner>;

        Assert.NotNull(result);
        Assert.Equal(3, model!.Count);
        Assert.Equal(1, model[0].Position);
    }

    [Fact]
    public async Task Edit_Get_ReturnsNotFound_WhenBannerMissing()
    {
        var db = GetDbContext();
        var controller = new HomeController(db);

        var result = await controller.Edit(999);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Edit_Get_ReturnsViewWithBanner_WhenFound()
    {
        var db = GetDbContext();
        db.Banners.Add(MakeBanner(1, 1));
        await db.SaveChangesAsync();

        var controller = new HomeController(db);
        var result = await controller.Edit(1) as ViewResult;

        Assert.NotNull(result);
        Assert.IsType<Banner>(result!.Model);
    }

    [Fact]
    public async Task Edit_Post_RedirectsToBanner_WhenModelValid()
    {
        var db = GetDbContext();
        db.Banners.Add(MakeBanner(1, 1));
        await db.SaveChangesAsync();

        var controller = new HomeController(db);
        var updated = MakeBanner(1, 5);
        var result = await controller.Edit(updated) as RedirectToActionResult;

        Assert.NotNull(result);
        Assert.Equal("Banner", result!.ActionName);
    }

    [Fact]
    public void Index_ReturnsViewWithBanners()
    {
        var db = GetDbContext();
        db.Banners.AddRange(
            MakeBanner(1, 2),
            MakeBanner(2, 1)
        );
        db.SaveChanges();

        var controller = new HomeController(db);
        var result = controller.Index() as ViewResult;
        var model = result!.Model as List<Banner>;

        Assert.NotNull(result);
        Assert.Equal(2, model!.Count);
        Assert.Equal(1, model[0].Position);
    }
}