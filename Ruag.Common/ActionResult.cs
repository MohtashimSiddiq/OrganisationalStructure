using Ruag.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruag.Common
{
    public class ActionResult<T>
    {
        public eReturnCode ReturnCode { get; set; }

        public string ReturnDescription { get; set; }

        public T Result { get; set; }
    }
}
