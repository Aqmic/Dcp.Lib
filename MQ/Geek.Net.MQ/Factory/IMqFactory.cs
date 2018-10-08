using System;
using System.Collections.Generic;
using System.Text;

namespace Geek.Net.MQ.Factory
{
    public interface IMqFactory
    {
        IMessageQueue CreateRabbitFactory();
       
    }
}
