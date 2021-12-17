using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;

namespace FinancialTransactionsApi.V1.Infrastructure.Conventers
{
    // TODO: This should go in a common NuGet package...

    /// <summary>
    /// Converter for enums where the value stored should be the enum value name (not the numeric value)
    /// </summary>
    public class DynamoDbEnumConverter<TEnum> : IPropertyConverter where TEnum : Enum
    {
        public DynamoDBEntry ToEntry(object value)
        {
            if (null == value)
            {
                return new DynamoDBNull();
            }

            return new Primitive(Enum.GetName(typeof(TEnum), value));
        }

        public object FromEntry(DynamoDBEntry entry)
        {
            var primitive = entry as Primitive;

            if (null == primitive)
            {
                return default(TEnum);
            }

            var valueAsEnum = (TEnum) Enum.Parse(typeof(TEnum), primitive.AsString());

            return valueAsEnum;
        }
    }
}
