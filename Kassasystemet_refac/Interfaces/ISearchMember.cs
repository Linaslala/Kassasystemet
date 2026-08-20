namespace Kassasystemet_refac
{
    public interface ISearchMember
    {
        List<IMemberModel> Search(string searchMemberText);
    }
}
