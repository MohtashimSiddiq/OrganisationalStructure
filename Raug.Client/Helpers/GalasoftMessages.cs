using Ruag.Client.Helpers.Enums;
using Ruag.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ruag.Client.Helpers
{
    public class LocaleMessage
    {
        public eLocales SelectedLocale { get; set; }
    }

    public class UIModeMessage
    {
        public eUIMode UIMode { get; set; }
    }

    public class UIColorMessage
    {
        public eUIColor UIColor { get; set; }
    }

    public class MsgBxResultMessage
    {
        public eMessageBoxResult Result { get; set; }
        public eMessageBoxType Type { get; set; }
    }

    public class ShowMsgBxMessage
    {
        public eMessageBoxType Type { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }

}
