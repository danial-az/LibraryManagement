using LibraryManagement.Domain.Books;
using LibraryManagement.Domain.Members;

namespace LibraryManagement.Domain.Loans;

public interface ILoanRepository
{
    Task<Loan?> GetByIdAsync(LoanId id);
    Task<IReadOnlyList<Loan>> GetActiveLoansByMemberAsync(MemberId memberId);
    Task AddAsync(Loan loan);
    Task UpdateAsync(Loan loan);
    Task<IReadOnlyList<Loan>> GetAllAsync();
}