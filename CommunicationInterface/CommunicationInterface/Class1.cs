using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.ServiceModel;

namespace CommunicationInterface
{
    [ServiceContract]
    public interface IMyobject
    {
        [OperationContract]
        object GetCommandString(object i, string player);
        [OperationContract]
        void MoveX(string name, int x);
        [OperationContract]
        void MoveY(string name, int y);
        [OperationContract]
        void CreateBullet(string name, int dir);
        [OperationContract]
        int state(string name);
        [OperationContract]
        void say(string say);
        [OperationContract]
        void addBlock(string name, int type);
    }
}
