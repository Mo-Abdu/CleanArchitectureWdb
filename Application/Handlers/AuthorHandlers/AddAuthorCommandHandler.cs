using Application.Commands.AuthorCommands;
using Application.Interfaces.RepositoryInterfaces;
using MediatR;
using Models;
namespace Application.Handlers.AuthorHandlers
{
    public class AddAuthorCommandHandler : IRequestHandler<AddAuthorCommand, Author>
    {
        private readonly IAuthorRepository _authorRepository;

        public AddAuthorCommandHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository), "Author repository cannot be null.");
        }

        public async Task<Author> Handle(AddAuthorCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Author name cannot be null or empty.", nameof(request.Name));
            }

            var newAuthor = new Author { Name = request.Name };

            var operationResult = await _authorRepository.AddAuthor(newAuthor);

            if (!operationResult.IsSuccess)
            {
                throw new InvalidOperationException(operationResult.ErrorMessage);
            }

            return operationResult.Data;

        }
    }
}
