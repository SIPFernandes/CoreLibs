using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Helpers
{
    public static class UsersHelper
    {
        public static bool SearchMembers(string? firstName, string? lastName, string searchExpression)
        {
            var result = false;

            if (!string.IsNullOrEmpty(searchExpression))
            {
                if (firstName == null)
                {
                    firstName = string.Empty;
                }

                if (lastName == null)
                {
                    lastName = string.Empty;
                }

                var array = searchExpression.Trim().Split();

                if (array.Length == 2)
                {
                    result = firstName.Equals(array[0],
                        StringComparison.CurrentCultureIgnoreCase) &&
                        (string.IsNullOrEmpty(array[^1]) ||
                        lastName.Contains(array[^1],
                        StringComparison.CurrentCultureIgnoreCase));
                }
                else if (array.Length == 1)
                {
                    result = firstName.Contains(array[0],
                        StringComparison.CurrentCultureIgnoreCase) ||
                        lastName.Contains(array[0],
                        StringComparison.CurrentCultureIgnoreCase);
                }
            }

            return result;
        }
    }
}
