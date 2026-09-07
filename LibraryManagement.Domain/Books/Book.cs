using LibraryManagement.Domain.Common;

namespace LibraryManagement.Domain.Books;

public class Book : Entity<BookId>
{
    public string Title { get; private set; } = string.Empty;
    public string Author { get; private set; } = string.Empty;
    public int TotalCopies { get; private set; }
    public int AvailableCopies { get; private set; }

    private Book(BookId id, string title, string author, int totalCopies) : base(id)
    {
        Title = title;
        Author = author;
        TotalCopies = totalCopies;
        AvailableCopies = totalCopies;
    }

    public static Book Create(string title, string author, int totalCopies)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("عنوان کتاب الزامی است.");

        if (string.IsNullOrWhiteSpace(author))
            throw new DomainException("نام نویسنده الزامی است.");

        if (totalCopies <= 0)
            throw new DomainException("تعداد نسخه‌ها باید بزرگ‌تر از صفر باشد.");

        return new Book(BookId.New(), title.Trim(), author.Trim(), totalCopies);
    }

    public void ReserveOneCopy()
    {
        if (AvailableCopies <= 0)
            throw new DomainException("نسخه‌ی در دسترسی از این کتاب وجود ندارد.");

        AvailableCopies--;
    }

    public void ReleaseOneCopy()
    {
        if (AvailableCopies >= TotalCopies)
            throw new DomainException("امکان بازگشت نسخه بیشتر از تعداد کل کتاب وجود ندارد.");

        AvailableCopies++;
    }

    public bool HasAvailableCopy() => AvailableCopies > 0;
    
}