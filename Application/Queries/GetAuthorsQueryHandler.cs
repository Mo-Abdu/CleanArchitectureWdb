

using Application.Interfaces.RepositoryInterfaces;
using MediatR;
using Models;
namespace Application.Queries
{
    public class GetAuthorsQueryHandler : IRequestHandler<GetAuthorsQuery, List<Author>>
    {
        private readonly IAuthorRepository _authorRepository;
        public GetAuthorsQueryHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository), "Author repository cannot be null.");
        }

        public async Task<List<Author>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
        {
            var authorsResult = await _authorRepository.GetAllAuthors();

            if (!authorsResult.IsSuccess)
            {
                throw new InvalidOperationException(authorsResult.ErrorMessage ?? "An error occurred while fetching authors.");
            }

            return authorsResult.Data;
        }

    }
}
