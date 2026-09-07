using System.Collections.Concurrent;
using LibraryManagement.Domain.Members;

namespace LibraryManagement.Infrastructure;

public class InMemoryMemberRepository : IMemberRepository
{
    private readonly ConcurrentDictionary<MemberId, Member> _members = new();

    public Task<Member?> GetByIdAsync(MemberId id)
    {
        _members.TryGetValue(id, out var member);
        return Task.FromResult(member);
    }

    public Task AddAsync(Member member)
    {
        _members[member.Id] = member;
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<Member>> GetAllAsync()
    {
        return Task.FromResult((IReadOnlyList<Member>)_members.Values.ToList());
    }

    public Task UpdateAsync(Member member)
    {
        _members[member.Id] = member;
        return Task.CompletedTask;
    }
}