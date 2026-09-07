using System.Collections.Concurrent;
using LibraryManagement.Domain.Books;

namespace LibraryManagement.Infrastructure;

public class InMemoryBookRepository : IBookRepository
{
    private readonly ConcurrentDictionary<BookId, Book> _books = new();

    public Task<Book?> GetByIdAsync(BookId id)
    {
        _books.TryGetValue(id, out var book);
        return Task.FromResult(book);
    }

    public Task AddAsync(Book book)
    {
        _books[book.Id] = book;
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<Book>> GetAllAsync()
    {
        return Task.FromResult((IReadOnlyList<Book>)_books.Values.ToList());
    }

    public Task UpdateAsync(Book book)
    {
        _books[book.Id] = book;
        return Task.CompletedTask;
    }
}