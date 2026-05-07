using System;

namespace BankApi.Domain
{
    public class Transaction : BaseEntity
    {
        public Guid AccountId { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public ETransactionType Type { get; private set; }

        private Transaction() { }  

        private Transaction(Guid accountId, decimal amount, ETransactionType type)
        {
            AccountId = accountId;
            Amount = amount;
            Type = type;
            CreatedAt = DateTime.UtcNow;
        }

        public static Transaction CreateDeposit(Guid accountId, decimal amount)
            => new Transaction(accountId, amount, ETransactionType.Deposit);

        public static Transaction CreateWithdraw(Guid accountId, decimal amount)
            => new Transaction(accountId, amount, ETransactionType.Withdraw);
    }
}
