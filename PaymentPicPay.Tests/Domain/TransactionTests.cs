using PaymentPicPay.API.Domain.Enums;
using PaymentPicPay.API.Domain.Models;
using PaymentPicPay.Tests.Domain._Builder;

namespace PaymentPicPay.Tests.Domain
{
    public class TransactionTests
    {
        private readonly TransactionB2C _transaction;
        private readonly CustomerBuilder _customerBuilder;
        private readonly MerchantBuilder _merchantBuilder;

        public TransactionTests()
        {
            _customerBuilder = new CustomerBuilder();
            _merchantBuilder = new MerchantBuilder();

            _transaction = new TransactionB2C(_customerBuilder.CreateSendBuild(), _merchantBuilder.Build(), 100);
        }

        [Fact]
        public void Deve_Criar_Objeto_Valido()
        {
            TransactionB2C transaction = _transaction;

            Assert.True(transaction.IsValid());
        }

        [Fact]
        public void Deve_Criar_Objeto_Invalido_Valor_Negativo()
        {
            TransactionB2C transaction = new(_customerBuilder.CreateSendBuild(), _merchantBuilder.Build(), -50); ;    

            Assert.False(transaction.IsValid());
        }

        [Fact]
        public void Realizar_Transferencia_Valida()
        {
            var Send = _customerBuilder.CreateSendBuild();
            var Receive = _merchantBuilder.Build();

            var transaction = new TransactionB2C(Send, Receive, 50);
            transaction.IsValid();

            transaction.Transfer();     

            Assert.Equal(950, Send.Wallet.Balance);
            Assert.Equal(1050, Receive.Wallet.Balance);
            Assert.Equal(EOperationStatus.Success, transaction.OperationStatus);
        }

        [Fact]
        public void Realizar_Rollback_Transferencia_Invalida_Quantia_Negativa()
        {
            var Send = _customerBuilder.CreateSendBuild();
            var Receive = _merchantBuilder.Build();

            var transaction = new TransactionB2C(Send, Receive, -50);
            transaction.IsValid();

            transaction.Transfer();

            Assert.Equal(1000, Send.Wallet.Balance);
            Assert.Equal(1000, Receive.Wallet.Balance);
            Assert.Equal(EOperationStatus.Error, transaction.OperationStatus);
        }
    }
}
