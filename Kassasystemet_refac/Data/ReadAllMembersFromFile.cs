namespace Kassasystemet_refac
{
    public class ReadAllMembersFromFile : IReadAllMembersFromFile
    {
        public List<IMemberModel> ReadAll()
        {
            var members = new List<IMemberModel>();

            string filePath = MemberFilePath.MembersPath;

            //if (!File.Exists(filePath))
            //    return members;

            //var lines = File.ReadAllLines(filePath);

            var lines = FileSystemHelper.ReadLinesIfExists(filePath);

            foreach (var line in lines)
            {
                var parts = line.Split(';');
                if (parts.Length != 3) continue;

                if (int.TryParse(parts[0], out int memberId))
                {
                    members.Add(new MemberModel(memberId, parts[1], parts[2]));
                }
            }
            return members;
        }
    }
}
