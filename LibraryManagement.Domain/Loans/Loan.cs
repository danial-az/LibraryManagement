using LibraryManagement.Domain.Books;
using LibraryManagement.Domain.Common;
using LibraryManagement.Domain.Members;

namespace LibraryManagement.Domain.Loans;

public class Loan : Entity<LoanId>
{
    public BookId BookId { get; private set; }
    public MemberId MemberId { get; private set; }
    private DateTime BorrowedOn { get; set; }
    private DateTime DueDate { get; set; }
    public DateTime? ReturnedOn { get; private set; }
    public LoanStatus Status { get; private set; }
    public decimal Fine { get; private set; }

    private Loan(LoanId id, BookId bookId, MemberId memberId, DateTime borrowedOn, DateTime dueDate)
        : base(id)
    {
        BookId = bookId;
        MemberId = memberId;
        BorrowedOn = borrowedOn;
        DueDate = dueDate;
        Status = LoanStatus.Active;
        Fine = 0m;
    }

    public static Loan Open(BookId bookId, MemberId memberId, DateTime borrowedOn, int loanPeriodInDays)
    {
        if (loanPeriodInDays <= 0)
            throw new DomainException("دوره امانت باید بزرگ‌تر از صفر باشد.");

        var dueDate = borrowedOn.AddDays(loanPeriodInDays);
        return new Loan(LoanId.New(), bookId, memberId, borrowedOn, dueDate);
    }

    public void Close(DateTime returnDate, IFineCalculator fineCalculator)
    {
        if (Status == LoanStatus.Returned)
            throw new DomainException("این امانت قبلاً بازگردانده شده است.");

        if (returnDate < BorrowedOn)
            throw new DomainException("تاریخ بازگشت نمی‌تواند قبل از تاریخ امانت باشد.");

        ReturnedOn = returnDate;
        Fine = fineCalculator.CalculateFine(DueDate, returnDate);
        Status = LoanStatus.Returned;
    }
}