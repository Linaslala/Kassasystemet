using LinasKlubbLivs.BusinessLogic.MemberLogic.MemberInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinasKlubbLivs.BusinessLogic.MemberLogic
{
    /// <summary>
    /// Representerar en klubbmedlem.
    /// 
    /// Innehåller information såsom medlemsnummer och namn.
    /// </summary>
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
