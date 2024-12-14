using Api.Controllers;
using Application.Commands.BookCommands;
using FakeItEasy;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Test
{
    public class BooksControllerTests
    {
        private IMediator _mediator;
        private BooksController _controller;

        [SetUp]
        public void SetUp()
        {
            _mediator = A.Fake<IMediator>();
            _controller = new BooksController(_mediator);
        }

        [Test]
        public async Task AddBook_ShouldReturnCreatedAtAction_WhenValidRequest()
        {
            // Arrange
            var command = new AddBookCommand("Book Name", "Book Title", "Book Description", 1);
            var book = new Book { Id = 1, Name = "Book Name", Title = "Book Title", Description = "Book Description", AuthorId = 1 };

            // Ställ in med FakeItEasy för att returnera boken som skapades
            A.CallTo(() => _mediator.Send(command, default))
                .Returns(book);

            // Act
            var result = await _controller.AddBook(command, default);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result as CreatedAtActionResult;
            createdResult?.StatusCode.Should().Be(201);
            createdResult?.Value.Should().BeEquivalentTo(book);
        }
    }
}
