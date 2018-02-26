using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CecilTest2
{
    class Program
    {
        static void Main(string[] args)
        {
            DirectoryInfo di = new DirectoryInfo(@"..\..\Test Assemblies");
            //DirectoryInfo di = new DirectoryInfo(@"..\..\Web Assemblies");
            foreach (FileInfo fi in di.GetFiles("*.*"))
            {
                Console.WriteLine("File " + fi.FullName);
                if (fi.FullName.EndsWith(".dll") || fi.FullName.EndsWith(".exe"))
                {
                    var module = ModuleDefinition.ReadModule(fi.FullName);
                    Console.WriteLine("Module Name:\t" + module.Name);
                    Console.WriteLine("Module FullyQualifiedName:\t" + module.FullyQualifiedName);
                    Console.WriteLine("Module RuntimeVersion:\t" + module.RuntimeVersion);

                    foreach (AssemblyNameReference ar in module.AssemblyReferences)
                    {
                        Console.WriteLine("Assembly Name:\t" + ar.Name);
                        Console.WriteLine("Assembly FullName:\t" + ar.FullName);
                        Console.WriteLine("Assembly Version:\t" + ar.Version.ToString());
                        Console.WriteLine("Assembly IsWindowsRuntime:\t" + ar.IsWindowsRuntime.ToString());
                    }
                    foreach (TypeDefinition t in module.Types)
                    {
                        Console.WriteLine("Type Name:\t" + t.Name);

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
