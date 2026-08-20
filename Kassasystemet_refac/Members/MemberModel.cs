using System;
using System.Collections.Generic;
using System.Text;
using static Kassasystemet_refac.SearchMember;

namespace Kassasystemet_refac
{
    public class MemberModel : IMemberModel
    {
        public int MemberIdNumber { get; }
        public string MemberFirstName { get; }
        public string MemberLastName { get; }
        public string MemberFullName => $"{MemberFirstName} {MemberLastName}";

        public MemberModel(int memberIdNumber, string memberFirstName, string memberLastName)
        {
            MemberIdNumber = memberIdNumber;
            MemberFirstName = memberFirstName;
            MemberLastName = memberLastName;
        }
    }
}
