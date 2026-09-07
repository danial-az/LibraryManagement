namespace LibraryManagement.Domain.Members;

public interface IMemberRepository
{
    Task<Member?> GetByIdAsync(MemberId id);
    Task AddAsync(Member member);
    Task<IReadOnlyList<Member>> GetAllAsync();
    Task UpdateAsync(Member member);
}