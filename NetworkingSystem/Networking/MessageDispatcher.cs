using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Networking{
    public abstract class MessageDispatcher<TMessageType> where TMessageType : class, new()
    {
        readonly List<(RouteAttribute route, Func<INetworkChannel, TMessageType, TMessageType> targetMethod)> _handlers = new List<(RouteAttribute route, Func<INetworkChannel, TMessageType, TMessageType> targetMethod)>();
        public Message Dispatch(INetworkChannel channel, TMessageType message)
        {
            foreach (var (route, target) in _handlers)
            {
                if (IsMatch(route, message))
                   return target(channel, message) as Message;
            }
            //No Handler what to do?
            return null;
        }

        public void Bind<TController>()
        {
            bool returnTypeIsMessageType(MethodInfo mi)
                => mi.ReturnType.IsSubclassOf(typeof(Message));
            bool returnTypeIsVoid(MethodInfo mi)
                => mi.ReturnType == typeof(void);

            var Methods = typeof(TController)
                .GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(HasRouteAttribute)
                .Where(x => x.GetParameters().Length >= 1 && x.GetParameters().Length <= 2)
                .Where(x => returnTypeIsMessageType(x) || returnTypeIsVoid(x));
            foreach (var mi in Methods)
            {
                var Wrapper = new Func<INetworkChannel, TMessageType, TMessageType>((channel, msg) =>
                  {
                      var Param = mi.GetParameters().Length == 1
                      ? Deserialize(mi.GetParameters()[0].ParameterType, msg)
                      : Deserialize(mi.GetParameters()[1].ParameterType, msg);
                      if (returnTypeIsVoid(mi))
                      {
                          var t = mi.GetParameters().Length == 1
                        ? (mi.Invoke(null, new object[] { Param }))
                        : (mi.Invoke(null, new object[] { channel, Param }));
                          return null;
                      }
                      else
                      {
                          var result = mi.GetParameters().Length == 1
                        ? (mi.Invoke(null, new object[] { Param }) as TMessageType)
                        : (mi.Invoke(null, new object[] { channel, Param }) as TMessageType);
                          if (result != null)
                              return Serialize(result);
                          else
                              return null;
                      }
                  });
                AddHandler(GetAttribute(mi), Wrapper);
            }
        }

        public void UnBind() => _handlers.Clear();

        private bool HasRouteAttribute(MethodInfo mi) => GetAttribute(mi) != null;

        public void Bind(INetworkChannel Channel)
        => Channel.OnMessage((TMessageType message) =>
            {
                var response = Dispatch(Channel, message);
                if (response != null)
                    Channel.Send(response, response.EMethod);
                

            });

        protected void AddHandler(RouteAttribute route, Func<INetworkChannel, TMessageType, TMessageType> targetMethod)
            => _handlers.Add((route, targetMethod));
        protected bool HasAttribute(MethodInfo mi)
           => GetAttribute(mi) != null;

        protected abstract object Deserialize(Type type, TMessageType message);

        protected abstract TMessageType Serialize<TResult>(TResult result);

        protected abstract TParam Deserialize<TParam>(TMessageType messageType);

        protected abstract bool IsMatch(RouteAttribute route, TMessageType message);

        protected abstract RouteAttribute GetAttribute(MethodInfo mi);
    }
}
