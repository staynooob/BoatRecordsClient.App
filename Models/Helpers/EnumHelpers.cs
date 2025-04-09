using System.ComponentModel;
using System.Reflection;

namespace BoatRecords.Models.Helpers;

static class EnumHelpers
{
    public static string ToStringEnums(Enum enm) {
        Type type = enm.GetType();

        MemberInfo[] memInfo = type.GetMember(enm.ToString());
        if (memInfo != null && memInfo.Length > 0)
        {
            object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attrs != null && attrs.Length > 0)
                return ((DescriptionAttribute)attrs[0]).Description;
        }
        return enm.ToString();
    }
}
