using System.Text.RegularExpressions;
using LibraryManagement.Domain.Common;

namespace LibraryManagement.Domain.Members;

public sealed class Email : ValueObject
{
    private static readonly Regex Pattern =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public string Value { get; }

    private Email(string value) => Value = value;

    public static Email Create(string rawValue)
    {
        if (string.IsNullOrWhiteSpace(rawValue) || !Pattern.IsMatch(rawValue))
            throw new DomainException("ایمیل معتبر نیست.");

        return new Email(rawValue.Trim().ToLowerInvariant());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}