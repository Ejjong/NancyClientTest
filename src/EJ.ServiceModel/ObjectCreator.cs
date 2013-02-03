using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection.Emit;

namespace EJ.ServiceModel
{
    public static class ObjectCreator
    {
        public static object CreateObject(Type type)
        {
            //return DelegateFactory.CreateCtor(type)();
            return CreateInstance(type);
        }

        public static object CreateInstance(Type type)
        {
            var method = new DynamicMethod("", typeof(object), Type.EmptyTypes);
            var il = method.GetILGenerator();

            if (type.IsValueType)
            {
                var local = il.DeclareLocal(type);
                // method.InitLocals == true, so we don't have to use initobj here
                il.Emit(OpCodes.Ldloc, local);
                il.Emit(OpCodes.Box, type);
                il.Emit(OpCodes.Ret);
            }
            else
            {
                var ctor = type.GetConstructor(Type.EmptyTypes);
                il.Emit(OpCodes.Newobj, ctor);
                il.Emit(OpCodes.Ret);
            }

            return method.Invoke(null, null);
        }
    }

    public static class DelegateFactory
    {
        public delegate object LateBoundCtor();

        private static readonly ConcurrentDictionary<Type, LateBoundCtor> _ctorCache = new ConcurrentDictionary<Type, LateBoundCtor>();

        public static LateBoundCtor CreateCtor(Type type)
        {
            LateBoundCtor ctor = _ctorCache.GetOrAdd(type, t =>
            {
                var ctorExpression = Expression.Lambda<LateBoundCtor>(Expression.Convert(Expression.New(type), typeof(object)));

                return ctorExpression.Compile();
            });

            return ctor;
        }
    }
}
