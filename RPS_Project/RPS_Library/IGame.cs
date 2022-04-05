using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RPS_Library
{
    [ServiceContract(CallbackContract = typeof(ICallback))]
    public interface IGame
    {
        [OperationContract]
        int Join();

        [OperationContract]
        void Leave(string clientName);

        [OperationContract]
        void PostChoice(HandSignalType choice);

        [OperationContract]
        HandSignalType[] GetAllChoices();
    }
}
