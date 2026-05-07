using System;
using System.Collections.Generic;

namespace BankApi.Domain
{
    public class Account : BaseEntity
    {
        public decimal Balance { get; private set; }
        public string AccountNumber { get; private set; }
        public string OwnerId { get; private set; }

        public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();
        private readonly List<Transaction> _transactions = new();

        private Account() { }

        public Account(string ownerId, string accountNumber, decimal initialBalance = 0)
        {
            if (string.IsNullOrWhiteSpace(ownerId))
                throw new ArgumentException("OwnerId is required.");

            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentException("AccountNumber is required.");

            if (initialBalance < 0)
                throw new ArgumentException("Initial balance cannot be negative.");

            OwnerId = ownerId;
            AccountNumber = accountNumber;
            Balance = initialBalance;
        }

        public void Deposit(decimal amount)
        {
            ValidateAmount(amount);

            Balance += amount;

            _transactions.Add(Transaction.CreateDeposit(Id, amount));

        }

        public void Withdraw(decimal amount)
        {
            ValidateAmount(amount);

            if (Balance < amount)
                throw new ArgumentException("Insufficient funds to withdraw.");

            Balance -= amount;

            _transactions.Add(Transaction.CreateWithdraw(Id, amount));
        }

        private static void ValidateAmount(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("The amount must be greater than 0.");
        }
    }
}