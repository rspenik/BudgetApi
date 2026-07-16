using BudgetApi.Controllers;
using BudgetApi.Core.DTOs.User;
using BudgetApi.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BudgetApi.Tests.Controllers
{
    public class UserControllerTests
    {
        private static UserDto CreateUserDto(int id = 1) => new()
        {
            Id = id,
            Username = "jdoe",
            FirstName = "Jane",
            LastName = "Doe",
            EmailAddress = "jane@example.com",
            PhoneNumber = "555-0100"
        };

        [Fact]
        public async Task GetAll_ReturnsOkWithUsers()
        {
            var service = new Mock<IUserService>();
            service.Setup(s => s.GetAllAsync()).ReturnsAsync([CreateUserDto(1), CreateUserDto(2)]);
            var controller = new UserController(service.Object);

            var result = await controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result);
            var users = Assert.IsAssignableFrom<IEnumerable<UserDto>>(ok.Value);
            Assert.Equal(2, users.Count());
        }

        [Fact]
        public async Task GetById_ExistingUser_ReturnsOkWithUser()
        {
            var service = new Mock<IUserService>();
            service.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(CreateUserDto());
            var controller = new UserController(service.Object);

            var result = await controller.GetById(1);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("jdoe", Assert.IsType<UserDto>(ok.Value).Username);
        }

        [Fact]
        public async Task GetById_MissingUser_ReturnsNotFound()
        {
            var service = new Mock<IUserService>();
            service.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((UserDto?)null);
            var controller = new UserController(service.Object);

            var result = await controller.GetById(99);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtActionWithUser()
        {
            var service = new Mock<IUserService>();
            service.Setup(s => s.CreateAsync(It.IsAny<CreateUserDto>())).ReturnsAsync(CreateUserDto());
            var controller = new UserController(service.Object);
            var dto = new CreateUserDto
            {
                Username = "jdoe",
                Password = "plain-text-password",
                FirstName = "Jane",
                LastName = "Doe",
                EmailAddress = "jane@example.com",
                PhoneNumber = "555-0100"
            };

            var result = await controller.Create(dto);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(UserController.GetById), created.ActionName);
        }

        [Fact]
        public async Task Update_IdMismatch_ReturnsBadRequest()
        {
            var service = new Mock<IUserService>();
            var controller = new UserController(service.Object);

            var result = await controller.Update(1, CreateUserDto(2));

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Update_ExistingUser_ReturnsNoContent()
        {
            var service = new Mock<IUserService>();
            service.Setup(s => s.UpdateAsync(It.IsAny<UserDto>())).ReturnsAsync(true);
            var controller = new UserController(service.Object);

            var result = await controller.Update(1, CreateUserDto());

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_MissingUser_ReturnsNotFound()
        {
            var service = new Mock<IUserService>();
            service.Setup(s => s.UpdateAsync(It.IsAny<UserDto>())).ReturnsAsync(false);
            var controller = new UserController(service.Object);

            var result = await controller.Update(1, CreateUserDto());

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent()
        {
            var service = new Mock<IUserService>();
            var controller = new UserController(service.Object);

            var result = await controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
            service.Verify(s => s.DeleteAsync(1), Times.Once);
        }
    }
}
