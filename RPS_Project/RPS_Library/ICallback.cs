using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel; //WCF stuff
using System.Text;
using System.Threading.Tasks;

namespace RPS_Library
{
    public interface ICallback
    {
        [OperationContract(IsOneWay = true)]
        void SendChoice(HandSignalType[] messages);
    }
}
