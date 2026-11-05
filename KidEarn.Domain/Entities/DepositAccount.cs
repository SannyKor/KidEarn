using System;
using System.Collections.Generic;
using System.Text;

namespace KidEarn.Domain.Entities
{
    public class DepositAccount
    {
        public int Id { get; set; }

        // Баланс на депозитному рахунку
        public decimal Balance { get; set; } = 0.00m;

        // Процентна ставка (наприклад, 0.05m = 5%)
        public decimal InterestRate { get; set; } = 0.05m;

        // Дата останнього нарахування відсотків (для уникнення повторного нарахування)
        public DateTime? LastInterestAppliedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Зв'язок 1:1 з дитиною
        public int ChildId { get; set; }
        public Child Child { get; set; } = null!;
    }
}
