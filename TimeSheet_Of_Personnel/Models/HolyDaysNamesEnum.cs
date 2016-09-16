using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace TimeSheet_Of_Personnel.Models
{
    // [Table("HR_HolyDaysNames")]
    public enum HolyDaysNamesEnum
    {
        [Description("субота")]
        субота,
        [Description("неділя")]
        неділя,
        [Description("Новий рік")]
        Новий_рік,
        [Description("Різдво Христове")]
        Різдво_Христове,
        [Description("Міжнародний жіночий день")]
        Міжнародний_жіночий_день,
        [Description("День міжнар. солідарності трудящих")]
        День_міжнарод_солідарності_трудящих,
        [Description("День перемоги у 2-й світовій війні")]
        День_перемоги_у_2й_світовій_війні,
        [Description("День Конституції України")]
        День_Конституції_України,
        [Description("День незалежності України")]
        День_незалежності_України,
        [Description("День захисника України")]
        День_захисника_України,
        [Description("Пасха (Великдень)")]
        Пасха_Великдень,
        [Description("Трійця")]
        Трійця
    }


    // TODO:

    // IMPLEMENT ATTRIBUTE_DESCRIPTION FOR ENUM !!!
    // IMPLEMENT ATTRIBUTE_DESCRIPTION FOR ENUM !!!
    // IMPLEMENT ATTRIBUTE_DESCRIPTION FOR ENUM !!!
    // IMPLEMENT ATTRIBUTE_DESCRIPTION FOR ENUM !!!

    // using System;
    // using System.Reflection;
    public static class EnumHelper
    {
        /// <summary>
        /// Retrieve the description on the enum, e.g.
        /// [Description("Bright Pink")]
        /// BrightPink = 2,
        /// Then when you pass in the enum, it will retrieve the description
        /// </summary>
        /// <param name="en">The Enumeration</param>
        /// <returns>A string representing the friendly name</returns>
        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }
    }
}