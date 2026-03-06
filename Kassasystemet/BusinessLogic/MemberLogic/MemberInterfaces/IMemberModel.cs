using System;
using System.Collections.Generic;
using System.Text;

namespace LinasKlubbLivs.BusinessLogic.MemberLogic.MemberInterfaces
{
    public interface IMemberModel
    {
        int MemberIdNumber { get; }
        string MemberFirstName { get; }
        string MemberLastName { get; }
        string MemberFullName { get; }
    }
}
