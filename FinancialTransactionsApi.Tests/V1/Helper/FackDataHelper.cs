using FinancialTransactionsApi.V1.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using FinancialTransactionsApi.V1.Boundary.Request;
using FinancialTransactionsApi.V1.Boundary.Response;
using FinancialTransactionsApi.V1.Infrastructure;
using Amazon.DynamoDBv2.Model;
using AutoFixture;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FinancialTransactionsApi.Tests.V1.Helper
{
    public static class FakeDataHelper
    {
        private static readonly Fixture _fixture = new Fixture();

        public static QueryResponse MockQueryResponse<T>(int cnt=1)
        {
            QueryResponse response = new QueryResponse();
            if (typeof(T) == typeof(Transaction))
            {
                for (int i = 0; i < cnt; i++)
                {
                    response.Items.Add(
                        new Dictionary<string, AttributeValue>()
                        {
                            {"id", new AttributeValue {S = _fixture.Create<Guid>().ToString()}},
                            {"address", new AttributeValue {S = _fixture.Create<string>().ToString()}},
                            {"balance_amount", new AttributeValue {N = _fixture.Create<decimal>().ToString("F")}},
                            {"bank_account_number", new AttributeValue {S = _fixture.Create<string>().ToString()}},
                            {"charged_amount", new AttributeValue {N = _fixture.Create<decimal>().ToString("F")}},
                            {"financial_month", new AttributeValue {N = _fixture.Create<int>().ToString()}},
                            {"financial_year", new AttributeValue {N = _fixture.Create<int>().ToString()}},
                            {"fund", new AttributeValue {S = _fixture.Create<string>()}},
                            {
                                "housing_benefit_amount",
                                new AttributeValue {N = _fixture.Create<decimal>().ToString("F")}
                            },
                            {"is_suspense", new AttributeValue {S = _fixture.Create<bool>().ToString()}},// GSI must be string or binary, so the string is implemented instead of boolean
                            {"paid_amount", new AttributeValue {N = _fixture.Create<decimal>().ToString("F")}},
                            {"payment_reference", new AttributeValue {S = _fixture.Create<string>()}},
                            {"period_no", new AttributeValue {N = _fixture.Create<int>().ToString()}},
                            {"target_id", new AttributeValue {S = _fixture.Create<Guid>().ToString()}},
                            {
                                "transaction_amount",
                                new AttributeValue {N = _fixture.Create<decimal>().ToString("F")}
                            },
                            {
                                "transaction_date",
                                new AttributeValue {S = _fixture.Create<DateTime>().ToString("F")}
                            },
                            {"transaction_source", new AttributeValue {S = _fixture.Create<string>()}},
                            {
                                "transaction_type",
                                new AttributeValue {S = _fixture.Create<TransactionType>().ToString()}
                            },
                            {
                                "person",
                                new AttributeValue
                                {
                                    M = new Dictionary<string, AttributeValue>
                                    {
                                        {"id", new AttributeValue {S = _fixture.Create<Guid>().ToString()}},
                                        {"fullName", new AttributeValue {S = _fixture.Create<string>()}}
                                    }
                                }
                            },
                            {
                                "suspense_resolution_info",
                                new AttributeValue
                                {
                                    M = new Dictionary<string, AttributeValue>
                                    {
                                        {
                                            "isConfirmed",
                                            new AttributeValue {BOOL = _fixture.Create<bool>()}
                                        },
                                        {
                                            "isApproved",
                                            new AttributeValue {BOOL = _fixture.Create<bool>()}
                                        },
                                        {"note", new AttributeValue {S = _fixture.Create<string>()}},
                                        {
                                            "resolutionDate",
                                            new AttributeValue {S = _fixture.Create<DateTime>().ToString("F")}
                                        }
                                    }
                                }
                            }
                        });
                }
            }
            return response;
        }

        public static QueryResponse MockQueryResponseWithoutConsolidatedCharges<T>()
        {
            QueryResponse response = new QueryResponse();
            return response;
        }
    }
}