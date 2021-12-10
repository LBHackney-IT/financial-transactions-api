using System;
using System.ComponentModel;

namespace FinancialTransactionsApi.V1.Helpers
{
    public static class EnumHelper
    {
        public static string GetDescription<TEnum>(this TEnum enumValue)
            where TEnum : struct, IConvertible
        {
            if (!typeof(TEnum).IsEnum)
                return null;

            var description = enumValue.ToString();
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            if (fieldInfo != null)
            {
                var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    description = ((DescriptionAttribute) attrs[0]).Description;
                }
            }

            return description;
        }
    }
}
