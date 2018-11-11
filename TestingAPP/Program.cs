using System;
using MT_ProxyParser;

namespace TestingAPP
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(MT_GetProxy.ParseProxies("Ukraine"));
        }
    }
}
