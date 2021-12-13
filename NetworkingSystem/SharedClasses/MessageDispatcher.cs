using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses{
    public abstract class MessageDispatcher<TMessageType> where TMessageType : class,new()
    {
        readonly List<(RouteAttribute route, Func<TMessageType, Task<TMessageType>> targetMethod)> _handlers = new List<(RouteAttribute route, Func<TMessageType, Task<TMessageType>> targetMethod)>();
        public async Task<TMessageType> DispatchAsync(TMessageType message)
        {
            foreach (var (route, target) in _handlers)
            {
                if (IsMatch(route,message))
                {
                    return await target(message);
                }
            }
            //No Handler what to do?
            return null;
        }
        protected void AddHandler(RouteAttribute route, Func<TMessageType, Task<TMessageType>> targetMethod)
            => _handlers.Add((route,targetMethod));
        protected abstract bool IsMatch(RouteAttribute route, TMessageType message);
        public abstract void Register<TParam,TResult>(Func<TParam,Task<TResult>> target);
        public abstract void Register<TParam>(Func<TParam, Task> target);
    }
}
