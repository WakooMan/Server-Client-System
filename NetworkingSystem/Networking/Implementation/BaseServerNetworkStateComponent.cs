using Networking.Implementation.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Networking.Implementation
{
    public abstract class BaseServerNetworkStateComponent : BaseNetworkStateComponent
    {
        public override void AddMessageHandlers(Network network)
        {
            MethodInfo[] Methods = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            List<MethodInfo> MethodList = Enumerable.ToList(Methods).Where(m=> m.GetParameters().Length==2 && m.GetParameters()[0].ParameterType.IsAssignableFrom(typeof(INetworkChannel)) && m.GetParameters()[1].ParameterType.IsSubclassOf(typeof(Message))).ToList();
            MethodList.ForEach(
                m =>
                {
                    network.Register(((Message)Activator.CreateInstance(m.GetParameters()[1].ParameterType)).MessageId, new Message.ClientToServerMessageHandlerDelegate((channel,message)=> m.Invoke(this,new object[] { channel,message })));
                });
        }

        public override void RemoveMessageHandlers(Network network)
        {
            MethodInfo[] Methods = GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            List<MethodInfo> MethodList = Enumerable.ToList(Methods).Where(m => m.GetParameters().Length == 2 && m.GetParameters()[0].ParameterType.GetInterface(nameof(INetworkChannel)) != null && m.GetParameters()[1].ParameterType.IsSubclassOf(typeof(Message))).ToList();
            MethodList.ForEach(
                m =>
                {
                    network.UnRegister(((Message)Activator.CreateInstance(m.GetParameters()[1].ParameterType)).MessageId,true);
                });
        }
    }
}
