using System;
using System.Collections.Generic;
using System.Text;

namespace KidEarn.Domain.Enums
{
    public enum TaskState
    {
        Available = 0,   // Створено батьком, доступне для взяття
        InProgress = 1,  // Дитина взяла в роботу
        Submitted = 2,   // Дитина позначила як виконане (очікує перевірки)
        Approved = 3,    // Батько підтвердив (кошти нараховуються)
        Rejected = 4     // Батько відхилив (потребує доопрацювання)
    }
}
