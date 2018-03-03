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
        NPCLevel8Auditable,
        NPCEndMarker
    }
    public enum NPCLevelsForFileNames
    {
        L0Basic_cs,
        L1Managed_cs,
        L2Persistable_cs,
        L3Deletable_cs,
        L4Collectible_cs,
        L5Extendible_cs,
        L6Authorized_cs,
        L7Optimized_cs,
        L8Auditable_cs
    }


    public class NPCModuleInfo
    {
        public string moduleInputName;
        public string moduleFileFullyQualifiedName;
        public string moduleProjectName;
        public string moduleFullyQualifiedProjectFolder;
        public string moduleFullyQualifiedRepositoryFolder;
        public string moduleTargetProjectName;
        public string moduleTargetFullyQualifiedProjectFolder;

        public NPCModuleInfo(string name, string moduleFullyQualifiedName)
        {
            Debug.Assert(!String.IsNullOrEmpty(name), "name");
            Debug.Assert(!String.IsNullOrEmpty(moduleFullyQualifiedName), "moduleFullyQualifiedName");

            moduleInputName = name;
            moduleFileFullyQualifiedName = moduleFullyQualifiedName;

            int binPos = moduleFileFullyQualifiedName.IndexOf("\\bin");
            int projectPos = moduleFileFullyQualifiedName.Substring(0, binPos).LastIndexOf("\\");

            moduleProjectName = moduleFileFullyQualifiedName.Substring(projectPos + 1, binPos - projectPos - 1);
            moduleFullyQualifiedProjectFolder = moduleFileFullyQualifiedName.Substring(0, binPos - 1);
            moduleFullyQualifiedRepositoryFolder = moduleFileFullyQualifiedName.Substring(0, projectPos);

            Console.WriteLine("**INFO*** NPCModuleInfo:\t'" + name + "', '" + moduleFileFullyQualifiedName);
            Console.WriteLine("**INFO*** NPCModuleInfo:\t'" + moduleProjectName + "', '" + moduleFullyQualifiedProjectFolder + "', '" + moduleFullyQualifiedRepositoryFolder + "'");
        }

        public string SetTargetProjectName(string snamespace)
        {
            moduleTargetProjectName = snamespace + ".Main";
            moduleTargetFullyQualifiedProjectFolder = moduleFullyQualifiedRepositoryFolder + "\\" + moduleTargetProjectName;
            Console.WriteLine("**INFO*** NPCModuleInfo:\t'" + moduleTargetProjectName + "', '" + moduleTargetFullyQualifiedProjectFolder + "'");
            return moduleTargetProjectName;
        }
    }

    public class NPCReferencedAssemblyInfo
    {
        public string assemblyInputName;

        public NPCReferencedAssemblyInfo(string name)
        {
            Debug.Assert(!String.IsNullOrEmpty(name), "name");

            assemblyInputName = name;

            Console.WriteLine("**INFO*** NPCAssemblyInfo:\t" + name);
        }
    }

    public class NPCClassInfo
    {
        public string classInputName;
        public string classOutputName;
        public string classNamespace;

        public NPCClassInfo(string name, string snamespace)
        {
            Debug.Assert(!String.IsNullOrEmpty(name), "name");
            Debug.Assert(!String.IsNullOrEmpty(snamespace), "snamespace");

            classInputName = name;
            classOutputName = name.Substring(0, 1).ToUpper() + name.Substring(1);
            classNamespace = snamespace;

            Console.WriteLine("**INFO*** NPCClassInfo:\t" + name + " " + snamespace);
        }
    }

    public class NPCFieldInfo
    {
        public int fieldClassIndex;
        public string fieldInputName; // x
        public string fieldPrivateFieldName; // _x
        public string fieldPublicFieldName; // X
        public string fieldInputType;
        public string fieldOutputType;

        public NPCFieldInfo(string name, string type, int classIndex)
        {
            Debug.Assert(!String.IsNullOrEmpty(name), "name");
            Debug.Assert(!String.IsNullOrEmpty(type), "type");

            switch (type)
            {
                case "System.Numerics.BigInteger":
                    {
                        fieldClassIndex = classIndex;
                        fieldInputName = name;
                        fieldPrivateFieldName = "_" + name.Substring(0, 1).ToLower() + name.Substring(1);
                        fieldPublicFieldName = name.Substring(0, 1).ToUpper() + name.Substring(1);
                        fieldInputType = type;
                        fieldOutputType = "BigInteger";
                        Console.WriteLine("**INFO*** NPCFieldInfo:\t" + name + ", " + type + ", " + fieldOutputType);
                        break;
                    }
                case "System.String":
                    {
                        fieldClassIndex = classIndex;
                        fieldInputName = name;
                        fieldPrivateFieldName = "_" + name.Substring(0, 1).ToLower() + name.Substring(1);
                        fieldPublicFieldName = name.Substring(0, 1).ToUpper() + name.Substring(1);
                        fieldInputType = type;
                        fieldOutputType = "string";
                        Console.WriteLine("**INFO*** NPCFieldInfo:\t" + name + ", " + type + ", " + fieldOutputType);
                        break;
                    }
                case "System.Byte[]":
                    {
                        fieldClassIndex = classIndex;
                        fieldInputName = name;
                        fieldPrivateFieldName = "_" + name.Substring(0, 1).ToLower() + name.Substring(1);
                        fieldPublicFieldName = name.Substring(0, 1).ToUpper() + name.Substring(1);
                        fieldInputType = type;
                        fieldOutputType = "byte[]";
                        Console.WriteLine("**INFO*** NPCFieldInfo:\t" + name + ", " + type + ", " + fieldOutputType);
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
        public const string NeoConvertTaskDllName = "Neo.ConvertTask.dll";

        public const string NeoEntityModel_csName = "NeoEntityModel_cs.txt";
        public const string NeoTrace_csName = "NeoTrace_cs.txt";
        public const string NeoStorageKey_csName = "NeoStorageKey_cs.txt";
        public const string NeoVersionedAppUser_csName = "NeoVersionedAppUser_cs.txt";

        public const string NPCLevel0Part1_csName = "NPCLevel0Part1_cs.txt";
        public const string NPCLevel0Part2_csName = "NPCLevel0Part2_cs.txt";

        public const string NPCLevel1Part1_csName = "NPCLevel1Part1_cs.txt";
        public const string NPCLevel1Part2_csName = "NPCLevel1Part2_cs.txt";
        public const string NPCLevel1SetXGetX_csName = "NPCLevel1SetXGetX_cs.txt";
        public const string NPCLevel1Set_csName   = "NPCLevel1Set_cs.txt";

        public const string NPCLevel2Part1_csName = "NPCLevel2Part1_cs.txt";
        public const string NPCLevel2Part2_csName = "NPCLevel2Part2_cs.txt";
        public const string NPCLevel2AFieldConsts_csName = "NPCLevel2AFieldConsts_cs.txt";
        public const string NPCLevel2BMissing_csName = "NPCLevel2BMissing_cs.txt";
        public const string NPCLevel2CPut_csName = "NPCLevel2CPut_cs.txt";
        public const string NPCLevel2DPut_csName = "NPCLevel2DPut_cs.txt";
        public const string NPCLevel2EPut_csName = "NPCLevel2EPut_cs.txt";
        public const string NPCLevel2FGet_csName = "NPCLevel2FGet_cs.txt";
        public const string NPCLevel2GGet_csName = "NPCLevel2GGet_cs.txt";
        public const string NPCLevel2HGet_csName = "NPCLevel2HGet_cs.txt";
        public const string NPCLevel2IGet_csName = "NPCLevel2IGet_cs.txt";

        public const string NPCLevel3Part1_csName = "NPCLevel3Part1_cs.txt";
        public const string NPCLevel3Part2_csName = "NPCLevel3Part2_cs.txt";
        public const string NPCLevel3ABury_csName = "NPCLevel3ABury_cs.txt";
        public const string NPCLevel3BBury_csName = "NPCLevel3BBury_cs.txt";
        public const string NPCLevel3CBury_csName = "NPCLevel3CBury_cs.txt";

        public const string NPCLevel4Part1_csName = "NPCLevel4Part1_cs.txt";
        public const string NPCLevel4Part2_csName = "NPCLevel4Part2_cs.txt";
        public const string NPCLevel4APutElement_csName = "NPCLevel4APutElement_cs.txt";
        public const string NPCLevel4BGetElement_csName = "NPCLevel4BGetElement_cs.txt";
        public const string NPCLevel4CGetElement_csName = "NPCLevel4CGetElement_cs.txt";
        public const string NPCLevel4DBuryElement_csName = "NPCLevel4DBuryElement_cs.txt";
        public const string NPCLevel4EBuryElement_csName = "NPCLevel4EBuryElement_cs.txt";

        public static string[] listDefaultAssemblies = { "Neo.SmartContract.Framework", "Neo.SmartContract.Framework.Services.System", "System.Numerics" };

        public List<NPCModuleInfo> listModuleInfo = null;
        public List<NPCReferencedAssemblyInfo> listAssemblyInfo = null;
        public List<NPCClassInfo> listClassInfo = null;
        public List<NPCFieldInfo> listFieldInfo = null;
        public List<NPCInterfaceInfo> listInterfaceInfo = null;

        public NPCCompilerContext()
        {
            listModuleInfo = new List<NPCModuleInfo>();
            listAssemblyInfo = new List<NPCReferencedAssemblyInfo>();
            listClassInfo = new List<NPCClassInfo>();
            listFieldInfo = new List<NPCFieldInfo>();
            listInterfaceInfo = new List<NPCInterfaceInfo>();

            foreach (string arName in listDefaultAssemblies)
            {
                listAssemblyInfo.Add(new NPCReferencedAssemblyInfo(arName));
            }
        }
    }

    class Program
    {
        public static string ProgramName = "npcc - NEO Class Framework (NPC) 2.0 Compiler";
        
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

                ctx.listModuleInfo.Add(new NPCModuleInfo(module.Name, module.FullyQualifiedName));

                foreach (AssemblyNameReference ar in module.AssemblyReferences)
                {
                    Console.WriteLine("Assembly Reference Name:\t" + ar.Name);
                    Console.WriteLine("  Assembly Reference FullName:\t" + ar.FullName);
                    Console.WriteLine("  Assembly Reference Version:\t" + ar.Version.ToString());
                    //Console.WriteLine("Assembly IsWindowsRuntime:\t" + ar.IsWindowsRuntime.ToString());

                    // Add it if it is not already there
                    NPCReferencedAssemblyInfo arFind = ctx.listAssemblyInfo.Find(
                    delegate (NPCReferencedAssemblyInfo dar)
                    {
                        return (dar.assemblyInputName == ar.Name);
                    });
                    if (arFind == null && ar.Name != "mscorlib") ctx.listAssemblyInfo.Add(new NPCReferencedAssemblyInfo(ar.Name));
                };

                int classIndex = 0;
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
                    Console.WriteLine("  t.Namespace:\t" + t.Namespace);
                   

                    if (t.IsClass & t.Name != "<Module>")
                    {
                        ctx.listClassInfo.Add(new NPCClassInfo(t.Name, t.Namespace));
                        ctx.listModuleInfo[0].SetTargetProjectName(t.Namespace);
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
                            ctx.listFieldInfo.Add(new NPCFieldInfo(f.Name, f.FieldType.ToString(), classIndex));
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
                    if (t.IsClass & t.Name != "<Module>")
                    {
                        classIndex++;
                    }
                }
            }
            return success;
        }

        private static bool ValidateAssembly(NPCCompilerContext ctx)
        {
            bool success = true;

            if (ctx.listModuleInfo.Count != 1)
            {
                string message = "**ERROR** Input assembly file must only contain 1 module definition/file. Not " + ctx.listModuleInfo.Count.ToString();
                Console.WriteLine(message);
                success = false;
            }

            if (ctx.listAssemblyInfo.Count < NPCCompilerContext.listDefaultAssemblies.Length)
            {
                string message = "**ERROR** ctx.listAssemblyInfo.Count < NPCCompilerContext.listDefaultAssemblies.Length. Not " +
                                 NPCCompilerContext.listDefaultAssemblies.Length.ToString();
                Console.WriteLine(message);
                success = false;
            }

            if (ctx.listClassInfo.Count < 1)
            {
                string message = "**ERROR** Input assembly file must contain 1 or more class definitions. Not " + ctx.listClassInfo.Count.ToString();
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
            Console.WriteLine(" " + ProgramName + " " + Assembly.GetEntryAssembly().GetName().Version.ToString());
            Console.WriteLine("*********************************************************");

            NPCCompilerContext ctx = new NPCCompilerContext();

            bool success = false;
            DirectoryInfo di = new DirectoryInfo(@"..\..\...\NPC.TestCases.T1\bin\debug");
            foreach (FileInfo fi in di.GetFiles("*.dll"))
            {
                success = ParseAssembly(ctx, fi);
                if (!success) throw new ArgumentException("Bad input assembly file (DLL): parse failed", fi.FullName);
                Console.WriteLine("**INFO*** Parsing succeeded:\t" + fi.FullName);

                success = ValidateAssembly(ctx);
                if (!success) throw new ArgumentException("Bad input assembly file (DLL): validation failed", fi.FullName);
                Console.WriteLine("**INFO*** Assembly validation succeeded:\t" + fi.FullName);

                success = ValidateTargetProjectEnvironment(ctx);
                if (!success) throw new ArgumentException("Bad target project environment/prerequisites: validation failed", fi.FullName);
                Console.WriteLine("**INFO*** Target project environment/prerequisites validation succeeded:\t" + fi.FullName);

                for (int classIndex = 0; classIndex < ctx.listClassInfo.Count; classIndex++)
                {
                    success = GenCode.GenerateCodeLevel0Basic(ctx, classIndex);
                    if (!success) throw new ArgumentException("Bad input assembly file (DLL): code generation failed", NPCLevels.NPCEndMarker.ToString());
                    Console.WriteLine("**INFO*** Code generation succeeded:\t" + NPCLevels.NPCEndMarker.ToString());

                    success = GenCode.GenerateCodeLevel1Managed(ctx, classIndex);
                    if (!success) throw new ArgumentException("Bad input assembly file (DLL): code generation failed", NPCLevels.NPCEndMarker.ToString());
                    Console.WriteLine("**INFO*** Code generation succeeded:\t" + NPCLevels.NPCEndMarker.ToString());

                    success = GenCode.GenerateCodeLevel2Persistable(ctx, classIndex);
                    if (!success) throw new ArgumentException("Bad input assembly file (DLL): code generation failed", NPCLevels.NPCEndMarker.ToString());
                    Console.WriteLine("**INFO*** Code generation succeeded:\t" + NPCLevels.NPCEndMarker.ToString());

                    success = GenCode.GenerateCodeLevel3Deletable(ctx, classIndex);
                    if (!success) throw new ArgumentException("Bad input assembly file (DLL): code generation failed", NPCLevels.NPCEndMarker.ToString());
                    Console.WriteLine("**INFO*** Code generation succeeded:\t" + NPCLevels.NPCEndMarker.ToString());

                    success = GenCode.GenerateCodeLevel4Collectible(ctx, classIndex);
                    if (!success) throw new ArgumentException("Bad input assembly file (DLL): code generation failed", NPCLevels.NPCEndMarker.ToString());
                    Console.WriteLine("**INFO*** Code generation succeeded:\t" + NPCLevels.NPCEndMarker.ToString());
                }
            }

            //var type = module.Types.First(x => x.Name == "A");
            //var method = type.Methods.First(x => x.Name == "test");

            //PrintMethods(method);
            //PrintFields(method);

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }

        private static bool ValidateTargetProjectEnvironment(NPCCompilerContext ctx)
        {
            bool success = true;
            // Scaffolding: Assume the developer has created an empty NeoContract C# project with the name "NPC" + className + "dApp"
            DirectoryInfo di = new DirectoryInfo(ctx.listModuleInfo[0].moduleTargetFullyQualifiedProjectFolder);
            FileInfo[] files = di.GetFiles("*.dll");
            foreach (FileInfo fi in files)
            {
                Console.WriteLine("**INFO*** ValidateEnvironment:\t" + fi.FullName);
            }

            if (files.Length != 1)
            {
                string message = "**ERROR** Project needs to be a standard NeoContract project with containing 1 DLL in the project folder. Not " + files.Length.ToString();
                Console.WriteLine(message);
                success = false;
            }

            if (files[0].Name != NPCCompilerContext.NeoConvertTaskDllName)
            {
                string message = "**ERROR** Project needs to be a standard NeoContract project with containing " + NPCCompilerContext.NeoConvertTaskDllName + ". Not " + files[0].Name;
                Console.WriteLine(message);
                success = false;
            }

            return success;
        }
    }
}
