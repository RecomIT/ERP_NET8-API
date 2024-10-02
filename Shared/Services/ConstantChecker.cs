namespace Shared.Services
{
    public static class ConstantChecker
    {
        public static string[] Genders = new string[] { "Male", "Female" };

        public static string[] Religions = new string[] { "ISLAM", "HINDU", "CHRISTIAN", "BUDDIST" };

        public static string[] BloodGroups = new string[] { "A+", "A-", "B+", "B-", "O+", "O-", "AB+", "AB-" };
        public static bool IsValid(string type, string value)
        {
            if (type == "Job Type")
            {
                var isExist = Jobtype.Jobtypes.FirstOrDefault(i => i.ToLower() == value.ToLower());
                if (isExist == null)
                {
                    return false;
                }
                return true;
            }
            if (type == "Gender")
            {
                var isExist = Genders.FirstOrDefault(i => i.ToLower() == value.ToLower());
                if (isExist == null)
                {
                    return false;
                }
                return true;
            }
            if (type == "Religion")
            {
                var isExist = Religions.FirstOrDefault(i => i.ToLower() == value.ToLower());
                if (isExist == null)
                {
                    return false;
                }
                return true;
            }
            if (type == "Blood Group")
            {
                var isExist = BloodGroups.FirstOrDefault(i => i.ToLower() == value.ToLower());
                if (isExist == null)
                {
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}
