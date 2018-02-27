using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace npcc
{
    public enum NPCLevels
    {
        // NPCLevel0Basic, NPCLevel1Managed, NPCLevel2Persistable, NPCLevel3Deletable, NPCLevel4Collectible, NPCLevel5Extendible, NPCLevel6Authorized, NPCLevel7Optimized
        NPCLevel0Basic,
        NPCLevel1Managed,
        NPCLevel2Persistable,
        NPCLevel3Deletable,
        NPCLevel4Collectible,
        NPCLevel5Extendible,
        NPCLevel6Authorized,
        NPCLevel7Optimized,
        NPCEndMarker
    }

    public class NPCAssemblyInfo
    {
        public string assemblyInputName;
        public string moduleFileFullyQualifiedName;

        public NPCAssemblyInfo(string name,string moduleFullyQualifiedName)
        {
            Debug.Assert(!String.IsNullOrEmpty(name), "name");

            assemblyInputName = name;
            moduleFileFullyQualifiedName = moduleFullyQualifiedName;

            Console.WriteLine("**INFO*** NPCAssemblyInfo:\t" + name, ", " + moduleFileFullyQualifiedName);
        }
    }

    public class NPCClassInfo
    {
        string classInputName;
        string classOutputName;

        public NPCClassInfo(string name)
        {
            Debug.Assert(!String.IsNullOrEmpty(name), "name");

            classInputName = name;
            classOutputName = name.Substring(0, 1).ToUpper() + name.Substring(1);

            Console.WriteLine("**INFO*** NPCClassInfo:\t" + name);
        }
    }

    public class NPCFieldInfo
    {
        string fieldInputName; // x
        string fieldPrivateFieldName; // _x
        string fieldPublicFieldName; // X
        string fieldType;

        public NPCFieldInfo(string name, string type)
        {
            Debug.Assert(!String.IsNullOrEmpty(name), "name");
            Debug.Assert(!String.IsNullOrEmpty(type), "type");

            switch (type)
            {
                case "System.Numerics.BigInteger":
                case "System.String":
                case "System.Byte[]":
                    {
                        fieldInputName = name;
                        fieldPrivateFieldName = "_" + name.Substring(0, 1).ToLower() + name.Substring(1);
                        fieldPublicFieldName = name.Substring(0, 1).ToUpper() + name.Substring(1);
                        fieldType = type;
                        Console.WriteLine("**INFO*** NPCFieldInfo:\t" + name + ", " + type);
                        break;
                    }
                default:
                    {   string message  = "**ERROR** Field type '" + type + "' is not supported in C#.NPC. Use BigInteger, byte[], or string.";
                        Console.WriteLine(message);
                        throw new ArgumentOutOfRangeException(fieldInputName, message);
                        //break;
                    }
            }
        }
    }

    public class NPCInterfaceInfo
    {
        string interfaceInputName;
        string interfaceOutputName;

        public NPCInterfaceInfo(string name)
        {
            Debug.Assert(!String.IsNullOrEmpty(name), "name");

            NPCLevels level;
            if (!Enum.TryParse<NPCLevels>(name, out level))
            {
                string message = "**ERROR** Interface level name '" + name + "' is not supported in C#.NPC. Use NPCLevel0Basic, NPCLevel1Managed, NPCLevel2Persistable, NPCLevel3Deletable, NPCLevel4Collectible, NPCLevel5Extendible, NPCLevel6Authorized, or NPCLevel7Optimized.";
                Console.WriteLine(message);
                throw new ArgumentOutOfRangeException(name, message);
            }
            else
            {
                switch (level)
                {
                    case NPCLevels.NPCLevel0Basic:
                    case NPCLevels.NPCLevel1Managed:
                    case NPCLevels.NPCLevel2Persistable:
                    case NPCLevels.NPCLevel3Deletable:
                    case NPCLevels.NPCLevel4Collectible:
                    case NPCLevels.NPCLevel5Extendible:
                    case NPCLevels.NPCLevel6Authorized:
                    case NPCLevels.NPCLevel7Optimized:
                        {
                            interfaceInputName = name;
                            interfaceOutputName = name.Substring(0, 1).ToUpper() + name.Substring(1);
                            Console.WriteLine("**INFO*** NPCInterfaceInfo:\t" + name);
                            break;
                        }
                    default:
                        {
                            string message = "**ERROR** Unexpected interface level name '" + name + "' is not supported in C#.NPC. Use NPCLevel1, NPCLevel2, NPCLevel3, NPCLevel4, NPCLevel5, NPCLevel6, or NPCLevel7.";
                            Console.WriteLine(message);
                            throw new ArgumentOutOfRangeException(name, message);
                            //break;
                        }
                }
            }
        }
    }

    public class NPCCompilerContext
    {
        public static string[] listDefaultAssemblies = { "Neo.SmartContract.Framework", "Neo.SmartContract.Framework.Services.System", "System.Numerics" };

        public List<NPCAssemblyInfo> listAssemblyInfo = null;
        public List<NPCClassInfo> listClassInfo = null;
        public List<NPCFieldInfo> listFieldInfo = null;
        public List<NPCInterfaceInfo> listInterfaceInfo = null;

        public NPCCompilerContext()
        {
            listAssemblyInfo = new List<NPCAssemblyInfo>();
            listClassInfo = new List<NPCClassInfo>();
            listFieldInfo = new List<NPCFieldInfo>();
            listInterfaceInfo = new List<NPCInterfaceInfo>();

            foreach (string arName in listDefaultAssemblies)
            {
                listAssemblyInfo.Add(new NPCAssemblyInfo(arName, ""));
            }
        }
    }

    class Program
    {
        private static bool ParseAssembly(NPCCompilerContext ctx, FileInfo fi)
        {
            bool success = true;

            if (!fi.FullName.EndsWith(".dll"))
            {
                success = false;
            }
            else
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

                    // Add it if it is not already there
                    NPCAssemblyInfo arFind = ctx.listAssemblyInfo.Find(
                    delegate (NPCAssemblyInfo dar)
                    {
                        return (dar.assemblyInputName == ar.Name);
                    });
                    if (arFind == null && ar.Name != "mscorlib") ctx.listAssemblyInfo.Add(new NPCAssemblyInfo(ar.Name, module.FullyQualifiedName));
                };

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

                    if (t.IsClass & t.Name != "<Module>")
                    {
                        ctx.listClassInfo.Add(new NPCClassInfo(t.Name));
                    }

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

                        if (t.IsClass & t.Name != "<Module>")
                        {
                            ctx.listInterfaceInfo.Add(new NPCInterfaceInfo(i.Name));
                        }
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

                        if (t.IsClass & t.Name != "<Module>")
                        {
                            ctx.listFieldInfo.Add(new NPCFieldInfo(f.Name, f.FieldType.ToString()));
                        }
                    }

                    Console.WriteLine("Type Methods...");
                    foreach (MethodDefinition m in t.Methods)
                    {
                        Console.WriteLine("    Method Name:\t" + m.Name);
                        Console.WriteLine("      m.Fullname:\t" + m.FullName);
                        //Console.WriteLine("      m.Module.FullyQualifiedName:\t" + m.Module.FullyQualifiedName);
                        Console.WriteLine("      m.ReturnType:\t" + m.ReturnType.ToString());
                        Helpers.PrintMethods(m);
                        Helpers.PrintFields(m);
                    }
                }
            }
            return success;
        }

        private static bool ValidateAssembly(NPCCompilerContext ctx)
        {
            bool success = true;

            if (ctx.listAssemblyInfo.Count < NPCCompilerContext.listDefaultAssemblies.Length)
            {
                string message = "**ERROR** ctx.listAssemblyInfo.Count < NPCCompilerContext.listDefaultAssemblies.Length. Not " +
                                 NPCCompilerContext.listDefaultAssemblies.Length.ToString();
                Console.WriteLine(message);
                success = false;
            }

            if (ctx.listClassInfo.Count != 1)
            {
                string message = "**ERROR** Input assembly file must only contain 1 class definition. Not " + ctx.listClassInfo.Count.ToString();
                Console.WriteLine(message);
                success = false;
            }

            if (ctx.listFieldInfo.Count < 1)
            {
                string message = "**ERROR** Input assembly file must contain 1 or more field definitions. Not " + ctx.listFieldInfo.Count.ToString();
                Console.WriteLine(message);
                success = false;
            }

            if (ctx.listInterfaceInfo.Count < 1)
            {
                string message = "**ERROR** Input assembly file must contain 1 or more interface definitions. Not " + ctx.listInterfaceInfo.Count.ToString();
                Console.WriteLine(message);
                success = false;
            }

            if (ctx.listInterfaceInfo.Count > (int)NPCLevels.NPCEndMarker)
            {
                string message = "**ERROR** Input assembly file class is derived from too many interfaces. Not " + ctx.listInterfaceInfo.Count.ToString();
                Console.WriteLine(message);
                success = false;
            }

            return success;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("*********************************************************");
            Console.WriteLine(" npcc - NEO Class Framework (NPC) 2.0 Compiler v" + Assembly.GetEntryAssembly().GetName().Version);
            Console.WriteLine("*********************************************************");

            NPCCompilerContext ctx = new NPCCompilerContext();

            bool success = false;
            DirectoryInfo di = new DirectoryInfo(@"..\..\...\NPCPoint0\bin\debug");
            foreach (FileInfo fi in di.GetFiles("*.dll"))
            {
                success = ParseAssembly(ctx, fi);
                if (!success) throw new ArgumentException("Bad input assembly file (DLL): parse failed", fi.FullName);
                Console.WriteLine("**INFO*** Parsing succeeded:\t" + fi.FullName);

                success = ValidateAssembly(ctx);
                if (!success) throw new ArgumentException("Bad input assembly file (DLL): validation failed", fi.FullName);
                Console.WriteLine("**INFO*** Validation succeeded:\t" + fi.FullName);

                success = GenCode.GenerateCodeLevel0Basic(ctx);
                if (!success) throw new ArgumentException("Bad input assembly file (DLL): code generation failed", NPCLevels.NPCEndMarker.ToString());
                Console.WriteLine("**INFO*** Code generation succeeded:\t" + NPCLevels.NPCEndMarker.ToString());

                success = GenCode.GenerateCodeLevel1Managed(ctx);
                if (!success) throw new ArgumentException("Bad input assembly file (DLL): code generation failed", NPCLevels.NPCEndMarker.ToString());
                Console.WriteLine("**INFO*** Code generation succeeded:\t" + NPCLevels.NPCEndMarker.ToString());

                success = GenCode.GenerateCodeLevel2Persistable(ctx);
                if (!success) throw new ArgumentException("Bad input assembly file (DLL): code generation failed", NPCLevels.NPCEndMarker.ToString());
                Console.WriteLine("**INFO*** Code generation succeeded:\t" + NPCLevels.NPCEndMarker.ToString());

                success = GenCode.GenerateCodeLevel3Deletable(ctx);
                if (!success) throw new ArgumentException("Bad input assembly file (DLL): code generation failed", NPCLevels.NPCEndMarker.ToString());
                Console.WriteLine("**INFO*** Code generation succeeded:\t" + NPCLevels.NPCEndMarker.ToString());

                success = GenCode.GenerateCodeLevel4Collectible(ctx);
                if (!success) throw new ArgumentException("Bad input assembly file (DLL): code generation failed", NPCLevels.NPCEndMarker.ToString());
                Console.WriteLine("**INFO*** Code generation succeeded:\t" + NPCLevels.NPCEndMarker.ToString());
            }

            //var type = module.Types.First(x => x.Name == "A");
            //var method = type.Methods.First(x => x.Name == "test");

            //PrintMethods(method);
            //PrintFields(method);

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
