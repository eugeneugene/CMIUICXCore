using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace CMIUICXCore.Code
{
    /// <summary>
    /// Defines the <see cref="EnumExtensions" />.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// The GetAttribute.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="value">The value<see cref="Enum"/>.</param>
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return attributes.Length > 0
              ? (T)attributes[0]
              : null;
        }

        /// <summary>
        /// The EnumMember.
        /// </summary>
        /// <param name="value">The value<see cref="Enum"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string EnumMember(this Enum value)
        {
            if (value == null)
                return null;

            var attribute = value.GetAttribute<EnumMemberAttribute>();
            return attribute == null ? value.ToString() : attribute.Value;
        }

        /// <summary>
        /// The FindByEnumMember.
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="enumMember">The enumMember<see cref="string"/>.</param>
        public static T FindByEnumMember<T>(string enumMember) where T : Enum
        {
            if (enumMember == null)
                return default;

            foreach (Enum value in typeof(T).GetEnumValues())
            {
                if (value.EnumMember() == enumMember)
                    return (T)value;
            }
            return default;
        }

        /// <summary>
        /// The XmlEnum.
        /// </summary>
        /// <param name="value">The value<see cref="Enum"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string XmlEnum(this Enum value)
        {
            if (value == null)
                return null;

            var attribute = value.GetAttribute<XmlEnumAttribute>();
            return attribute == null ? value.ToString() : attribute.Name;
        }

        /// <summary>
        /// The DisplayEnum.
        /// </summary>
        /// <param name="value">The value<see cref="Enum"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string DisplayEnum(this Enum value)
        {
            if (value == null)
                return null;

            var attribute = value.GetAttribute<DisplayAttribute>();
            return attribute == null ? value.ToString() : attribute.GetDescription();
        }
    }
}
