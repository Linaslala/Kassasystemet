using LinasKlubbLivs.BusinessLogic.MemberLogic.MemberInterfaces;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinasKlubbLivs.BusinessLogic.MemberLogic.MemberDataManager
{
    /// <summary>
    /// Läser alla medlemmar från fil och återskapar medlemsobjekt.
    /// </summary>
    public class ReadAllMembersFromFile : IReadAllMembersFromFile
    {
        public List<IMemberModel> ReadAll()
        {
            var members = new List<IMemberModel>();

            string filePath = MemberFilePath.MembersPath;

            if (!File.Exists(filePath))
                return members;

            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                var parts = line.Split(';');
                if (parts.Length != 3) continue;

                if (int.TryParse(parts[0], out int id))
                {
                    members.Add(new MemberModel(id, parts[1], parts[2]));
                }
            }

            return members;
        }
    }
}
