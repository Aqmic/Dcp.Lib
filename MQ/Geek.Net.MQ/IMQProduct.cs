namespace Geek.Net.MQ
{
    using System;

    public interface IMQProduct
    {
        void Send(byte[] body);
    }
}

