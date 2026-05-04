using Xunit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechnoCinema.Controllers;
using TechnoCinema.Data;
using TechnoCinema.Models;

namespace TechnoCinema.Tests
{
    public class HomeControllerTests
    {
        private ApplicationDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDbContext(options);
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            var context = CreateContext();
            var controller = new HomeController(context);

            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_ViewBagBannersOrderedByPosition()
        {
            var context = CreateContext();
            context.Banners.AddRange(
                new Banner { Position = 3, Name = "C", ImageUrl = "c.jpg", TargetUrl = "/c" },
                new Banner { Position = 1, Name = "A", ImageUrl = "a.jpg", TargetUrl = "/a" },
                new Banner { Position = 2, Name = "B", ImageUrl = "b.jpg", TargetUrl = "/b" }
            );
            context.SaveChanges();
            var controller = new HomeController(context);

            controller.Index();

            var banners = controller.ViewBag.Banners as List<Banner>;
            Assert.Equal(new[] { 1, 2, 3 }, banners.Select(b => b.Position));
        }

        [Fact]
        public void Index_TopMoviesOrderedByRatingDescending()
        {
            var context = CreateContext();
            context.Movies.AddRange(
                new Movie { Title = "A", Rating = 6 },
                new Movie { Title = "B", Rating = 9 },
                new Movie { Title = "C", Rating = 7 }
            );
            context.SaveChanges();
            var controller = new HomeController(context);

            controller.Index();

            var movies = controller.ViewBag.TopMovies as List<Movie>;
            Assert.Equal(new[] { 9, 7, 6 }, movies.Select(m => (int)m.Rating));
        }

        [Fact]
        public void Index_TopMoviesMax()
        {
            var context = CreateContext();
            for (int i = 1; i <= 10; i++)
                context.Movies.Add(new Movie { Title = $"Movie {i}", Rating = i });
            context.SaveChanges();
            var controller = new HomeController(context);

            controller.Index();

            var movies = controller.ViewBag.TopMovies as List<Movie>;
            Assert.True(movies.Count <= 5);
        }

        [Fact]
        public void Edit_ReturnsBannersAsModel()
        {
            var context = CreateContext();
            context.Banners.AddRange(
                new Banner { Position = 1, Name = "A", ImageUrl = "a.jpg", TargetUrl = "/a" },
                new Banner { Position = 2, Name = "B", ImageUrl = "b.jpg", TargetUrl = "/b" }
            );
            context.SaveChanges();
            var controller = new HomeController(context);

            var result = controller.Edit() as ViewResult;

            var model = result.Model as List<Banner>;
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public void DeleteBanner_RemovesBannerAndRedirects()
        {
            var context = CreateContext();
            context.Banners.Add(new Banner { Id = 1, Position = 1, Name = "A", ImageUrl = "a.jpg", TargetUrl = "/a" });
            context.SaveChanges();
            var controller = new HomeController(context);

            var result = controller.DeleteBanner(1);

            Assert.Equal(0, context.Banners.Count());
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Edit", redirect.ActionName);
        }

        [Fact]
        public void CreateBanner_Get_ReturnsViewResult()
        {
            var controller = new HomeController(CreateContext());

            var result = controller.CreateBanner();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void CreateBanner_Post_ValidBanner_SavesAndRedirectsToEdit()
        {
            var context = CreateContext();
            var controller = new HomeController(context);
            var banner = new Banner { Name = "New", ImageUrl = "new.jpg", TargetUrl = "/new", Position = 1 };

            var result = controller.CreateBanner(banner);

            Assert.Equal(1, context.Banners.Count());
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Edit", redirect.ActionName);
        }

        [Fact]
        public void EditBanner_Get_ReturnsViewWithBanner()
        {
            var context = CreateContext();
            context.Banners.Add(new Banner { Id = 1, Name = "A", ImageUrl = "a.jpg", TargetUrl = "/a", Position = 1 });
            context.SaveChanges();
            var controller = new HomeController(context);

            var result = controller.EditBanner(1) as ViewResult;

            var model = Assert.IsType<Banner>(result.Model);
            Assert.Equal("A", model.Name);
        }

        [Fact]
        public void EditBanner_Post_ValidBanner_UpdatesAndRedirectsToEdit()
        {
            var context = CreateContext();
            context.Banners.Add(new Banner { Id = 1, Name = "Old", ImageUrl = "old.jpg", TargetUrl = "/old", Position = 1 });
            context.SaveChanges();
            context.ChangeTracker.Clear(); // detach all tracked entities
            var controller = new HomeController(context);
            var updated = new Banner { Id = 1, Name = "New", ImageUrl = "new.jpg", TargetUrl = "/new", Position = 1 };

            var result = controller.EditBanner(1, updated);

            Assert.Equal("New", context.Banners.Find(1).Name);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Edit", redirect.ActionName);
        }

        [Fact]
        public void Edit_ReturnsBannersOrderedByPosition()
        {
            var context = CreateContext();
            context.Banners.AddRange(
                new Banner { Position = 2, Name = "B", ImageUrl = "b.jpg", TargetUrl = "/b" },
                new Banner { Position = 1, Name = "A", ImageUrl = "a.jpg", TargetUrl = "/a" }
            );
            context.SaveChanges();
            var controller = new HomeController(context);

            var result = controller.Edit() as ViewResult;
            var model = result.Model as List<Banner>;

            Assert.Equal(new[] { 1, 2 }, model.Select(b => b.Position));
        }
    }
}