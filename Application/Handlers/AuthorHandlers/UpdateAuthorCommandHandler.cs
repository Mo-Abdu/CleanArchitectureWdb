using Application.Commands.AuthorCommands;
using Application.Interfaces.RepositoryInterfaces;
using MediatR;
using Models;
namespace Application.Handlers.AuthorHandlers
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, Author>
    {
        private readonly IAuthorRepository _authorRepository;

        public UpdateAuthorCommandHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository), "Author repository cannot be null.");
        }

        public async Task<Author> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null.");
            }

            var operationResult = await _authorRepository.GetAuthorById(request.Id);

            if (!operationResult.IsSuccess || operationResult.Data == null)
            {
                throw new KeyNotFoundException($"Author with ID {request.Id} not found.");
            }

            var author = operationResult.Data; 

            author.Name = request.Name;

            var updateResult = await _authorRepository.UpdateAuthor(request.Id, author);

            if (!updateResult.IsSuccess)
            {
                throw new InvalidOperationException(updateResult.ErrorMessage);
            }

            return updateResult.Data; 

        }

    }
}
