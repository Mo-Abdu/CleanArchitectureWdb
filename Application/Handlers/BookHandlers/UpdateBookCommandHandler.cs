using Application.Commands.BookCommands;
using Application.Interfaces.RepositoryInterfaces;
using MediatR;
using Models;
namespace Application.Handlers.BookHandlers
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Book> 
    {
        private readonly IBookRepository _bookRepository;

        public UpdateBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository), "Book repository cannot be null.");
        }

        public async Task<Book> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null.");
            }

            var operationResult = await _bookRepository.GetBookById(request.Id);
            if (!operationResult.IsSuccess)
            {
                throw new KeyNotFoundException($"Book with ID {request.Id} not found.");
            }

            var book = operationResult.Data; 

            book.Name = request.Name ?? book.Name; 
            book.Title = request.Title ?? book.Title; 
            book.Description = request.Description ?? book.Description; 

            var updateResult = await _bookRepository.UpdateBook(request.Id, book);

            if (!updateResult.IsSuccess)
            {
                throw new InvalidOperationException("Failed to update the book.");
            }

            return updateResult.Data;  

        }
    }
       
}
