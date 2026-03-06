using System;
using System.Collections.Generic;
using System.Text;

namespace LinasKlubbLivs.BusinessLogic.MemberLogic.MemberInterfaces
{
    public interface ISearchMember
    {
        List<IMemberModel> Search(string searchMemberText);
    }
}
