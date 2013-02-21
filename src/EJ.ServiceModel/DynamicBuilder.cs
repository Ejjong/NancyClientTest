using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace EJ.ServiceModel
{
    public class DynamicBuilder
    {
        public AppDomain CurrentDomain
        {
            get { return Thread.GetDomain(); }
        }

        public AssemblyBuilder GetAssemblyBuilder(string assemblyName)
        {
            var aname = new AssemblyName(assemblyName);
            aname.CodeBase = Directory.GetCurrentDirectory();
            AppDomain currentDomain = AppDomain.CurrentDomain;
            AssemblyBuilder builder = currentDomain.DefineDynamicAssembly(aname, AssemblyBuilderAccess.RunAndSave);

            return builder;
        }

        public ModuleBuilder GetModule(AssemblyBuilder asmBuilder)
        {
            ModuleBuilder builder = asmBuilder.DefineDynamicModule("MyBuilderModule", "MyBuilder.dll");

            return builder;
        }

        public EnumBuilder GetEnum(ModuleBuilder modBuilder, string enumNm)
        {
            EnumBuilder builder = modBuilder.DefineEnum(enumNm, TypeAttributes.Public, typeof(System.Int32));

            return builder;
        }

        public TypeBuilder GetType(ModuleBuilder modBuilder, string classNm)
        {
            TypeBuilder builder = modBuilder.DefineType(classNm, TypeAttributes.Public);

            return builder;
        }

        public TypeBuilder GetTypeBuilder(ModuleBuilder moduleBuilder, string classNm, params string[] genericParameters)
        {
            TypeBuilder builder = moduleBuilder.DefineType(classNm, TypeAttributes.Public);
            GenericTypeParameterBuilder[] genBuilders = builder.DefineGenericParameters(genericParameters);

            foreach (var genericTypeParameterBuilder in genBuilders)
            {
                genericTypeParameterBuilder.SetGenericParameterAttributes(GenericParameterAttributes.ReferenceTypeConstraint |
                    GenericParameterAttributes.DefaultConstructorConstraint);
            }

            return builder;
        }

        public MethodBuilder GetMethod(TypeBuilder typeBuilder, string methodNm)
        {
            MethodBuilder builder = typeBuilder.DefineMethod(methodNm,
                                                              MethodAttributes.Public | MethodAttributes.HideBySig);
            return builder;
        }

        public MethodBuilder GetMethod(TypeBuilder typeBuilder, string methodNm, Type returnType, params Type[] parameterTypes)
        {
            MethodBuilder builder = typeBuilder.DefineMethod(methodNm,
                                                             MethodAttributes.Public | MethodAttributes.HideBySig,
                                                             CallingConventions.HasThis, returnType, parameterTypes);
            return builder;
        }

        public MethodBuilder GetMethod(TypeBuilder typeBuilder, string methodNm, Type returnType, string[] genericParameters, params Type[] parameterTypes)
        {
            MethodBuilder builder = typeBuilder.DefineMethod(methodNm,
                                                             MethodAttributes.Public | MethodAttributes.HideBySig,
                                                             CallingConventions.HasThis, returnType, parameterTypes);

            GenericTypeParameterBuilder[] genBuilders = builder.DefineGenericParameters(genericParameters);

            foreach (var genBuilder in genBuilders)
            {
                genBuilder.SetGenericParameterAttributes(GenericParameterAttributes.ReferenceTypeConstraint |
                                                         GenericParameterAttributes.DefaultConstructorConstraint);
            }
            return builder;
        }
   

        public void CreateBuilder(Type parentBuilder)
        {
            //Step 1:  Create the Assembly
            AssemblyBuilder asmBuilder = this.GetAssemblyBuilder("MyBuilder");

            //Step 2: Add A Module to the Assembly
            ModuleBuilder mbuilder = this.GetModule(asmBuilder);

            //Step 3: Implement parentBuilder to crete Builder
            Type builder = this.CreateBuilderImpl(mbuilder, parentBuilder);

            asmBuilder.Save("MyBuilder.dll");

            dynamic instance =  Activator.CreateInstance("MyBuilder", "Builder", new object[]{});
            var ret = instance.GetIndex();
            Console.WriteLine(ret);
        }

        private Type CreateBuilderImpl(ModuleBuilder mbuilder, Type parentBuilder)
        {
            Type[] interfaces = {parentBuilder};
            TypeBuilder tbuilder = mbuilder.DefineType("Builder", TypeAttributes.Public |
                                                                  TypeAttributes.AutoClass |
                                                                  TypeAttributes.AnsiClass |
                                                                  TypeAttributes.BeforeFieldInit,
                                                                  typeof(System.Object),
                                                                  interfaces);

            ConstructorBuilder cBuilder = tbuilder.DefineConstructor(MethodAttributes.Public,
                                                                     CallingConventions.Standard, Type.EmptyTypes);
            ConstructorInfo conObj = typeof (object).GetConstructor(new Type[0]);

            ILGenerator cil = cBuilder.GetILGenerator();
            cil.Emit(OpCodes.Ldarg_0);
            cil.Emit(OpCodes.Call, conObj);
            cil.Emit(OpCodes.Ret);

            MethodBuilder mGetIndex = tbuilder.DefineMethod("GetIndex", MethodAttributes.Public |
                                                                        MethodAttributes.HideBySig |
                                                                        MethodAttributes.NewSlot |
                                                                        MethodAttributes.Virtual |
                                                                        MethodAttributes.Final,
                                                            CallingConventions.Standard,
                                                            typeof (System.String),
                                                            Type.EmptyTypes);
            mGetIndex.SetImplementationFlags(MethodImplAttributes.Managed);

            ILGenerator il = mGetIndex.GetILGenerator();
            Label label = il.DefineLabel();
            
            il.Emit(OpCodes.Nop);
            il.Emit(OpCodes.Ldstr, "Hello World");
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Br_S, label);
            il.Emit(OpCodes.Ldloc_0);
            il.MarkLabel(label);
            il.Emit(OpCodes.Ret);

            return tbuilder.CreateType();
        }
    }
}
