using System.Collections.Concurrent;
using LibraryManagement.Domain.Loans;
using LibraryManagement.Domain.Members;

namespace LibraryManagement.Infrastructure;

public class InMemoryLoanRepository : ILoanRepository
{
    private readonly ConcurrentDictionary<LoanId, Loan> _loans = new();

    public Task<Loan?> GetByIdAsync(LoanId id)
    {
        _loans.TryGetValue(id, out var loan);
        return Task.FromResult(loan);
    }

    public Task<IReadOnlyList<Loan>> GetActiveLoansByMemberAsync(MemberId memberId)
    {
        var result = _loans.Values
            .Where(l => l.MemberId.Equals(memberId) && l.Status != LoanStatus.Returned)
            .ToList();
        return Task.FromResult((IReadOnlyList<Loan>)result);
    }

    public Task AddAsync(Loan loan)
    {
        _loans[loan.Id] = loan;
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Loan loan)
    {
        _loans[loan.Id] = loan;
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<Loan>> GetAllAsync()
    {
        return Task.FromResult((IReadOnlyList<Loan>)_loans.Values.ToList());
    }
}