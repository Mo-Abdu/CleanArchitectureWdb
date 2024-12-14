using Application.Commands.BookCommands;
using Application.Interfaces.RepositoryInterfaces;
using MediatR;
namespace Application.Handlers.BookHandlers
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, bool>
    {
        private readonly IBookRepository _bookRepository;

        public DeleteBookCommandHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository), "Book repository cannot be null.");
        }

        public async Task<bool> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null.");
            }

            var result = await _bookRepository.DeleteBookById(request.BookId);

            if (!result.IsSuccess)
            {
                throw new InvalidOperationException($"Failed to delete book with ID {request.BookId}. Error: {result.ErrorMessage}");
            }

            return true; 

        }
    }
}
