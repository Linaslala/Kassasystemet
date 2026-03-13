using LinasKlubbLivs.BusinessLogic.MemberLogic.MemberInterfaces;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinasKlubbLivs.BusinessLogic.MemberLogic.MemberDataManager
{
    /// <summary>
    /// Sparar medlemsdata till fil.
    /// 
/// </summary>
    public class SaveMemberToFile : ISaveMemberToFile
    {
        public void SaveAll(List<IMemberModel> members)
        {
            string filePath = MemberFilePath.MembersPath;

            var memberDirectory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrWhiteSpace(memberDirectory) && !Directory.Exists(memberDirectory))
                Directory.CreateDirectory(memberDirectory);

            using var writer = new StreamWriter(filePath, false);

            foreach (var member in members)
            {
                writer.WriteLine($"{member.MemberIdNumber};{member.MemberFirstName};{member.MemberLastName}");
            }
        }
    }
}
