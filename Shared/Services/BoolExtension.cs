

namespace Shared.Services
{
    public static class BoolExtension
    {
        //bool TryParseBool();
        public static string[] values = { "true", "false", "yes", "no", "1", "0", "y", "n" };
        public static string ParseBool(string value)
        {
            value = value == null ? "" : value.Trim();
            var val = values.FirstOrDefault(i => i == value.ToLower());
            if (val != null)
            {
                return val.Trim().ToString();
            }
            return "";
        }
        public static bool TryParseBool(string value)
        {
            value = value == null ? "" : value.Trim();
            var val = values.FirstOrDefault(i => i == value.ToLower());
            if (val != null)
            {
                return true;
            }
            return false;
        }
    }
}
