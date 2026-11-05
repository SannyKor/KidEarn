using KidEarn.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace KidEarn.Domain.Entities
{
    public class ExtraTask
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }

        // Фіксована сума винагороди, яку призначив батько
        public decimal RewardAmount { get; set; }

        public TaskState State { get; set; } = TaskState.Available;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }

        // Foreign Key та навігаційна властивість на дитину
        public int ChildId { get; set; }
        public Child Child { get; set; } = null!;
    }
}
