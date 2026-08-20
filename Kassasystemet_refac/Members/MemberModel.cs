using static Kassasystemet_refac.SearchMember;

namespace Kassasystemet_refac
{
    //POCO (Plain Old CLR Object)
    //Innehåller mest sata, lite logik och få beroenden
    //Därför bra att börja med tidigt i refaktoreringen!
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
