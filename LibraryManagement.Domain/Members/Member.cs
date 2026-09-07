using LibraryManagement.Domain.Common;

namespace LibraryManagement.Domain.Members;

public class Member : Entity<MemberId>
{
    private const int MaxActiveLoans = 3;

    public string FullName { get; private set; } = string.Empty;
    public Email Email { get; private set; } = null!;
    public MembershipStatus Status { get; private set; }
    public int ActiveLoansCount { get; private set; }

    private Member(MemberId id, string fullName, Email email) : base(id)
    {
        FullName = fullName;
        Email = email;
        Status = MembershipStatus.Active;
        ActiveLoansCount = 0;
    }

    public static Member Register(string fullName, Email email)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("نام عضو الزامی است.");

        return new Member(MemberId.New(), fullName.Trim(), email);
    }

    public bool CanBorrow() =>
        Status == MembershipStatus.Active && ActiveLoansCount < MaxActiveLoans;

    public void RegisterNewLoan()
    {
        if (!CanBorrow())
            throw new DomainException("عضو مجاز به امانت گرفتن کتاب جدید نیست.");

        ActiveLoansCount++;
    }

    public void RegisterReturnedLoan()
    {
        if (ActiveLoansCount <= 0)
            throw new DomainException("عضو هیچ امانت فعالی ندارد.");

        ActiveLoansCount--;
    }

    public void Suspend() => Status = MembershipStatus.Suspended;
    public void Activate() => Status = MembershipStatus.Active;
}

