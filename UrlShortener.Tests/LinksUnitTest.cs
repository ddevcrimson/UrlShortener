using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UrlShortener.Controllers;
using UrlShortener.Models;
using UrlShortener.Models.Dto;
using UrlShortener.Services;

namespace UrlShortener.Tests
{
    class LinksServiceMock1 : ILinksService
    {
        Link? Link { get; set; }

        public Task<Link?> FirstOrNull(string uuid)
        {
            return Task.FromResult(Link);
        }

        public Task<Link> Insert(LinkDto dto)
        {
            Link = new()
            {
                Id = Guid.NewGuid(),
                Href = dto.Href,
                CreatedAt = DateTime.UtcNow,
            };
            return Task.FromResult(Link);
        }

        public Task<int> RemoveExpired(TimeSpan deadline)
        {
            return Task.FromResult(0);
        }
    }

    public class LinksUnitTest
    {
        LinksController controller = new(new LinksServiceMock1());

        [Fact]
        public void Test_Root()
        {
            Assert.Equal("Hey there!", controller.Hello());
        }

        [Fact]
        public async Task Test_LinkNotFound()
        {
            var resp = await controller.Get("");
            Assert.IsType<BadRequestObjectResult>(resp);
        }

        [Fact]
        public void Test_InvalidLink()
        {
            var model = new LinkDto { Href = "" };
            var valid = Validator.TryValidateObject(model, new(model), null);
            Assert.False(valid);
        }

        [Fact]
        public async Task Test_CreateAndGet()
        {
            var resp1 = await controller.Create(new() { Href = "http://google.com" }) as OkObjectResult;
            Assert.NotNull(resp1);
            var resp1Value = resp1!.Value as string;
            Assert.NotNull(resp1Value);
            var resp2 = await controller.Get(resp1Value);
            Assert.IsType<RedirectResult>(resp2);
        }
    }
}