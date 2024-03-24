using FluentValidation;
using PaymentPicPay.API.Data.Context;
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

            #region Transaction
            app.MapPost("v1/api/transaction",
                async
                (TransactionViewModel viewModel,
                IValidator<Transaction> Validator,
                ITransferAuthorizerService authorizationService,
                IEmailSendService emailSendService,
                PaymentDataContext context) =>
                {
                    try
                    {
                        if (viewModel == null)
                            return Results.BadRequest("The transaction data is invalid.");

                        #region Buscar usuarios

                        User userSend = context.Customers.FirstOrDefault(send => send.Id == viewModel.SendId);

                        User userReceived = null;
                        switch (viewModel.TransactionType)
                        {
                            case ETransactionType.B2B:
                                userReceived = context.Customers.FirstOrDefault(received => received.Id == viewModel.ReceiveId);
                                break;
                            case ETransactionType.B2C:
                                userReceived = context.Merchants.FirstOrDefault(received => received.Id == viewModel.ReceiveId);
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

                        await context.SaveChangesAsync();

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
