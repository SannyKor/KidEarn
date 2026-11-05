using KidEarn.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidEarn.Domain.Entities
{
    public class WalletTransaction
    {
        public int Id { get; set; }

        // Сума операції (завжди позитивна; напрямок визначається TransactionType)
        public decimal Amount { get; set; }

        public TransactionType Type { get; set; }

        // Опис операції для дитини та батьків (наприклад: "Винагорода за прибирання кімнати")
        public string Description { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        // Зв'язок з дитиною
        public int ChildId { get; set; }
        public Child Child { get; set; } = null!;
    }
}
