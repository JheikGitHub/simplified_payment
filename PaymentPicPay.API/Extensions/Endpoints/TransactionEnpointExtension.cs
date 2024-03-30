using FluentValidation;
using PaymentPicPay.API.Data.Repositories._RepositoryWrapper;
using PaymentPicPay.API.Domain.Enums;
using PaymentPicPay.API.Domain.Models;
using PaymentPicPay.API.Services.Externals.EmailSendService;
using PaymentPicPay.API.Services.Externals.TransferAuthorizer;
using PaymentPicPay.API.Services.ViewModels.Transactions;

namespace PaymentPicPay.API.Extensions.Endpoints
{
    public static class TransactionEnpointExtension
    {
        public static WebApplication UseTransactionEnpoints(this WebApplication app)
        {

            #region GetById Transaction

            app.MapGet(
                "v1/api/transaction/{id:int}",
                async
                (int id,
                IRepositoryWrapper repository) =>
                {
                    try
                    {
                        var transaction = await repository.TransactionRepository.GetAsync(id, true);

                        return Results.Ok(transaction);
                    }
                    catch (Exception e)
                    {
                        return Results.Problem(
                            e.Message,
                            statusCode: StatusCodes.Status500InternalServerError,
                            title: "Error in get transaction by id");
                    }

                }).WithName("GetTransactionById")
                .WithOpenApi(options =>
                {
                    options.Description = "Get transaction by id.";
                    options.Summary = "Get transaction by id.";
                    return options;
                })
                .Produces<IEnumerable<TransactionViewModel>>(statusCode: 200)
                .Produces(statusCode: 500); ;

            #endregion

            #region GetAll Transaction
            app.MapGet(
                "v1/api/transactions",
                async
                (IRepositoryWrapper repository) =>
                {
                    try
                    {
                        var transactions = await repository.TransactionRepository.GetAllIncludes();

                        return Results.Ok(transactions);
                    }
                    catch (Exception e)
                    {
                        return Results.Problem(
                            e.Message,
                            statusCode: StatusCodes.Status500InternalServerError,
                            title: "Error in get all transactions");
                    }
                }).WithName("GetTransactions")
                .WithOpenApi(options =>
                {
                    options.Description = "Get all transactions.";
                    options.Summary = "Get all transactions.";
                    return options;
                })
                .Produces<IEnumerable<TransactionViewModel>>(statusCode: 200)
                .Produces(statusCode: 500); ;
            #endregion

            #region POST Transaction
            _ = app.MapPost("v1/api/transaction",
                async
                (CreateTransactionViewModel transactionViewModel,
                ITransferAuthorizerService authorizationService,
                IEmailSendService emailSendService,
                IRepositoryWrapper repository) =>
                {
                    try
                    {
                        if (transactionViewModel == null)
                            return Results.BadRequest("The transaction data is invalid.");

                        #region Buscar usuarios e cria transação

                        Customer userSend = await repository.CustomerRepository.GetAsync(transactionViewModel.SendId, false);

                        Merchant merchantReceived = null;
                        Customer customerReceived = null;
                        Transaction transaction = null;

                        if (transactionViewModel.TransactionType == ETransactionType.B2B)
                        {
                            if (transactionViewModel.SendId == transactionViewModel.ReceiveId)
                                return Results.BadRequest("Invalid transaction, it is not possible to transfer value to the same customer.");

                            customerReceived = await repository.CustomerRepository.GetAsync(transactionViewModel.ReceiveId, false);

                            transaction = new TransactionB2B(userSend, customerReceived, transactionViewModel.Amount);

                        }
                        else if (transactionViewModel.TransactionType == ETransactionType.B2C)
                        {
                            merchantReceived = await repository.MerchantRepository.GetAsync(transactionViewModel.ReceiveId, false);

                            transaction = new TransactionB2C(userSend, merchantReceived, transactionViewModel.Amount);
                        }

                        #endregion

                        #region Valida os dados da transação
                        transaction.IsValid();

                        if (!transaction.IsValid())
                            return Results.ValidationProblem(transaction.Validations.ToDictionary());

                        #endregion

                        #region Autenticador externo
                        //var authorizationTransfer = await authorizationService.AuthorizationTranfer();
                        var authorizationTransfer = true;

                        if (!authorizationTransfer)
                            return Results.Problem(
                                detail: "Error when making the transaction.",
                                statusCode: StatusCodes.Status500InternalServerError,
                                title: "External authorizer");

                        #endregion

                        #region Realizar a transação

                        transaction.Transfer();

                        if (!transaction.IsValid())
                            return Results.ValidationProblem(transaction.Validations.ToDictionary());
                        #endregion

                        #region Notificação de recebimento
                            if (transactionViewModel.TransactionType == ETransactionType.B2B)
                                await emailSendService.SendNotificationTransaction(customerReceived);
                            else if (transactionViewModel.TransactionType == ETransactionType.B2C)
                                await emailSendService.SendNotificationTransaction(merchantReceived);

                        #endregion

                        #region Salva a transação
                        await repository.TransactionRepository.AddAsync(transaction);

                        await repository.SaveChangesAsync();
                        #endregion

                        return Results.Created(
                            $"v1/api/transaction/{transaction.Id}",
                            transaction);
                    }
                    catch (Exception e)
                    {
                        return Results.Problem(
                            detail: e.Message,
                            statusCode: StatusCodes.Status500InternalServerError,
                            title: "Error when making the transaction.");
                    }
                })
                .Produces<IEnumerable<CreateTransactionViewModel>>(statusCode: 200)
                .Produces(statusCode: 500); ; ;
            #endregion

            return app;
        }
    }
}
