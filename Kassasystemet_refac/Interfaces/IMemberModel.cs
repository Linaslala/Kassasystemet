namespace Kassasystemet_refac
{
    public interface IMemberModel
    {
        int MemberIdNumber { get; }
        string MemberFirstName { get; }
        string MemberLastName { get; }
        string MemberFullName { get; }
    }
}
