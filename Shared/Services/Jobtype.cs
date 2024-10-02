namespace Shared.Services
{
    public static class Jobtype
    {
        public static string Permanent = "Permanent";
        public static string Contractual = "Contractual";
        public static string Freelancer = "Freelancer";
        public static string Intern = "Intern";
        public static string Probation = "Probation";
        public static string[] Jobtypes = new string[] { "Permanent", "Contractual", "Freelancer", "Intern", "Probation" };
        public static bool IsValid(string jobtype)
        {
            if (Utility.IsNullEmptyOrWhiteSpace(jobtype) == false)
            {
                var j = jobtype.ReplaceWhitespace();

                if (j.ToLower() == Jobtype.Permanent.ToLower())
                {
                    return true;
                }
                if (j.ToLower() == Jobtype.Contractual.ToLower())
                {
                    return true;
                }
                if (j.ToLower() == Jobtype.Freelancer.ToLower())
                {
                    return true;
                }
                if (j.ToLower() == Jobtype.Intern.ToLower())
                {
                    return true;
                }
                if (j.ToLower() == Jobtype.Probation.ToLower())
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
