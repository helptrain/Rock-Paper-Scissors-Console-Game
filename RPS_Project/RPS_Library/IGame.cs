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
        //void Leave();
        void Leave(string clientName);

        [OperationContract(IsOneWay = true)]
        void PostChoice(string playerName, HandSignalType play);

        [OperationContract]
        Dictionary<string, HandSignalType> GetAllChoices();

        [OperationContract]
        string Playing();

        [OperationContract]
        void SetPlayerOneHand(HandSignalType p);

        [OperationContract]
        void SetPlayerTwoHand(HandSignalType p);

        [OperationContract]
        void ResetChoices();
    }
}
