//using System.Threading.Tasks;
using System.ServiceModel;
using System.Drawing;

namespace CommunicationInterface
{
    [ServiceContract]
    public interface IMyobject
    {
        [OperationContract]
        object GetCommandString(object i, string player);
        [OperationContract]
        void MoveX(string name, double x);
        [OperationContract]
        void MoveY(string name, double y);
        [OperationContract]
        void CreateBullet(string name, double spx, double spy);
        [OperationContract]
        int state(string name);
        [OperationContract]
        void say(string say);
        [OperationContract]
        void addBlock(string name, int type);
        [OperationContract]
        string logOrCreate(string name, Color color);
    }
}
