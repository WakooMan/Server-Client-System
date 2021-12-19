using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SharedClasses{
    public abstract class MessageDispatcher<TMessageType> where TMessageType : class,new()
    {
        readonly List<(RouteAttribute route, Func<INetworkChannel,TMessageType, Task<TMessageType>> targetMethod)> _handlers = new List<(RouteAttribute route, Func<INetworkChannel,TMessageType, Task<TMessageType>> targetMethod)>();
        public async Task<TMessageType> DispatchAsync(INetworkChannel channel,TMessageType message)
        {
            foreach (var (route, target) in _handlers)
            {
                if (IsMatch(route,message))
                {
                    return await target(channel,message);
                }
            }
            //No Handler what to do?
            return null;
        }

        public void Bind<TController>()
        {
            bool returnTypeIsTask(MethodInfo mi)
                => mi.ReturnType.IsAssignableFrom(typeof(Task));
            bool returnTypeIsTaskT(MethodInfo mi)
                =>mi.ReturnType.BaseType?.IsAssignableFrom(typeof(Task))??false;

            var Methods = typeof(TController)
                .GetMethods(BindingFlags.Public| BindingFlags.Static)
                .Where(HasRouteAttribute)
                .Where(x => x.GetParameters().Length >= 1 && x.GetParameters().Length <= 2)
                .Where(x => returnTypeIsTask(x) || returnTypeIsTaskT(x));
            foreach (var mi in Methods)
            {
                var Wrapper = new Func<INetworkChannel,TMessageType,Task<TMessageType>>( async (channel,msg) => 
                {
                    var Param = mi.GetParameters().Length==1
                    ? Deserialize(mi.GetParameters()[0].ParameterType, msg)
                    : Deserialize(mi.GetParameters()[1].ParameterType,msg);
                    try
                    {
                        if (returnTypeIsTask(mi))
                        {
                            var t = mi.GetParameters().Length == 1
                            ? (mi.Invoke(null, new object[] { Param }) as Task)
                            : (mi.Invoke(null, new object[] { channel,Param }) as Task);
                            if (t != null)
                                await t;
                            return null;
                        }
                        else
                        {
                            var result = mi.GetParameters().Length == 1
                            ? (await (mi.Invoke(null, new object[] { Param }) as dynamic) as dynamic)
                            :(await (mi.Invoke( null,new object[] { channel,Param } ) as dynamic) as dynamic);
                            if (result != null)
                                return Serialize( result as dynamic);
                            else
                                return null;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        return null;
                    }
                });
                AddHandler(GetAttribute(mi),Wrapper);
            }
        }

        private bool HasRouteAttribute(MethodInfo mi) => GetAttribute(mi)!=null;

        public void Bind<TProtocol>(NetworkChannel<TProtocol,TMessageType> Channel)
            where TProtocol : Protocol<TMessageType>,new()
        => Channel.OnMessage(async message =>
            {
                var response = await DispatchAsync(Channel,message).ConfigureAwait(false);
                if (response != null)
                {
                    try
                    {
                        await Channel.SendAsync(response).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
            });
        public virtual void Register<TParam, TResult>(Func< TParam, Task<TResult>> target)
            => Register(new Func<INetworkChannel, TParam, Task<TResult>>((c, m) => target(m)));
        public virtual void Register<TParam, TResult>(Func<INetworkChannel,TParam, Task<TResult>> target)
        {
            if (!HasAttribute(target.Method))
                throw new Exception("Missing Required Route Attribute");

            var wrapper = new Func<INetworkChannel,TMessageType, Task<TMessageType>>(async (channel,message) =>
            {
                var Param = Deserialize<TParam>(message);
                var result = await target(channel,Param);

                if (result != null)
                    return Serialize(result);
                else
                    return null;
            });
            AddHandler(GetAttribute(target.Method), wrapper);
        }
        public virtual void Register<TParam>(Func<TParam, Task> target)
            => Register(new Func<INetworkChannel, TParam, Task>((c,m)=>target(m)));
        public virtual void Register<TParam>(Func<INetworkChannel,TParam, Task> target)
        {
            if (!HasAttribute(target.Method))
                throw new Exception("Missing Required Route Attribute");

            var wrapper = new Func<INetworkChannel,TMessageType, Task<TMessageType>>(async (channel,message) =>
            {
                var Param = Deserialize<TParam>(message);
                await target(channel,Param);
                return null;
            });
            AddHandler(GetAttribute(target.Method), wrapper);
        }

        protected void AddHandler(RouteAttribute route, Func<INetworkChannel,TMessageType, Task<TMessageType>> targetMethod)
            => _handlers.Add((route,targetMethod));
        protected bool HasAttribute(MethodInfo mi)
           => GetAttribute(mi) != null;

        protected abstract object Deserialize(Type type,TMessageType message);

        protected abstract TMessageType Serialize<TResult>(TResult result);

        protected abstract TParam Deserialize<TParam>(TMessageType messageType);

        protected abstract bool IsMatch(RouteAttribute route, TMessageType message);

        protected abstract RouteAttribute GetAttribute(MethodInfo mi);
    }
}
