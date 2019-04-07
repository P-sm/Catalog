using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Сatalog.Enum
{
    public enum GenderEnum
    {
        [Description("Неизвестно")]
        Unknown = 0,
        [Description("Мужской")]
        Men = 1,
        [Description("Женский")]
        Women = 2,
        [Description("Неприменимо")]
        Inapplicable = 9
    };
}
