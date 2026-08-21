using static Kassasystemet_refac.SearchMemberMenu;

namespace Kassasystemet_refac
{
    public class SaveMemberToFile : ISaveMemberToFile
    {
        public void SaveAll(List<IMemberModel> members)
        {
            string filePath = MemberFilePath.MembersPath;

            var memberDirectory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrWhiteSpace(memberDirectory) && !Directory.Exists(memberDirectory))
                Directory.CreateDirectory(memberDirectory);

            using var memberWriter = new StreamWriter(filePath, false);

            foreach (var member in members)
            {
                memberWriter.WriteLine($"{member.MemberIdNumber};{member.MemberFirstName};{member.MemberLastName}");
            }
        }
    }
}
