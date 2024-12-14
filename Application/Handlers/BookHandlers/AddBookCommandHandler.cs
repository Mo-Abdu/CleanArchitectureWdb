using Application.Commands.BookCommands;
using Application.Interfaces.RepositoryInterfaces;
using MediatR;
using Models;
namespace Application.Handlers.BookHandlers
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand, Book>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;

        public AddBookCommandHandler(IBookRepository bookRepository, IAuthorRepository authorRepository)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository), "Book repository cannot be null.");
            _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository), "Author repository cannot be null.");
        }

        public async Task<Book> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null.");
            }

            var authorResult = await _authorRepository.GetAuthorById(request.AuthorId);
            if (!authorResult.IsSuccess || authorResult.Data == null)
            {
                throw new KeyNotFoundException($"Author with ID {request.AuthorId} not found.");
            }

            var author = authorResult.Data; 

            var newBook = new Book
            {
                Name = request.Name,
                Title = request.Title,
                Description = request.Description,
                Author = author
            };

            var result = await _bookRepository.AddBook(newBook);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException(result.ErrorMessage ?? "Failed to add the book to the repository.");
            }

            return result.Data; 

        }
    }
}
