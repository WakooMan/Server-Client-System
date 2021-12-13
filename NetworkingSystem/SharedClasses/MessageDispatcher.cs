using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClasses
{
    public abstract class MessageDispatcher<TMessageType> where TMessageType : class,new()
    {
        public abstract void Register<TParam,TResult>(Func<TParam,Task<TResult>> target);
        public abstract void Register<TParam>(Func<TParam, Task> target);
        public abstract Task<TMessageType> DispatchAsync(TMessageType message);
    }
}
