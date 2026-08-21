namespace Kassasystemet_refac
{
    public class MemberSearch : ISearchMember
    {
        private readonly IReadAllMembersFromFile _memberReader;

        public MemberSearch(IReadAllMembersFromFile memberReader)
        {
            _memberReader = memberReader;
        }

        public List<IMemberModel> Search(string searchMemberText)
        {
            var allMembers = _memberReader.ReadAll();

            if (string.IsNullOrWhiteSpace(searchMemberText))
                return allMembers;

            string userQuery = searchMemberText.Trim().ToLowerInvariant();

            return allMembers
                .Where(m =>
                {
                    string firstName = (m.MemberFirstName ?? "").ToLowerInvariant();
                    string lastName = (m.MemberLastName ?? "").ToLowerInvariant();
                    string fullName = (m.MemberFullName ?? "").ToLowerInvariant();

                    return m.MemberIdNumber.ToString().Contains(userQuery)
                           || firstName.Contains(userQuery)
                           || lastName.Contains(userQuery)
                           || fullName.Contains(userQuery);
                })
                .ToList();
        }
    }
}
