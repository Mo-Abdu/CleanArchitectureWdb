
using Application.Interfaces.RepositoryInterfaces;
using MediatR;
using Models;
namespace Application.Queries
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, Author>
    {
        private readonly IAuthorRepository _authorRepository;

        public GetAuthorByIdQueryHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository), "Author repository cannot be null.");
        }

        public async Task<Author> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null.");
            }

            var authorResult = await _authorRepository.GetAuthorById(request.Id);

            if (!authorResult.IsSuccess)
            {
                throw new KeyNotFoundException(authorResult.ErrorMessage ?? $"Author with ID {request.Id} was not found.");
            }

            return authorResult.Data;
        }
    }      
    
}
