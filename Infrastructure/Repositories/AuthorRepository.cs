using Application.Interfaces.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using Models;
namespace Infrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly RealDatabase _context;

        public AuthorRepository(RealDatabase context)
        {
            _context = context;
        }

        public async Task<OperationResult<List<Author>>> GetAllAuthors()
        {
            var authors = await _context.Authors.ToListAsync();
            if (authors == null || authors.Count == 0)
            {
                return OperationResult<List<Author>>.Failure("No authors found.");
            }
            return OperationResult<List<Author>>.Success(authors);
        }

        public async Task<OperationResult<Author>> GetAuthorById(int id)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == id);
            if (author == null)
            {
                return OperationResult<Author>.Failure($"Author with ID {id} not found.");
            }
            return OperationResult<Author>.Success(author);
        }
        public async Task<OperationResult<Author>> AddAuthor(Author author)
        {
            if (author == null)
            {
                return OperationResult<Author>.Failure("Author cannot be null.");
            }

            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
            return OperationResult<Author>.Success(author);
        }

        public async Task<OperationResult<Author>> UpdateAuthor(int id, Author author)
        {
            if (author == null)
            {
                return OperationResult<Author>.Failure("Author cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(author.Name))
            {
                return OperationResult<Author>.Failure("Author name cannot be empty.");
            }

            var existingAuthor = await _context.Authors.FindAsync(id);
            if (existingAuthor == null)
            {
                return OperationResult<Author>.Failure($"Author with ID {id} not found.");
            }

            existingAuthor.Name = author.Name;
            await _context.SaveChangesAsync();

            return OperationResult<Author>.Success(existingAuthor);
        }

        public async Task<OperationResult<bool>> DeleteAuthorById(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return OperationResult<bool>.Failure($"Author with ID {id} not found.");
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return OperationResult<bool>.Success(true);
        }

    }
}
