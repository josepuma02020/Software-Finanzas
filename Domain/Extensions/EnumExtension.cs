using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Extensions
{
    public static class EnumExtension
    {
        private const string TYPE_IS_NOT_ENUM = "Type '{0}' is not Enum";
        private const int EMPTY_ARRAY = 0;
        private const int FIRST_ITEM = 0;

        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException(string.Format(TYPE_IS_NOT_ENUM, type));
            }

            System.Reflection.MemberInfo[] members = type.GetMember(value.ToString());
            if (members.Length == EMPTY_ARRAY)
            {
                return string.Empty;
            }

            var member = members.FirstOrDefault();

            if (member == null)
            {
                return string.Empty;
            }

            object[] attributes = member.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length == EMPTY_ARRAY)
            {
                return string.Empty;
            }

            var attribute = (DescriptionAttribute)attributes[FIRST_ITEM];
            return attribute.Description;
        }
    }
}
