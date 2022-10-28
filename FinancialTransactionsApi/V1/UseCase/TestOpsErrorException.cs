using System;
using System.Runtime.Serialization;

namespace FinancialTransactionsApi.V1.UseCase
{
    [Serializable]
    public class TestOpsErrorException : Exception
    {
        public TestOpsErrorException() : base("This is a test exception to test our integrations") { }

        protected TestOpsErrorException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base("This is a test exception to test our integrations") { }
    }
}
