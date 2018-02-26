using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace CecilTest2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*********************************************************");
            Console.WriteLine(" npcc - NEO Class Framework (NPC) 2.0 Compiler v"  + Assembly.GetEntryAssembly().GetName().Version);
            Console.WriteLine("*********************************************************");
            DirectoryInfo di = new DirectoryInfo(@"..\..\...\NPCPoint0\bin\debug");
            foreach (FileInfo fi in di.GetFiles("*.*"))
            {
                if (fi.FullName.EndsWith(".dll"))
                {
                    Console.WriteLine("File " + fi.FullName);
                    var module = ModuleDefinition.ReadModule(fi.FullName);

                    Console.WriteLine("Module Name:\t" + module.Name);
                    Console.WriteLine("  Module FullyQualifiedName:\t" + module.FullyQualifiedName);
                    Console.WriteLine("  Module RuntimeVersion:\t" + module.RuntimeVersion);
                    Console.WriteLine("  Module HasExportedTypes:\t" + module.HasExportedTypes.ToString());
                    Console.WriteLine("  Module HasTypes:\t" + module.HasTypes.ToString());
                    Console.WriteLine("  Module HasCustomAttributes:\t" + module.HasCustomAttributes.ToString());
                    Console.WriteLine("  Module HasModuleReferences:\t" + module.HasModuleReferences.ToString());
                    Console.WriteLine("  Module HasAssemblyReferences:\t" + module.HasAssemblyReferences.ToString());

                    foreach (AssemblyNameReference ar in module.AssemblyReferences)
                    {
                        Console.WriteLine("Assembly Reference Name:\t" + ar.Name);
                        Console.WriteLine("  Assembly Reference FullName:\t" + ar.FullName);
                        Console.WriteLine("  Assembly Reference Version:\t" + ar.Version.ToString());
                        //Console.WriteLine("Assembly IsWindowsRuntime:\t" + ar.IsWindowsRuntime.ToString());
                    }

                    Console.WriteLine("Module Types...");
                    foreach (TypeDefinition t in module.Types)
                    {
                        Console.WriteLine("Type Name:\t" + t.Name);
                        Console.WriteLine("  t.Fullname:\t" + t.FullName);
                        Console.WriteLine("  t.IsAbstract:\t" + t.IsAbstract.ToString());
                        Console.WriteLine("  t.IsClass:\t" + t.IsClass.ToString());
                        Console.WriteLine("  t.BaseType:\t" + (t.BaseType == null ? "<null>" : t.BaseType.ToString()));
                        Console.WriteLine("  t.DeclaringType:\t" + (t.DeclaringType == null ? "<null>" : t.DeclaringType.ToString()));
                        Console.WriteLine("  t.IsEnum:\t" + t.IsEnum.ToString());
                        Console.WriteLine("  t.IsInterface:\t" + t.IsInterface.ToString());
                        Console.WriteLine("  t.IsNotPublic:\t" + t.IsNotPublic.ToString());
                        Console.WriteLine("  t.IsPublic:\t" + t.IsPublic.ToString());
                        Console.WriteLine("  t.HasInterfaces:\t" + t.HasInterfaces.ToString());
                        Console.WriteLine("  t.HasNestedTypes:\t" + t.HasNestedTypes.ToString());
                        Console.WriteLine("  t.HasCustomAttributes:\t" + t.HasCustomAttributes.ToString());
                        Console.WriteLine("  t.HasFields:\t" + t.HasFields.ToString());
                        Console.WriteLine("  t.HasMethods:\t" + t.HasMethods.ToString());
                        Console.WriteLine("  t.HasProperties:\t" + t.HasProperties.ToString());
                        Console.WriteLine("  t.HasMethods:\t" + t.HasProperties.ToString());

                        if (t.BaseType != null)
                        {
                            TypeReference trBaseType = t.BaseType;
                            Console.WriteLine("    trBaseType Name:\t" + trBaseType.Name);
                            //Console.WriteLine("    trBaseType DeclaringType:\t" + trBaseType.DeclaringType.ToString());
                        }

                        Console.WriteLine("Type Interfaces...");
                        foreach (var i in t.Interfaces)
                        {
                            Console.WriteLine("    Field Name:\t" + i.Name);
                            Console.WriteLine("      f.Fullname:\t" + i.FullName);
                            //Console.WriteLine("      f.Module.FullyQualifiedName:\t" + i.Module.FullyQualifiedName);
                            Console.WriteLine("      f.DeclaringType:\t" + (i.DeclaringType == null ? "<null>" : i.DeclaringType.ToString()));
                        }

                        Console.WriteLine("Type Fields...");
                        foreach (FieldDefinition f in t.Fields)
                        {
                            Console.WriteLine("    Field Name:\t" + f.Name);
                            Console.WriteLine("      f.Fullname:\t" + f.FullName);
                            //Console.WriteLine("      f.Module.FullyQualifiedName:\t" + f.Module.FullyQualifiedName);
                            Console.WriteLine("      f.DeclaringType:\t" + f.DeclaringType.ToString());
                            Console.WriteLine("      f.FieldType:\t" + f.FieldType.ToString());
                            Console.WriteLine("      f.IsPrivate:\t" + f.IsPrivate.ToString());
                            Console.WriteLine("      f.IsPublic:\t" + f.IsPublic.ToString());
                            Console.WriteLine("      f.InitialValue.Length:\t" + f.InitialValue.Length.ToString());
                        }

                        Console.WriteLine("Type Methods...");
                        foreach (MethodDefinition m in t.Methods)
                        {
                            Console.WriteLine("    Method Name:\t" + m.Name);
                            Console.WriteLine("      m.Fullname:\t" + m.FullName);
                            //Console.WriteLine("      m.Module.FullyQualifiedName:\t" + m.Module.FullyQualifiedName);
                            Console.WriteLine("      m.ReturnType:\t" + m.ReturnType.ToString());
                            PrintMethods(m);
                            PrintFields(m);
                        }
                    }
                }
            }

            //var type = module.Types.First(x => x.Name == "A");
            //var method = type.Methods.First(x => x.Name == "test");

            //PrintMethods(method);
            //PrintFields(method);

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }

        public static void PrintMethods(MethodDefinition method)
        {
            Console.WriteLine("      Methods called by:\t" + method.Name);
            if (method.Body != null && method.Body.Instructions != null) foreach (var instruction in method.Body.Instructions)
                {
                    if (instruction.OpCode == OpCodes.Call)
                    {
                        MethodReference methodCalled = instruction.Operand as MethodReference;
                        if (methodCalled != null)
                        {
                            Console.WriteLine("\tmethodCalled.Name:\t" + methodCalled.Name);
                            Console.WriteLine("\t  mc.Name:\t" + methodCalled.FullName);
                            //Console.WriteLine("\t  mc.Module.FullyQualifiedName:\t" + methodCalled.Module.FullyQualifiedName);
                            Console.WriteLine("\t  mc.ReturnType:\t" + methodCalled.ReturnType.ToString());
                        }
                    }
                }
        }

        public static void PrintFields(MethodDefinition method)
        {
            Console.WriteLine("      Fields in " + method.Name);
            if(method.Body != null && method.Body.Instructions != null) foreach (var instruction in method.Body.Instructions)
            {
                if (instruction.OpCode == OpCodes.Ldfld)
                {
                    FieldReference field = instruction.Operand as FieldReference;
                    if (field != null)
                    {
                        Console.WriteLine("\tfield.Name:\t" + field.Name);
                        Console.WriteLine("\t  f.FullName:\t" + field.FullName);
                        Console.WriteLine("\t  f.FieldType:\t" + field.FieldType.ToString());
                    }
                }
            }
        }
    }
}
