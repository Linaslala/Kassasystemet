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
            var all = _reader.ReadAll();

            if (string.IsNullOrWhiteSpace(searchMemberText))
                return all;

            string query = searchMemberText.Trim().ToLowerInvariant();

            return all
                .Where(m =>
                {
                    // Null-säker text
                    string first = (m.MemberFirstName ?? "").ToLowerInvariant();
                    string last = (m.MemberLastName ?? "").ToLowerInvariant();
                    string full = (m.MemberFullName ?? "").ToLowerInvariant();

                    return m.MemberIdNumber.ToString().Contains(query)
                           || first.Contains(query)
                           || last.Contains(query)
                           || full.Contains(query);
                })
                .ToList();
        }
    }
}