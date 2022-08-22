using System.Text.RegularExpressions;

namespace StudentRecordMgntSystem
{
    class HelperFunctions
    {
        public bool IsEmpty(string str)
        {
            bool b = string.IsNullOrEmpty(str);
            return b;
        }
        public bool ValidateEmail(string email)
        {
            if (!this.IsEmpty(email))
            {
                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match match = regex.Match(email);
                return match.Success ? true : false;
            }
            return true;
        }
    }

}