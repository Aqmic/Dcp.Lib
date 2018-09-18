using System;
using System.Collections.Generic;
using System.Text;

namespace Dcp.Net.MQ.Rpc.Handler
{
   public class ValidationAttribute:Attribute
    {
        //
        // 摘要:
        //     验证指定的对象。
        //
        // 参数:
        //   value:
        //     要验证的对象的值。
        //
        //   name:
        //     要包括错误消息中的名称。
        //
        // 异常:
        //   T:System.ComponentModel.DataAnnotations.ValidationException:
        //     value 无效。
        public void Validate(object value, string name) { }
    }
}
