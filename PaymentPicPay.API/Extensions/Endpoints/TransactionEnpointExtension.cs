using FluentValidation;
using PaymentPicPay.API.Data.Repositories._RepositoryWrapper;
using PaymentPicPay.API.Domain.Enums;
using PaymentPicPay.API.Domain.Models;
using PaymentPicPay.API.Services.Externals.EmailSendService;
using PaymentPicPay.API.Services.Externals.TransferAuthorizer;
using PaymentPicPay.API.Services.ViewModels;

namespace PaymentPicPay.API.Extensions.Endpoints
{
    public static class TransactionEnpointExtension
    {
        public static WebApplication UseTransactionEnpoints(this WebApplication app)
        {

            #region GET Transaction
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
                });
            #endregion

            #region POST Transaction
            app.MapPost("v1/api/transaction",
                async
                (TransactionViewModel viewModel,
                IValidator<Transaction> Validator,
                ITransferAuthorizerService authorizationService,
                IEmailSendService emailSendService,
                IRepositoryWrapper repository) =>
                {
                    try
                    {
                        if (viewModel == null)
                            return Results.BadRequest("The transaction data is invalid.");

                        #region Buscar usuarios

                        User userSend = await repository.CustomerRepository.GetAsync(viewModel.SendId, false);

                        User userReceived = null;
                        switch (viewModel.TransactionType)
                        {
                            case ETransactionType.B2B:
                                userReceived = await repository.CustomerRepository.GetAsync(viewModel.ReceiveId, false);
                                break;
                            case ETransactionType.B2C:
                                userReceived = await repository.MerchantRepository.GetAsync(viewModel.ReceiveId, false);
                                break;

                        }

                        #endregion

                        #region Criar transação
                        Transaction transaction = new(
                            userSend.Id,
                            userReceived.Id,
                            viewModel.Amount,
                            viewModel.TransactionType);

                        #endregion

                        #region Validar os dados da transação
                        var result = Validator.Validate(transaction);

                        if (!result.IsValid)
                            return Results.ValidationProblem(result.ToDictionary());

                        var authorizationTransfer = await authorizationService.AuthorizationTranfer();

                        if (!authorizationTransfer)
                            return Results.Problem(
                                detail: "Error when making the transaction.",
                                statusCode: StatusCodes.Status500InternalServerError,
                                title: "External authorizer");

                        #endregion

                        #region Realizar a transação

                        var resultTransaction = transaction.Transfer(userSend, userReceived);

                        #endregion

                        #region Notificação de recebimento
                        if (resultTransaction)
                            await emailSendService.SendNotificationTransaction(userReceived);

                        #endregion

                        #region Salva a transação

                        await repository.TransactionRepository.AddAsync(transaction);

                        await repository.SaveChangesAsync();
                        #endregion

                        return Results.Ok();
                    }
                    catch (Exception e)
                    {
                        return Results.Problem(
                            detail: e.Message,
                            statusCode: StatusCodes.Status500InternalServerError,
                            title: "Error when making the transaction.");
                    }
                });
            #endregion

            return app;
        }
    }
}
