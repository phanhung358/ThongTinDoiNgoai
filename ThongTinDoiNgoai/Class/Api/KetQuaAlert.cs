using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThongTinDoiNgoai
{
    public class KetQuaAlert
    {
        public KetQuaAlert()
        { }
        public KetQuaAlert(int code_, string message_)
        {
            code = code_;
            message = message_;
        }
        public int code { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}