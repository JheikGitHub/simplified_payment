using FluentValidation.Results;
using PaymentPicPay.API.Domain.Enums;

namespace PaymentPicPay.API.Domain.Models
{
    public class TransactionB2C : Transaction
    {
        //Mapper Entity Framework
        protected TransactionB2C() { }

        public TransactionB2C(
            Customer send,
            Merchant receive,
            decimal amount)
            : base(amount)
        {
            Send = send;
            Receive = receive;
            TransactionType = ETransactionType.B2C;
        }

        public Customer Send { get; private set; }
        public Merchant Receive { get; private set; }

        public override bool Transfer()
        {
            return base.Transfer();
        }

        public override bool IsValid()
        {
            if (Send is null)
                Validations.Errors.Add(
                   new ValidationFailure
                   (
                       nameof(Send),
                       "Error send is null.")
                   );

            if (Receive is null)
                Validations.Errors.Add(
                   new ValidationFailure
                   (
                       nameof(Receive),
                       "Error receive is null.")
                   );

            return base.IsValid();
        }

        protected override void ValidateTransferData()
        {
            SendId = Send.Id;
            ReceiveId = Receive.Id;

            //Transferencia realizada de Cliente para Lojista
            if (Send.Wallet.Payment(Amount))
            {
                Receive.Wallet.Receiving(Amount);
                OperationStatus = EOperationStatus.Success;
            }
            else
            {
                Validations.Errors
                    .Add(new ValidationFailure
                    (
                        nameof(SendId),
                        "Error performing transfer. Detail: Customer without the full amount available.")
                    );

                OperationStatus = EOperationStatus.Error;
            }
        }
    }
}
