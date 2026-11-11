using System;
using System.Collections.Generic;
using System.Text;

namespace KidEarn.Domain.Entities
{
    public class Child
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        // Базова щоденна ставка кишенькових грошей (наприклад, 50.00 грн)
        public decimal DailyBaseAllowance { get; set; }

        // Поточний коефіцієнт на цей тиждень (за замовчуванням 1.00m)
        // Розраховується автоматично на основі успішності за попередній тиждень
        public decimal CurrentWeeklyMultiplier { get; set; } = 1.00m;

        // Поточний баланс доступних Nut (основний гаманець)
        public decimal NutBalance { get; set; } = 0.00m;

        // Прив'язка до сім'ї (Multi-Tenant ID)
        public int FamilyId { get; set; }
        public Family Family { get; set; } = null!;

        // Власний Identity логін для входу
        public string? IdentityUserId { get; set; }

        // Навігаційні колекції
        public DepositAccount? DepositAccount { get; set; }
        public ICollection<Grade> Grades { get; set; } = new List<Grade>();
        public ICollection<ExtraTask> Tasks { get; set; } = new List<ExtraTask>();
        public ICollection<WalletTransaction> Transactions { get; set; } = new List<WalletTransaction>();
        public ICollection<PayoutRequest> PayoutRequests { get; set; } = new List<PayoutRequest>();
    }
}
