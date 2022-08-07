using Ardalis.GuardClauses;
using Faith.Domain.Common;

namespace Faith.Domain.Users;

public class Username : ValueObject
{

    public string Firstname { get; set; }
    public string Lastname { get; set; }

    private Username()
    {

    }
    public Username(string firstname, string lastname)
    {
        Firstname = Guard.Against.NullOrEmpty(firstname, nameof(firstname));
        Lastname = Guard.Against.NullOrEmpty(lastname, nameof(lastname));
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Firstname.ToLower();
        yield return Lastname.ToLower();
    }
}