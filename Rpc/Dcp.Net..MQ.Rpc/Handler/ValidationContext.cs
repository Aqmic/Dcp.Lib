using System;
using System.Collections.Generic;
using System.Text;

namespace Dcp.Net.MQ.Rpc.Handler
{
    public class ValidationContext
    {
        public string MemberName{ get; set; }
        public ValidationContext(object obj)
        {

        }
        public static void ValidateObject(object instance, ValidationContext validationContext) { }

        public static void ValidateObject(object instance, ValidationContext validationContext, bool validateAllProperties) {

        }
    }
}
