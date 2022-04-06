using Networking.Implementation.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Networking.Implementation
{
    public class BaseClientNetworkStateComponent : BaseNetworkStateComponent
    {
        public override void AddMessageHandlers(Network network)
        {
            MethodInfo[] Methods = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            List<MethodInfo> MethodList = Enumerable.ToList(Methods).Where(m => m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType.IsSubclassOf(typeof(Message))).ToList();
            MethodList.ForEach(
                    m =>
                    {
                        network.Register(((Message)Activator.CreateInstance(m.GetParameters()[0].ParameterType)).MessageId, new Message.ServerToClientMessageHandlerDelegate(message => m.Invoke(this, new object []{ message })));
                    });
        }

        public override void RemoveMessageHandlers(Network network)
        {
            MethodInfo[] Methods = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            List<MethodInfo> MethodList = Enumerable.ToList(Methods).Where(m => m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType.IsSubclassOf(typeof(Message))).ToList();
            MethodList.ForEach(
                    m =>
                    {
                        network.UnRegister(((Message)Activator.CreateInstance(m.GetParameters()[0].ParameterType)).MessageId,false);
                    });
        }
    }
}
