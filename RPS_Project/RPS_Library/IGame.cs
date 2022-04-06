using System;
using System.Collections.Generic;
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
        bool Join(string clientName);

        [OperationContract(IsOneWay = true)]
        void Leave();

        [OperationContract(IsOneWay = true)]
        void PostChoice(HandSignalType choice);

        [OperationContract]
        HandSignalType[] GetAllChoices();
        [OperationContract]
        string Playing();
        [OperationContract]
        void SetPlayerHands(HandSignalType p1, HandSignalType p2);
        [OperationContract]
        void ResetChoices();
    }
}
