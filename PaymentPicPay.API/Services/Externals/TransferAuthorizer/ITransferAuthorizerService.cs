namespace PaymentPicPay.API.Services.Externals.TransferAuthorizer
{
    public interface ITransferAuthorizerService : IDisposable
    {
        Task<bool> AuthorizationTranfer();
    }
}
