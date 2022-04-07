/**
 * Project Name: RPS_Library
 * File Name: ICallback.cs
 * Author(s): L. Bas, S. Podkorytov, M. Ivanov, T. Pollard
 * Date: 2022-04-06
 * Context: Interface for callbacks consisting of an operation contract to send a player choice
 */

using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace RPS_Library
{
    public interface ICallback
    {
        [OperationContract(IsOneWay = true)]
        void SendChoice(Dictionary<string, HandSignalType> dic);
    }
}
