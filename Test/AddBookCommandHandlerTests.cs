using Application.Commands.BookCommands;
using Application.Handlers.BookHandlers;
using Application.Interfaces.RepositoryInterfaces;
using FakeItEasy;
using FluentAssertions;
using Models;

namespace Test
{
    public class AddBookCommandHandlerTests
    {
        private IBookRepository _bookRepository;
        private IAuthorRepository _authorRepository;
        private AddBookCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            // Mocka repository
            _bookRepository = A.Fake<IBookRepository>();
            _authorRepository = A.Fake<IAuthorRepository>();

            // Skapa handlern och injicera mockade repositoryn
            _handler = new AddBookCommandHandler(_bookRepository, _authorRepository);
        }

        [Test]
        public async Task Handle_ShouldAddBook_WhenValidCommand()
        {
            // Arrange
            var command = new AddBookCommand("Book Name", "Book Title", "Book Description", 1);
            var author = new Author { Id = 1, Name = "Author Name" };
            var book = new Book { Id = 1, Name = "Book Name", Title = "Book Title", Description = "Book Description", AuthorId = 1 };

            // Mocka författarrepositoryt för att returnera en framgångsrik operation
            A.CallTo(() => _authorRepository.GetAuthorById(command.AuthorId))
                .Returns(OperationResult<Author>.Success(author));

            // Mocka bokrepositoryt för att returnera en framgångsrik operation
            A.CallTo(() => _bookRepository.AddBook(A<Book>._))
                .Returns(OperationResult<Book>.Success(book));

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            result.Should().BeEquivalentTo(book);
        }
    }
}
