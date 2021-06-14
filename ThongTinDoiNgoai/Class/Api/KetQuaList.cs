using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThongTinDoiNgoai
{
    public class KetQuaList
    {
        public KetQuaList()
        { }
        public KetQuaList(int code_, string message_)
        {
            code = code_;
            message = message_;
        }
        public int code { get; set; }
        public string message { get; set; }
        public object data { get; set; }
        public int totalrow { get; set; }
    }
}