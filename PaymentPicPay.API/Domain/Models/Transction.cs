using PaymentPicPay.API.Domain.Enums;

namespace PaymentPicPay.API.Domain.Models
{
    public class Transaction : EntityBase
    {
        protected Transaction() { }

        public Transaction(int sendId, int receiveId, decimal amount, ETransactionType transactionType)
        {
            SendId = sendId;
            ReceiveId = receiveId;
            Amount = amount;
            TransactionType = transactionType;
            OperationStatus = EOperationStatus.Waiting;
        }

        public int SendId { get; private set; }
        public int ReceiveId { get; private set; }
        public decimal Amount { get; private set; }
        public EOperationStatus OperationStatus { get; private set; }
        public ETransactionType TransactionType { get; private set; }

        public bool Transfer(User send, User receive)
        {
            try
            {
                if (send is not Customer)
                    OperationStatus = EOperationStatus.Error;
                else
                {
                    send.Wallet.Payment(Amount);
                    receive.Wallet.Receiving(Amount);
                    OperationStatus = EOperationStatus.Success;
                }
            }
            catch
            {
                OperationStatus = EOperationStatus.Error;
            }

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
    }
}
