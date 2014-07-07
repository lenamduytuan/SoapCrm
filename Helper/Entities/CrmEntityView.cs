
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernSoapApp.Helper.Entities
{
    public class CrmEntityView 
    {

        public const string EntityLogicalName = "savedquery";
        public string  ReturnedTypeCode {get;set;}
        public string  FetchXml {get;set;}
        public string  LayoutXml {get;set;}
        public int?  QueryType {get;set;}
        public int EntityTypeCode { get; set; }
             
    }
}
