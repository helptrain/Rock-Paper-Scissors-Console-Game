using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPS_Library
{
    public class ConcreteCallback : ICallback
    {
        public void SendChoice(HandSignalType[] messages)
        {
            Console.WriteLine("This Triggered");
        }
    }
}
