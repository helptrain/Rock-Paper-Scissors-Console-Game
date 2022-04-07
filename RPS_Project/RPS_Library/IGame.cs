/**
 * Project Name: RPS_Library
 * File Name: IGame.cs
 * Author(s): L. Bas, S. Podkorytov, M. Ivanov, T. Pollard
 * Date: 2022-04-06
 * Context: Interface for Game consisting of service and operation contracts
 */

using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace RPS_Library
{
    [ServiceContract(CallbackContract = typeof(ICallback))]
    public interface IGame
    {
        [OperationContract]
        bool Join(string clientName);

        [OperationContract(IsOneWay = true)]
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
