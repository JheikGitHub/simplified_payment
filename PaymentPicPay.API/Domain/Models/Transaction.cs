using FluentValidation.Results;
using PaymentPicPay.API.Domain.Enums;

namespace PaymentPicPay.API.Domain.Models
{
    public class Transaction : EntityBase
    {
        protected Transaction() { }

        public Transaction(decimal amount)
        {
            Amount = amount;
            OperationStatus = EOperationStatus.Waiting;
        }

        public int SendId { get; protected set; }
        public int ReceiveId { get; protected set; }
        public decimal Amount { get; private set; }
        public EOperationStatus OperationStatus { get; protected set; }
        public ETransactionType TransactionType { get; protected set; }

        public override bool IsValid()
        {
            if (Amount <= 0)
                Validations.Errors.Add(
                   new ValidationFailure
                   (
                       nameof(Amount),
                       "Error amount must be greater than zero.")
                   );

            return Validations.IsValid;
        }

        public virtual bool Transfer()
        {
            try
            {
                if (Validations.IsValid)
                    ValidateTransferData();
                else
                    OperationStatus = EOperationStatus.Error;


                switch (OperationStatus)
                {
                    case EOperationStatus.Waiting:
                        return false;

                    case EOperationStatus.Error:
                        UpdateDate();
                        return false;

                    case EOperationStatus.Success:
                        UpdateDate();
                        return true;
                }

                return false;
            }
            catch
            {
                OperationStatus = EOperationStatus.Error;
            }

            return false;
        }

        protected virtual void ValidateTransferData()
        {
            throw new NotImplementedException();
        }
    }
}
