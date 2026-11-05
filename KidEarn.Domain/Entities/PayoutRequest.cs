using KidEarn.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidEarn.Domain.Entities
{
    public class PayoutRequest
    {
        public int Id { get; set; }

        // Запитана кількість Nut
        public decimal Amount { get; set; }

        public PayoutRequestStatus Status { get; set; } = PayoutRequestStatus.Pending;

        // Бажана мета (наприклад: "Купити квиток у кіно" або "Готівка")
        public string Purpose { get; set; } = string.Empty;

        public string? RejectionReason { get; set; }

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ProcessedAt { get; set; }

        // Зв'язок з дитиною
        public int ChildId { get; set; }
        public Child Child { get; set; } = null!;
    }
}
