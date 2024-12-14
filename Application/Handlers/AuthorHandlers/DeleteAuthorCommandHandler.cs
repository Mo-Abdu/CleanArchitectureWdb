using Application.Commands.AuthorCommands;
using Application.Interfaces.RepositoryInterfaces;
using MediatR;
using Models;
namespace Application.Handlers.AuthorHandlers
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, bool>
    {
        private readonly IAuthorRepository _authorRepository;

        public DeleteAuthorCommandHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<bool> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetAuthorById(request.Id);

            if (author == null)
            {
                throw new KeyNotFoundException("Author not found.");
            }

            var operationResult = await _authorRepository.DeleteAuthorById(request.Id);

            if (!operationResult.IsSuccess)
            {
                throw new InvalidOperationException(operationResult.ErrorMessage);
            }

            return operationResult.Data; 

        }
    }
}
