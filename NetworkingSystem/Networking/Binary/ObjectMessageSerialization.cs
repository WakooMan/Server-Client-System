﻿using System;

namespace Networking
{
    public static class ObjectMessageSerialization
    {
        public static object Serialize<T>(T instance) =>(object)instance;
        public static T Deserialize<T>(object objmsg) => (T)objmsg;
        public static object Deserialize(Type type, object objmsg) => objmsg;
    }
}
