using PaymentPicPay.API.Domain.Enums;
using PaymentPicPay.API.Domain.Models;
using PaymentPicPay.API.Domain.Validators;
using PaymentPicPay.Tests.Domain._Builder;

namespace PaymentPicPay.Tests.Domain
{
    public class TransactionTests
    {
        private readonly Transaction _transaction;
        private readonly TransactionValidator _transactionValidator;
        private readonly CustomerBuilder _customerBuilder;
        private readonly MerchantBuilder _merchantBuilder;

        public TransactionTests()
        {
            _transaction = new Transaction(1, 1, 100, ETransactionType.B2B);
            _transactionValidator = new TransactionValidator();
            _customerBuilder = new CustomerBuilder();
            _merchantBuilder = new MerchantBuilder();
        }

        [Fact]
        public void Objeto_Transaction_Valido()
        {
            Transaction transaction = _transaction;
            var validator = _transactionValidator.Validate(transaction);

            Assert.True(validator.IsValid);
            Assert.IsType<Transaction>(transaction);
        }

        [Fact]
        public void Objeto_Transaction_Valor_Negativo_Invalido()
        {
            Transaction transaction = new(1, 1, -100, ETransactionType.B2B); ;
            var validator = _transactionValidator.Validate(transaction);

            Assert.False(validator.IsValid);
        }

        [Fact]
        public void Realizar_Transferencia_Valida()
        {
            var Send = _customerBuilder.Build();
            var Receive = _merchantBuilder.Build();
            var transaction = new Transaction(Send.Id, Receive.Id, 50, ETransactionType.B2C);

            transaction.Transfer(Send, Receive);

            Assert.Equal(950, Send.Wallet.Balance);
            Assert.Equal(1050, Receive.Wallet.Balance);
            Assert.Equal(EOperationStatus.Success, transaction.OperationStatus);
        }

        [Fact]
        public void Realizar_Rollback_Transferencia_Invalida()
        {
            var Send = _customerBuilder.Build();
            var Receive = _merchantBuilder.Build();
            var transaction = new Transaction(Send.Id, Send.Id, 50, ETransactionType.B2B);

            transaction.Transfer(Send, Send);

            Assert.Equal(1000, Send.Wallet.Balance);
            Assert.Equal(1000, Receive.Wallet.Balance);
            Assert.Equal(EOperationStatus.Error, transaction.OperationStatus);
        }
    }
}
