using System;
using System.Collections.Generic;
using System.Text;

namespace KidEarn.Domain.Enums
{
    public enum TransactionType
    {
        DailyAllowance = 1,       // Щоденна ставка (з урахуванням коефіцієнта успішності)
        TaskReward = 2,           // Винагорода за підтверджене завдання
        DepositTransferIn = 3,    // Переказ з основного гаманця на депозит
        DepositTransferOut = 4,   // Повернення з депозиту на основний гаманець
        Payout = 5,               // Виведення готівки / списання батьками
        ManualAdjustment = 6      // Ручне коригування батьком (бонус або штраф)
    }
}
