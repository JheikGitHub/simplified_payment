using FluentValidation.Results;
using PaymentPicPay.API.Domain.Enums;

namespace PaymentPicPay.API.Domain.Models
{
    public class TransactionB2B : Transaction
    {
        //Mapper Entity Framework
        protected TransactionB2B() { }

        public TransactionB2B(
            Customer send,
            Customer receive,
            decimal amount)
            : base(amount)
        {
            Send = send;
            Receive = receive;
            TransactionType = ETransactionType.B2B;
        }

        public Customer Send { get; private set; }
        public Customer Receive { get; private set; }

        public override bool Transfer()
        {
            return base.Transfer();
        }

        public override bool IsValid()
        {
            if (Send.Id == Receive.Id)
                Validations.Errors.Add(
                    new ValidationFailure
                    (
                        "sendId",
                        "Error when transferring to the same person.")
                    );



            return base.IsValid();
        }

        protected override void ValidateTransferData()
        {
            SendId = Send.Id;
            ReceiveId = Receive.Id;

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

