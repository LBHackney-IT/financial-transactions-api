// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:FinancialTransactionsApi.V1.ExceptionMiddleware.Invoke(Microsoft.AspNetCore.Http.HttpContext)~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "<Pending>", Scope = "member", Target = "~M:FinancialTransactionsApi.V1.Infrastructure.ValidationExtensions.HaveAllFieldsInAddTransactionModel(FinancialTransactionsApi.V1.Boundary.Request.AddTransactionRequest)~System.Boolean")]
[assembly: SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "<Pending>", Scope = "member", Target = "~M:FinancialTransactionsApi.V1.Infrastructure.ValidationExtensions.HaveAllFieldsInUpdateTransactionModel(FinancialTransactionsApi.V1.Boundary.Request.UpdateTransactionRequest)~System.Boolean")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>", Scope = "member", Target = "~M:FinancialTransactionsApi.V1.Controllers.BaseController.GetErrorMessage(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary)~System.String")]
[assembly: SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>", Scope = "member", Target = "~M:FinancialTransactionsApi.V1.Controllers.BaseController.GetCorrelationId~System.String")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>", Scope = "member", Target = "~M:FinancialTransactionsApi.Startup.Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder,Microsoft.AspNetCore.Hosting.IWebHostEnvironment)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>", Scope = "member", Target = "~M:FinancialTransactionsApi.V1.UseCase.AddUseCase.ExecuteAsync(FinancialTransactionsApi.V1.Boundary.Request.AddTransactionRequest)~System.Threading.Tasks.Task{FinancialTransactionsApi.V1.Boundary.Response.TransactionResponse}")]
[assembly: SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>", Scope = "member", Target = "~M:FinancialTransactionsApi.V1.UseCase.AddUseCase.ExecuteAsync(FinancialTransactionsApi.V1.Boundary.Request.AddTransactionRequest)~System.Threading.Tasks.Task{FinancialTransactionsApi.V1.Boundary.Response.TransactionResponse}")]
[assembly: SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "<Pending>", Scope = "member", Target = "~M:FinancialTransactionsApi.V1.Controllers.FinancialTransactionsApiController.GetAll(System.String,FinancialTransactionsApi.V1.Boundary.Request.TransactionQuery)~System.Threading.Tasks.Task{Microsoft.AspNetCore.Mvc.IActionResult}")]
[assembly: SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "<Pending>", Scope = "member", Target = "~M:FinancialTransactionsApi.V1.Controllers.FinancialTransactionsApiController.Add(System.String,System.String,FinancialTransactionsApi.V1.Boundary.Request.AddTransactionRequest)~System.Threading.Tasks.Task{Microsoft.AspNetCore.Mvc.IActionResult}")]
[assembly: SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "<Pending>", Scope = "member", Target = "~M:FinancialTransactionsApi.V1.Controllers.FinancialTransactionsApiController.Update(System.String,System.String,System.Guid,FinancialTransactionsApi.V1.Boundary.Request.UpdateTransactionRequest)~System.Threading.Tasks.Task{Microsoft.AspNetCore.Mvc.IActionResult}")]
[assembly: SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "<Pending>", Scope = "member", Target = "~M:FinancialTransactionsApi.V1.Controllers.FinancialTransactionsApiController.AddBatch(System.String,System.String,System.Collections.Generic.IEnumerable{FinancialTransactionsApi.V1.Boundary.Request.AddTransactionRequest})~System.Threading.Tasks.Task{Microsoft.AspNetCore.Mvc.IActionResult}")]
[assembly: SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "<Pending>", Scope = "member", Target = "~M:FinancialTransactionsApi.V1.Infrastructure.ValidationExtensions.HaveAllFieldsInAddWeeklyChargeModel(FinancialTransactionsApi.V1.Boundary.Request.AddTransactionRequest)~System.Boolean")]
[assembly: SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>", Scope = "member", Target = "~M:FinancialTransactionsApi.V1.Controllers.FinancialTransactionsApiController.GetTransactionList(FinancialTransactionsApi.V1.Boundary.Request.TransactionSearchRequest)~System.Threading.Tasks.Task{Microsoft.AspNetCore.Mvc.IActionResult}")]

[assembly: SuppressMessage("Usage", "CA1801:Review unused parameters", Justification = "<Pending>", Scope = "member", Target = "~M:FinancialTransactionsApi.V1.Controllers.FinancialTransactionsApiController.Get(System.String,System.Guid,System.Guid)~System.Threading.Tasks.Task{Microsoft.AspNetCore.Mvc.IActionResult}")]
[assembly: SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "<Pending>", Scope = "member", Target = "~M:FinancialTransactionsApi.V1.Infrastructure.ValidationExtensions.HaveAllFieldsInBatchProcessingModel(FinancialTransactionsApi.V1.Domain.Transaction)~System.Boolean")]
//[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:FinancialTransactionsApi.V1.Infrastructure.ElasticSearchExtensions.ConfigureElasticSearch(Microsoft.Extensions.DependencyInjection.IServiceCollection,Microsoft.Extensions.Configuration.IConfiguration)")]
//[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:FinancialTransactionsApi.V1.UseCase.ExportQuarterlyReportUseCase.WritePdfFile(System.Collections.Generic.List{FinancialTransactionsApi.V1.Domain.Transaction})~System.Byte[]")]
//[assembly: SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>", Scope = "member", Target = "~M:FinancialTransactionsApi.V1.UseCase.ExportQuarterlyReportUseCase.WritePdfFile(System.Collections.Generic.List{FinancialTransactionsApi.V1.Boundary.Response.ExportTransactionResponse})~System.Byte[]")]
