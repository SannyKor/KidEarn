using System;
using System.Collections.Generic;
using System.Text;

namespace KidEarn.Domain.Enums
{
    public enum PayoutRequestStatus
    {
        Pending = 0,    // Очікує розгляду батьком
        Approved = 1,   // Схвалено і виплачено (баланс списано)
        Rejected = 2    // Відхилено батьком (кошти не списуються)
    }
}
