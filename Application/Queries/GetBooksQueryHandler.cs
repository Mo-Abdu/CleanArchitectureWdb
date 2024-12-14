
using Application.Interfaces.RepositoryInterfaces;
using MediatR;
using Models;
namespace Application.Queries
{
    public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, List<Book>>
    {
        private readonly IBookRepository _bookRepository;


        public GetBooksQueryHandler(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository), "Book repository cannot be null.");
        }

        public async Task<List<Book>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            var booksResult = await _bookRepository.GetAllBooks();

            if (!booksResult.IsSuccess || booksResult.Data == null || booksResult.Data.Count == 0)
            {
                return new List<Book>(); 
            }

            return booksResult.Data; 
                                     
        }

    }
}