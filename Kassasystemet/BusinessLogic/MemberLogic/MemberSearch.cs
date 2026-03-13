using LinasKlubbLivs.BusinessLogic.MemberLogic.MemberInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinasKlubbLivs.BusinessLogic.MemberLogic
{
    /// <summary>
    /// Sökmotor för medlemmar.
    /// 
    /// Stödjer sökning på medlemsnummer och namn.
    /// Tom sökning (entertryckning) returnerar alla medlemmar.
    /// </summary>
    public class MemberSearch : ISearchMember
    {
        private readonly IReadAllMembersFromFile _reader;

        public MemberSearch(IReadAllMembersFromFile reader)
        {
            _reader = reader;
        }

        public List<IMemberModel> Search(string searchMemberText)
        {
            var allMembers = _reader.ReadAll();

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