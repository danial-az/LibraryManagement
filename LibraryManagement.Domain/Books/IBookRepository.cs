namespace LibraryManagement.Domain.Books;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(BookId id);
    Task AddAsync(Book book);
    Task<IReadOnlyList<Book>> GetAllAsync();
    Task UpdateAsync(Book book);
}