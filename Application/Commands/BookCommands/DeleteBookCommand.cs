using MediatR;
namespace Application.Commands.BookCommands
{
    public class DeleteBookCommand : IRequest<bool>
    {
        public int BookId { get; set; } 

        public DeleteBookCommand(int bookId)
        {
            BookId = bookId; 
        }
    }
}
