using Application.Interfaces.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Models;
namespace Infrastructure.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly RealDatabase _context;

        public BookRepository(RealDatabase context)
        {
            _context = context;
        }

        public async Task<OperationResult<Book>> GetBookById(int id)
        {
            var book = await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
                return OperationResult<Book>.Failure("Book not found");

            return OperationResult<Book>.Success(book);
        }

        public async Task<OperationResult<List<Book>>> GetAllBooks()
        {
            var books = await _context.Books.Include(b => b.Author).ToListAsync();
            if (books == null || books.Count == 0)
                return OperationResult<List<Book>>.Failure("No books found");

            return OperationResult<List<Book>>.Success(books);
        }
        public async Task<OperationResult<Book>> AddBook(Book book)
        {
            try
            {
                _context.Books.Add(book);
                await _context.SaveChangesAsync();
                return OperationResult<Book>.Success(book);
            }
            catch (Exception ex)
            {
                return OperationResult<Book>.Failure($"Error adding book: {ex.Message}");
            }
        }

        public async Task<OperationResult<bool>> DeleteBookById(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book == null)
                return OperationResult<bool>.Failure("Book not found");

            try
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return OperationResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"Error deleting book: {ex.Message}");
            }
        }

        public async Task<OperationResult<Book>> UpdateBook(int id, Book book)
        {
            var existingBook = await _context.Books.FindAsync(id);
            if (existingBook == null)
                return OperationResult<Book>.Failure("Book not found");

            try
            {
                existingBook.Name = book.Name;
                existingBook.Title = book.Title;
                existingBook.Description = book.Description;
                existingBook.AuthorId = book.AuthorId;

                _context.Books.Update(existingBook);
                await _context.SaveChangesAsync();
                return OperationResult<Book>.Success(existingBook);
            }
            catch (Exception ex)
            {
                return OperationResult<Book>.Failure($"Error updating book: {ex.Message}");
            }
        }
    }
}
