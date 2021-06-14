using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Runtime;

namespace ThongTinDoiNgoai
{
    [DataContract]
    public class ThongTinTaiKhoan
    {
        [DataMember(Name = "Success")]
        public bool Success { get; set; }
        [DataMember(Name = "Token")]
        public string Token { get; set; }
        [DataMember(Name = "RefreshToken")]
        public object RefreshToken { get; set; }
        [DataMember(Name = "Message")]
        public object Message { get; set; }
        [DataMember(Name = "FullName")]
        public string FullName { get; set; }
        [DataMember(Name = "Email")]
        public string Email { get; set; }
        [DataMember(Name = "CellPhone")]
        public string CellPhone { get; set; }
        [DataMember(Name = "Address")]
        public string Address { get; set; }
        [DataMember(Name = "OwnerCode")]
        public object OwnerCode { get; set; }

        [DataMember(Name = "ErrCode")]
        public int ErrCode { get; set; }

        [DataMember(Name = "IdentifierCode")]
        public string IdentifierCode { get; set; }
        
    }
}