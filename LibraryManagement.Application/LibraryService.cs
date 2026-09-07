using LibraryManagement.Domain.Books;
using LibraryManagement.Domain.Common;
using LibraryManagement.Domain.Loans;
using LibraryManagement.Domain.Members;

namespace LibraryManagement.Application;

public class LibraryService
{
    private readonly IBookRepository _bookRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly ILoanRepository _loanRepository;
    private readonly IFineCalculator _fineCalculator;

    private const int LoanPeriodInDays = 14;

    public LibraryService(
        IBookRepository bookRepository,
        IMemberRepository memberRepository,
        ILoanRepository loanRepository,
        IFineCalculator fineCalculator)
    {
        _bookRepository = bookRepository;
        _memberRepository = memberRepository;
        _loanRepository = loanRepository;
        _fineCalculator = fineCalculator;
    }

    public async Task<Book> AddBookAsync(string title, string author, int copies)
    {
        var book = Book.Create(title, author, copies);
        await _bookRepository.AddAsync(book);
        return book;
    }

    public async Task<Member> RegisterMemberAsync(string fullName, string email)
    {
        var member = Member.Register(fullName, Email.Create(email));
        await _memberRepository.AddAsync(member);
        return member;
    }

    public async Task<Loan> BorrowBookAsync(BookId bookId, MemberId memberId, DateTime borrowedOn)
    {
        var book = await _bookRepository.GetByIdAsync(bookId)
            ?? throw new DomainException("کتاب مورد نظر یافت نشد.");

        var member = await _memberRepository.GetByIdAsync(memberId)
            ?? throw new DomainException("عضو مورد نظر یافت نشد.");

        if (!book.HasAvailableCopy())
            throw new DomainException("در حال حاضر نسخه‌ای از این کتاب موجود نیست.");

        if (!member.CanBorrow())
            throw new DomainException("عضو مجاز به امانت گرفتن کتاب جدید نیست.");

        book.ReserveOneCopy();
        member.RegisterNewLoan();

        var loan = Loan.Open(bookId, memberId, borrowedOn, LoanPeriodInDays);

        await _bookRepository.UpdateAsync(book);
        await _memberRepository.UpdateAsync(member);
        await _loanRepository.AddAsync(loan);

        return loan;
    }

    public async Task<Loan> ReturnBookAsync(LoanId loanId, DateTime returnDate)
    {
        var loan = await _loanRepository.GetByIdAsync(loanId)
            ?? throw new DomainException("رکورد امانت یافت نشد.");

        var book = await _bookRepository.GetByIdAsync(loan.BookId)
            ?? throw new DomainException("کتاب مربوط به این امانت یافت نشد.");

        var member = await _memberRepository.GetByIdAsync(loan.MemberId)
            ?? throw new DomainException("عضو مربوط به این امانت یافت نشد.");

        loan.Close(returnDate, _fineCalculator);
        book.ReleaseOneCopy();
        member.RegisterReturnedLoan();

        await _loanRepository.UpdateAsync(loan);
        await _bookRepository.UpdateAsync(book);
        await _memberRepository.UpdateAsync(member);

        return loan;
    }
}