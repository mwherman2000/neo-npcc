using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Threading;

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
        NPCLevel4CollectibleExt,
        NPCLevel5Extendible,
        NPCLevel6Authorized,
        NPCLevel7Optimized,
        NPCLevel8Auditable,

        NPCLevel0CustomMethods,
        NPCLevel1CustomMethods,
        NPCLevel2CustomMethods,
        NPCLevel3CustomMethods,
        NPCLevel4CustomMethods,

        NPCEndMarker
    }

    public enum NPCLevelsForFileNames
    {
        L0Basic_cs,
        L1Managed_cs,
        L2Persistable_cs,
        L3Deletable_cs,
        L4Collectible_cs,
        L4CollectibleExt_cs,
        L5Extendible_cs,
        L6Authorized_cs,
        L7Optimized_cs,
        L8Auditable_cs,

        L0CustomMethods_cs,
        L1CustomMethods_cs,
        L2CustomMethods_cs,
        L3CustomMethods_cs,
        L4CustomMethods_cs,
    }


    public class NPCModuleInfo
    {
        public string moduleInputName;
        public string moduleDLLFullyQualifiedName;
        public string moduleModelClassProjectName;
        public string moduleModelClassFullyQualifiedProjectFolder;
        public string moduleModelClassFullyQualifiedRepositoryFolder;
        public string moduleTargetProjectName;
        public string moduleTargetFullyQualifiedProjectFolder;

        public NPCModuleInfo(string name, string moduleFullyQualifiedName)
        {
            Debug.Assert(!String.IsNullOrEmpty(name), "name");
            Debug.Assert(!String.IsNullOrEmpty(moduleFullyQualifiedName), "moduleFullyQualifiedName");

            moduleInputName = name;
            moduleDLLFullyQualifiedName = moduleFullyQualifiedName;

            int binPos = moduleDLLFullyQualifiedName.IndexOf("\\bin");
            int projectPos = moduleDLLFullyQualifiedName.Substring(0, binPos).LastIndexOf("\\");

            moduleModelClassProjectName = moduleDLLFullyQualifiedName.Substring(projectPos + 1, binPos - projectPos - 1);
            moduleModelClassFullyQualifiedProjectFolder = moduleDLLFullyQualifiedName.Substring(0, binPos);
            moduleModelClassFullyQualifiedRepositoryFolder = moduleDLLFullyQualifiedName.Substring(0, projectPos);

            if (Trace.Info) Console.WriteLine("**INFO*** NPCModuleInfo:DLL\t'" + name + "', '" + moduleDLLFullyQualifiedName);
            if (Trace.Info) Console.WriteLine("**INFO*** NPCModuleInfo:ModelClass\t'" + moduleModelClassProjectName + "', '" + moduleModelClassFullyQualifiedProjectFolder + "', '" + moduleModelClassFullyQualifiedRepositoryFolder + "'");
        }

        public string SetTargetProjectName(string snamespace)
        {
            moduleTargetProjectName = snamespace + ".Main";
            moduleTargetFullyQualifiedProjectFolder = moduleModelClassFullyQualifiedRepositoryFolder + "\\" + moduleTargetProjectName;
            if (Trace.Info) Console.WriteLine("**INFO*** NPCModuleInfo:Target\t'" + moduleTargetProjectName + "', '" + moduleTargetFullyQualifiedProjectFolder + "'");
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

            if (Trace.Verbose) Console.WriteLine("**VERB*** NPCAssemblyInfo:\t" + name);
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

            if (Trace.Info) Console.WriteLine("**INFO*** NPCClassInfo:\t" + name + " " + snamespace);
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
                        if (Trace.Info) Console.WriteLine("**INFO*** NPCFieldInfo:\t\t" + name + ", " + type + ", " + fieldOutputType);
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
                        if (Trace.Info) Console.WriteLine("**INFO*** NPCFieldInfo:\t\t" + name + ", " + type + ", " + fieldOutputType);
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
                        if (Trace.Info) Console.WriteLine("**INFO*** NPCFieldInfo:\t\t" + name + ", " + type + ", " + fieldOutputType);
                        break;
                    }
                default:
                    {   string message  = "**ERROR** Field type '" + type + "' is not supported in C#.NPC. Use BigInteger, byte[], or string.";
                        if (Trace.Error) Console.WriteLine(message);
                        throw new ArgumentOutOfRangeException(fieldInputName, message);
                        //break;
                    }
            }
        }
    }

    public class NPCClassInterfaceInfo
    {
        public int interfaceClassIndex;
        public string interfaceInputName;
        public string interfaceOutputName;

        public NPCClassInterfaceInfo(string name, int classIndex)
        {
            Debug.Assert(!String.IsNullOrEmpty(name), "name");

            interfaceClassIndex = classIndex;

            NPCLevels level;
            if (!Enum.TryParse<NPCLevels>(name, out level))
            {
                string message = "**ERROR** Interface level name '" + name + "' is not supported in C#.NPC. Use NPCLevel0Basic, NPCLevel1Managed, NPCLevel2Persistable, NPCLevel3Deletable, NPCLevel4Collectible, NPCLevel5Extendible, NPCLevel6Authorized, or NPCLevel7Optimized.";
                if (Trace.Error) Console.WriteLine(message);
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
                    case NPCLevels.NPCLevel4CollectibleExt:
                    case NPCLevels.NPCLevel5Extendible:
                    case NPCLevels.NPCLevel6Authorized:
                    case NPCLevels.NPCLevel7Optimized:

                    case NPCLevels.NPCLevel0CustomMethods:
                    case NPCLevels.NPCLevel1CustomMethods:
                    case NPCLevels.NPCLevel2CustomMethods:
                    case NPCLevels.NPCLevel3CustomMethods:
                    case NPCLevels.NPCLevel4CustomMethods:
                        {
                            interfaceInputName = name;
                            interfaceOutputName = name.Substring(0, 1).ToUpper() + name.Substring(1);
                            if (Trace.Info) Console.WriteLine("**INFO*** NPCInterfaceInfo:\t" + name);
                            break;
                        }
                    default:
                        {
                            string message = "**ERROR** Unexpected interface level name '" + name + "' is not supported in C#.NPC. Use NPCLevel1, NPCLevel2, NPCLevel3, NPCLevel4, NPCLevel5, NPCLevel6, or NPCLevel7.";
                            if (Trace.Error) Console.WriteLine(message);
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

        public const string NPCLevel4Part1Ext_csName = "NPCLevel4Part1Ext_cs.txt";
        public const string NPCLevel4Part2Ext_csName = "NPCLevel4Part2Ext_cs.txt";
        public const string NPCLevel4APutElementExt_csName = "NPCLevel4APutElementExt_cs.txt";
        public const string NPCLevel4BGetElementExt_csName = "NPCLevel4BGetElementExt_cs.txt";
        public const string NPCLevel4CGetElementExt_csName = "NPCLevel4CGetElementExt_cs.txt";
        public const string NPCLevel4DBuryElementExt_csName = "NPCLevel4DBuryElementExt_cs.txt";
        public const string NPCLevel4EBuryElementExt_csName = "NPCLevel4EBuryElementExt_cs.txt";

        public static string[] listDefaultAssemblies = { "Neo.SmartContract.Framework", "Neo.SmartContract.Framework.Services.System", "System.Numerics" };

        public List<NPCModuleInfo> listModuleInfo = null;
        public List<NPCReferencedAssemblyInfo> listAssemblyInfo = null;
        public List<NPCClassInfo> listClassInfo = null;
        public List<NPCFieldInfo> listFieldInfo = null;
        public List<NPCClassInterfaceInfo> listClassInterfaceInfo = null;

        public NPCCompilerContext()
        {
            listModuleInfo = new List<NPCModuleInfo>();
            listAssemblyInfo = new List<NPCReferencedAssemblyInfo>();
            listClassInfo = new List<NPCClassInfo>();
            listFieldInfo = new List<NPCFieldInfo>();
            listClassInterfaceInfo = new List<NPCClassInterfaceInfo>();

            foreach (string arName in listDefaultAssemblies)
            {
                listAssemblyInfo.Add(new NPCReferencedAssemblyInfo(arName));
            }
        }
    }

    class Program
    {
        public const string ProgramName = "npcc - NEO Class Framework (NPC) 2.0 Compiler";
        public static string pathProjectBinDebug = "";

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            if (Trace.Splash) Console.WriteLine("*********************************************************");
            if (Trace.Splash) Console.WriteLine(" " + ProgramName + " v" + Assembly.GetEntryAssembly().GetName().Version.ToString());
            if (Trace.Splash) Console.WriteLine("*********************************************************");
            if (Trace.Splash) Console.WriteLine();
            
            if (args.Length == 1)
            {
                pathProjectBinDebug = args[0];
            }
            else
            {
                if (Trace.Splash) Console.WriteLine("Usage: " + Assembly.GetEntryAssembly().GetName().Name + " <projectbinfolder>");
                if (Trace.Splash) Console.WriteLine();
                if (Trace.Exit) Console.WriteLine("Press enter to exit...");
                if (Trace.Exit) Console.ReadLine();
                Environment.Exit(0);
            }

            if (Trace.Info) Console.WriteLine("**INFO*** Project bin\\Debug Folder:\t\"" + pathProjectBinDebug + "\"");

            NPCCompilerContext ctx = new NPCCompilerContext();

            bool success = false;
            DirectoryInfo di = new DirectoryInfo(pathProjectBinDebug); //  @"..\..\...\NPC.TestCases.T1\bin\debug"
            foreach (FileInfo fi in di.GetFiles("*.dll"))
            {
                if (Trace.Info) Console.WriteLine("**INFO*** Assembly:\t" + fi.FullName);

                if (Trace.Info) Console.WriteLine();
                success = ParseAssembly(ctx, fi);
                if (!success) throw new ArgumentException("Bad input assembly file (DLL): parse failed", fi.FullName);
                if (Trace.Info) Console.WriteLine("**INFO*** Parsing succeeded:\t" + fi.FullName);

                if (Trace.Info) Console.WriteLine();
                success = ValidateAssembly(ctx);
                if (!success) throw new ArgumentException("Bad input assembly file (DLL): validation failed", fi.FullName);
                if (Trace.Info) Console.WriteLine("**INFO*** Assembly validation succeeded:\t" + fi.FullName);

                if (Trace.Info) Console.WriteLine();
                success = ValidateTargetProjectEnvironment(ctx);
                if (!success) throw new ArgumentException("Bad target project environment/prerequisites: validation failed", fi.FullName);
                if (Trace.Info) Console.WriteLine("**INFO*** Target project environment/prerequisites validation succeeded:\t" + fi.FullName);

                for (int classIndex = 0; classIndex < ctx.listClassInfo.Count; classIndex++)
                {
                    if (Trace.Info) Console.WriteLine();
                    List<NPCClassInterfaceInfo> listClassInterfaces = ctx.listClassInterfaceInfo.FindAll(
                       delegate (NPCClassInterfaceInfo dci)
                       {
                           return (dci.interfaceClassIndex == classIndex && dci.interfaceOutputName == NPCLevels.NPCLevel0Basic.ToString());
                       });
                    if (listClassInterfaces.Count == 1)
                    {
                        success = GenCode.GenerateCodeLevel0Basic(ctx, classIndex);
                        if (!success) throw new ArgumentException("Bad input assembly file (DLL): code generation failed", NPCLevels.NPCLevel0Basic.ToString());
                        if (Trace.Info) Console.WriteLine("**INFO*** Code generation succeeded:\t" + ctx.listClassInfo[classIndex].classOutputName + " \t: " + NPCLevels.NPCLevel0Basic.ToString());
                    }

                    listClassInterfaces = ctx.listClassInterfaceInfo.FindAll(
                       delegate (NPCClassInterfaceInfo dci)
                       {
                           return (dci.interfaceClassIndex == classIndex && dci.interfaceOutputName == NPCLevels.NPCLevel1Managed.ToString());
                       });
                    if (listClassInterfaces.Count == 1)
                    {
                        success = GenCode.GenerateCodeLevel1Managed(ctx, classIndex);
                        if (!success) throw new ArgumentException("Bad input assembly file (DLL): code generation failed", NPCLevels.NPCLevel1Managed.ToString());
                        if (Trace.Info) Console.WriteLine("**INFO*** Code generation succeeded:\t" + ctx.listClassInfo[classIndex].classOutputName + " \t: " + NPCLevels.NPCLevel1Managed.ToString());
                    }

                    listClassInterfaces = ctx.listClassInterfaceInfo.FindAll(
                       delegate (NPCClassInterfaceInfo dci)
                       {
                           return (dci.interfaceClassIndex == classIndex && dci.interfaceOutputName == NPCLevels.NPCLevel2Persistable.ToString());
                       });
                    if (listClassInterfaces.Count == 1)
                    {
                        success = GenCode.GenerateCodeLevel2Persistable(ctx, classIndex);
                        if (!success) throw new ArgumentException("Bad input assembly file (DLL): code generation failed", NPCLevels.NPCLevel2Persistable.ToString());
                        if (Trace.Info) Console.WriteLine("**INFO*** Code generation succeeded:\t" + ctx.listClassInfo[classIndex].classOutputName + " \t: " + NPCLevels.NPCLevel2Persistable.ToString());
                    }

                    listClassInterfaces = ctx.listClassInterfaceInfo.FindAll(
                       delegate (NPCClassInterfaceInfo dci)
                       {
                           return (dci.interfaceClassIndex == classIndex && dci.interfaceOutputName == NPCLevels.NPCLevel3Deletable.ToString());
                       });
                    if (listClassInterfaces.Count == 1)
                    {
                        success = GenCode.GenerateCodeLevel3Deletable(ctx, classIndex);
                        if (!success) throw new ArgumentException("Bad input assembly file (DLL): code generation failed", NPCLevels.NPCLevel3Deletable.ToString());
                        if (Trace.Info) Console.WriteLine("**INFO*** Code generation succeeded:\t" + ctx.listClassInfo[classIndex].classOutputName + " \t: " + NPCLevels.NPCLevel3Deletable.ToString());
                    }

                    listClassInterfaces = ctx.listClassInterfaceInfo.FindAll(
                       delegate (NPCClassInterfaceInfo dci)
                       {
                           return (dci.interfaceClassIndex == classIndex && dci.interfaceOutputName == NPCLevels.NPCLevel4Collectible.ToString());
                       });
                    if (listClassInterfaces.Count == 1)
                    {
                        success = GenCode.GenerateCodeLevel4Collectible(ctx, classIndex);
                        if (!success) throw new ArgumentException("Bad input assembly file (DLL): code generation failed", NPCLevels.NPCLevel4Collectible.ToString());
                        if (Trace.Info) Console.WriteLine("**INFO*** Code generation succeeded:\t" + ctx.listClassInfo[classIndex].classOutputName + " \t: " + NPCLevels.NPCLevel4Collectible.ToString());
                    }

                    listClassInterfaces = ctx.listClassInterfaceInfo.FindAll(
                       delegate (NPCClassInterfaceInfo dci)
                       {
                           return (dci.interfaceClassIndex == classIndex && dci.interfaceOutputName == NPCLevels.NPCLevel4CollectibleExt.ToString());
                       });
                    if (listClassInterfaces.Count == 1)
                    {
                        success = GenCode.GenerateCodeLevel4CollectibleExt(ctx, classIndex);
                        if (!success) throw new ArgumentException("Bad input assembly file (DLL): code generation failed", NPCLevels.NPCLevel4CollectibleExt.ToString());
                        if (Trace.Info) Console.WriteLine("**INFO*** Code generation succeeded:\t" + ctx.listClassInfo[classIndex].classOutputName + " \t: " + NPCLevels.NPCLevel4CollectibleExt.ToString());
                    }

                    listClassInterfaces = ctx.listClassInterfaceInfo.FindAll(
                       delegate (NPCClassInterfaceInfo dci)
                       {
                           return (dci.interfaceClassIndex == classIndex && dci.interfaceOutputName.EndsWith("CustomMethods"));
                       });
                    if (listClassInterfaces.Count >= 1)
                    {
                        success = GenCode.GenerateCodeCustomMethods(ctx, classIndex, listClassInterfaces);
                        if (!success) throw new ArgumentException("Bad input assembly file (DLL): code generation failed", NPCLevels.NPCLevel4CustomMethods.ToString());
                        if (Trace.Info) Console.WriteLine("**INFO*** Code generation succeeded:\t" + ctx.listClassInfo[classIndex].classOutputName + " \t: " + NPCLevels.NPCLevel4CustomMethods.ToString());
                    }
                }
            }

            //var type = module.Types.First(x => x.Name == "A");
            //var method = type.Methods.First(x => x.Name == "test");

            //if (Trace.Verbose) PrintMethods(method);
            //if (Trace.Verbose) PrintFields(method);

            if (Trace.Exit) Console.WriteLine("Press enter to exit...");
            if (Trace.Exit) Console.ReadLine();
            Environment.Exit(0);
        }

        private static bool ParseAssembly(NPCCompilerContext ctx, FileInfo fi)
        {
            bool success = true;

            if (!fi.FullName.EndsWith(".dll"))
            {
                success = false;
            }
            else
            {
                if (Trace.Verbose) Console.WriteLine("File " + fi.FullName);
                var module = ModuleDefinition.ReadModule(fi.FullName);

                if (Trace.Verbose) Console.WriteLine("Module Name:\t" + module.Name);
                if (Trace.Verbose) Console.WriteLine("  Module FullyQualifiedName:\t" + module.FullyQualifiedName);
                if (Trace.Verbose) Console.WriteLine("  Module RuntimeVersion:\t" + module.RuntimeVersion);
                if (Trace.Verbose) Console.WriteLine("  Module HasExportedTypes:\t" + module.HasExportedTypes.ToString());
                if (Trace.Verbose) Console.WriteLine("  Module HasTypes:\t" + module.HasTypes.ToString());
                if (Trace.Verbose) Console.WriteLine("  Module HasCustomAttributes:\t" + module.HasCustomAttributes.ToString());
                if (Trace.Verbose) Console.WriteLine("  Module HasModuleReferences:\t" + module.HasModuleReferences.ToString());
                if (Trace.Verbose) Console.WriteLine("  Module HasAssemblyReferences:\t" + module.HasAssemblyReferences.ToString());

                ctx.listModuleInfo.Add(new NPCModuleInfo(module.Name, module.FullyQualifiedName));

                foreach (AssemblyNameReference ar in module.AssemblyReferences)
                {
                    if (Trace.Verbose) Console.WriteLine("Assembly Reference Name:\t" + ar.Name);
                    if (Trace.Verbose) Console.WriteLine("  Assembly Reference FullName:\t" + ar.FullName);
                    if (Trace.Verbose) Console.WriteLine("  Assembly Reference Version:\t" + ar.Version.ToString());
                    //if (Trace.Verbose) Console.WriteLine("Assembly IsWindowsRuntime:\t" + ar.IsWindowsRuntime.ToString());

                    // Add it if it is not already there
                    // https://msdn.microsoft.com/en-us/library/x0b5b5bc(v=vs.110).aspx
                    NPCReferencedAssemblyInfo arFind = ctx.listAssemblyInfo.Find(
                    delegate (NPCReferencedAssemblyInfo dar)
                    {
                        return (dar.assemblyInputName == ar.Name);
                    });
                    if (arFind == null && ar.Name != "mscorlib") ctx.listAssemblyInfo.Add(new NPCReferencedAssemblyInfo(ar.Name));
                };

                int classIndex = 0;
                if (Trace.Verbose) Console.WriteLine("Module Types...");
                foreach (TypeDefinition t in module.Types)
                {
                    if (Trace.Verbose) Console.WriteLine("Type Name:\t" + t.Name);
                    if (Trace.Verbose) Console.WriteLine("  t.Fullname:\t" + t.FullName);
                    if (Trace.Verbose) Console.WriteLine("  t.IsAbstract:\t" + t.IsAbstract.ToString());
                    if (Trace.Verbose) Console.WriteLine("  t.IsClass:\t" + t.IsClass.ToString());
                    if (Trace.Verbose) Console.WriteLine("  t.BaseType:\t" + (t.BaseType == null ? "<null>" : t.BaseType.ToString()));
                    if (Trace.Verbose) Console.WriteLine("  t.DeclaringType:\t" + (t.DeclaringType == null ? "<null>" : t.DeclaringType.ToString()));
                    if (Trace.Verbose) Console.WriteLine("  t.IsEnum:\t" + t.IsEnum.ToString());
                    if (Trace.Verbose) Console.WriteLine("  t.IsInterface:\t" + t.IsInterface.ToString());
                    if (Trace.Verbose) Console.WriteLine("  t.IsNotPublic:\t" + t.IsNotPublic.ToString());
                    if (Trace.Verbose) Console.WriteLine("  t.IsPublic:\t" + t.IsPublic.ToString());
                    if (Trace.Verbose) Console.WriteLine("  t.HasInterfaces:\t" + t.HasInterfaces.ToString());
                    if (Trace.Verbose) Console.WriteLine("  t.HasNestedTypes:\t" + t.HasNestedTypes.ToString());
                    if (Trace.Verbose) Console.WriteLine("  t.HasCustomAttributes:\t" + t.HasCustomAttributes.ToString());
                    if (Trace.Verbose) Console.WriteLine("  t.HasFields:\t" + t.HasFields.ToString());
                    if (Trace.Verbose) Console.WriteLine("  t.HasMethods:\t" + t.HasMethods.ToString());
                    if (Trace.Verbose) Console.WriteLine("  t.HasProperties:\t" + t.HasProperties.ToString());
                    if (Trace.Verbose) Console.WriteLine("  t.HasMethods:\t" + t.HasProperties.ToString());
                    if (Trace.Verbose) Console.WriteLine("  t.Namespace:\t" + t.Namespace);
                   

                    if (t.IsClass & t.Name != "<Module>")
                    {
                        ctx.listClassInfo.Add(new NPCClassInfo(t.Name, t.Namespace));
                        ctx.listModuleInfo[0].SetTargetProjectName(t.Namespace);
                    }

                    if (t.BaseType != null)
                    {
                        TypeReference trBaseType = t.BaseType;
                        if (Trace.Verbose) Console.WriteLine("    trBaseType Name:\t" + trBaseType.Name);
                        //if (Trace.Verbose) Console.WriteLine("    trBaseType DeclaringType:\t" + trBaseType.DeclaringType.ToString());
                    }

                    if (Trace.Verbose) Console.WriteLine("Type Interfaces...");
                    foreach (var i in t.Interfaces)
                    {
                        if (Trace.Verbose) Console.WriteLine("    Field Name:\t" + i.Name);
                        if (Trace.Verbose) Console.WriteLine("      f.Fullname:\t" + i.FullName);
                        //if (Trace.Verbose) Console.WriteLine("      f.Module.FullyQualifiedName:\t" + i.Module.FullyQualifiedName);
                        if (Trace.Verbose) Console.WriteLine("      f.DeclaringType:\t" + (i.DeclaringType == null ? "<null>" : i.DeclaringType.ToString()));

                        if (t.IsClass & t.Name != "<Module>")
                        {
                            ctx.listClassInterfaceInfo.Add(new NPCClassInterfaceInfo(i.Name, classIndex));
                        }
                    }

                    if (Trace.Verbose) Console.WriteLine("Type Fields...");
                    foreach (FieldDefinition f in t.Fields)
                    {
                        if (Trace.Verbose) Console.WriteLine("    Field Name:\t" + f.Name);
                        if (Trace.Verbose) Console.WriteLine("      f.Fullname:\t" + f.FullName);
                        //if (Trace.Verbose) Console.WriteLine("      f.Module.FullyQualifiedName:\t" + f.Module.FullyQualifiedName);
                        if (Trace.Verbose) Console.WriteLine("      f.DeclaringType:\t" + f.DeclaringType.ToString());
                        if (Trace.Verbose) Console.WriteLine("      f.FieldType:\t" + f.FieldType.ToString());
                        if (Trace.Verbose) Console.WriteLine("      f.IsPrivate:\t" + f.IsPrivate.ToString());
                        if (Trace.Verbose) Console.WriteLine("      f.IsPublic:\t" + f.IsPublic.ToString());
                        if (Trace.Verbose) Console.WriteLine("      f.HasConstant:\t" + f.HasConstant.ToString());
                        if (f.Constant != null) if (Trace.Verbose) Console.WriteLine("      f.Constant:\t" + f.Constant.ToString());
                        if (Trace.Verbose) Console.WriteLine("      f.HasDefault:\t" + f.HasDefault.ToString());
                        if (f.InitialValue != null)
                        {
                            if (Trace.Verbose) Console.WriteLine("      f.InitialValue:\t" + f.InitialValue.ToString());
                            if (Trace.Verbose) Console.WriteLine("      f.InitialValue.Length:\t" + f.InitialValue.Length.ToString());
                            if (f.InitialValue.Length > 0)
                            {
                                Int32 initialValue = BitConverter.ToInt32(f.InitialValue, 0);
                                if (Trace.Verbose) Console.WriteLine("      f.InitialValue:\t" + initialValue.ToString());
                            }
                        }

                        if (t.IsClass & t.Name != "<Module>")
                        {
                            ctx.listFieldInfo.Add(new NPCFieldInfo(f.Name, f.FieldType.ToString(), classIndex));
                        }
                    }

                    if (Trace.Verbose) Console.WriteLine("Type Methods...");
                    foreach (MethodDefinition m in t.Methods)
                    {
                        if (Trace.Verbose) Console.WriteLine("    Method Name:\t" + m.Name);
                        if (Trace.Verbose) Console.WriteLine("      m.Fullname:\t" + m.FullName);
                        //if (Trace.Verbose) Console.WriteLine("      m.Module.FullyQualifiedName:\t" + m.Module.FullyQualifiedName);
                        if (Trace.Verbose) Console.WriteLine("      m.ReturnType:\t" + m.ReturnType.ToString());
                        if (Trace.Verbose) Helpers.PrintMethods(m);
                        if (Trace.Verbose) Helpers.PrintFields(m);
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
                if (Trace.Error) Console.WriteLine(message);
                success = false;
            }

            if (ctx.listAssemblyInfo.Count < NPCCompilerContext.listDefaultAssemblies.Length)
            {
                string message = "**ERROR** ctx.listAssemblyInfo.Count < NPCCompilerContext.listDefaultAssemblies.Length. Not " +
                                 NPCCompilerContext.listDefaultAssemblies.Length.ToString();
                if (Trace.Error) Console.WriteLine(message);
                success = false;
            }

            if (ctx.listClassInfo.Count < 1)
            {
                string message = "**ERROR** Input assembly file must contain 1 or more class definitions. Not " + ctx.listClassInfo.Count.ToString();
                if (Trace.Error) Console.WriteLine(message);
                success = false;
            }

            List<NPCClassInterfaceInfo> listClassInterfaces;
            for (int classIndex = 0; classIndex < ctx.listClassInfo.Count; classIndex++)
            {
                List<NPCFieldInfo> listClassFields = ctx.listFieldInfo.FindAll(
                   delegate (NPCFieldInfo dfi)
                   {
                       return (dfi.fieldClassIndex == classIndex);
                   });
                if (listClassFields.Count < 1)
                {
                    string message = "**ERROR** Input assembly file must contain 1 or more field definitions. Not " + ctx.listFieldInfo.Count.ToString();
                    if (Trace.Error) Console.WriteLine(message);
                    success = false;
                }

                listClassInterfaces = ctx.listClassInterfaceInfo.FindAll(
                   delegate (NPCClassInterfaceInfo dci)
                   {
                       return (dci.interfaceClassIndex == classIndex);
                   });
                if (listClassInterfaces.Count < 1)
                {
                    string message = "**ERROR** Input assembly file must contain 1 or more interface definitions. Not " + ctx.listClassInterfaceInfo.Count.ToString();
                    if (Trace.Error) Console.WriteLine(message);
                    success = false;
                }

                bool NPCLevel0BasicInterface = false;
                listClassInterfaces = ctx.listClassInterfaceInfo.FindAll(
                   delegate (NPCClassInterfaceInfo dci)
                   {
                       return (dci.interfaceClassIndex == classIndex && dci.interfaceOutputName == NPCLevels.NPCLevel0Basic.ToString());
                   });
                if (listClassInterfaces.Count == 1) NPCLevel0BasicInterface = true;

                bool NPCLevel1ManagedInterface = false;
                listClassInterfaces = ctx.listClassInterfaceInfo.FindAll(
                   delegate (NPCClassInterfaceInfo dci)
                   {
                       return (dci.interfaceClassIndex == classIndex && dci.interfaceOutputName == NPCLevels.NPCLevel1Managed.ToString());
                   });
                if (listClassInterfaces.Count == 1)
                {
                    NPCLevel1ManagedInterface = true;
                    if (!NPCLevel0BasicInterface)
                    {
                        string message = "**ERROR** Class " + ctx.listClassInfo[classIndex].classInputName + " : " + NPCLevels.NPCLevel1Managed.ToString() + " requires interface " + NPCLevels.NPCLevel0Basic.ToString();
                        if (Trace.Error) Console.WriteLine(message);
                        success = false;
                    }
                }

                bool NPCLevel2PersistableInterface = false;
                listClassInterfaces = ctx.listClassInterfaceInfo.FindAll(
                   delegate (NPCClassInterfaceInfo dci)
                   {
                       return (dci.interfaceClassIndex == classIndex && dci.interfaceOutputName == NPCLevels.NPCLevel2Persistable.ToString());
                   });
                if (listClassInterfaces.Count == 1)
                {
                    NPCLevel2PersistableInterface = true;
                    if (!NPCLevel1ManagedInterface)
                    {
                        string message = "**ERROR** Class " + ctx.listClassInfo[classIndex].classInputName + " : " + NPCLevels.NPCLevel2Persistable.ToString() + " requires interface " + NPCLevels.NPCLevel1Managed.ToString();
                        if (Trace.Error) Console.WriteLine(message);
                        success = false;
                    }
                }

                bool NPCLevel3DeletableInterface = false;
                listClassInterfaces = ctx.listClassInterfaceInfo.FindAll(
                   delegate (NPCClassInterfaceInfo dci)
                   {
                       return (dci.interfaceClassIndex == classIndex && dci.interfaceOutputName == NPCLevels.NPCLevel3Deletable.ToString());
                   });
                if (listClassInterfaces.Count == 1)
                {
                    NPCLevel3DeletableInterface = true;
                    if (!NPCLevel2PersistableInterface)
                    {
                        string message = "**ERROR** Class " + ctx.listClassInfo[classIndex].classInputName + " : " + NPCLevels.NPCLevel3Deletable.ToString() + " requires interface " + NPCLevels.NPCLevel2Persistable.ToString();
                        if (Trace.Error) Console.WriteLine(message);
                        success = false;
                    }
                }

                bool NPCLevel4CollectibleInterface = false;
                listClassInterfaces = ctx.listClassInterfaceInfo.FindAll(
                   delegate (NPCClassInterfaceInfo dci)
                   {
                       return (dci.interfaceClassIndex == classIndex && dci.interfaceOutputName == NPCLevels.NPCLevel4Collectible.ToString());
                   });
                if (listClassInterfaces.Count == 1)
                {
                    NPCLevel4CollectibleInterface = true;
                    if (!NPCLevel3DeletableInterface)
                    {
                        string message = "**ERROR** Class " + ctx.listClassInfo[classIndex].classInputName + " : " + NPCLevels.NPCLevel4Collectible.ToString() + " requires interface " + NPCLevels.NPCLevel3Deletable.ToString();
                        if (Trace.Error) Console.WriteLine(message);
                        success = false;
                    }
                }

                bool NPCLevel4CollectibleExtInterface = false;
                listClassInterfaces = ctx.listClassInterfaceInfo.FindAll(
                   delegate (NPCClassInterfaceInfo dci)
                   {
                       return (dci.interfaceClassIndex == classIndex && dci.interfaceOutputName == NPCLevels.NPCLevel4CollectibleExt.ToString());
                   });
                if (listClassInterfaces.Count == 1)
                {
                    NPCLevel4CollectibleExtInterface = true;
                    if (!NPCLevel3DeletableInterface)
                    {
                        string message = "**ERROR** Class " + ctx.listClassInfo[classIndex].classInputName + " : " + NPCLevels.NPCLevel4CollectibleExt.ToString() + " requires interface " + NPCLevels.NPCLevel3Deletable.ToString();
                        if (Trace.Error) Console.WriteLine(message);
                        success = false;
                    }
                }
            }
            return success;
        }

        // Reference: https://stackoverflow.com/questions/6518979/catch-all-error-handling-on-application-level
        // Reference: http://www.csharp-examples.net/catching-unhandled-exceptions/
        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string message = "**ERROR** Compile stopped: " + (e.ExceptionObject as Exception).ToString();
            if (Trace.Error) Console.WriteLine(message);

            if (Trace.Exit) Console.WriteLine("Press enter to exit...");
            if (Trace.Exit) Console.ReadLine();
            Environment.Exit(-1);
        }

        private static bool ValidateTargetProjectEnvironment(NPCCompilerContext ctx)
        {
            bool success = true;
            // Scaffolding: Assume the developer has created an empty NeoContract C# project with the name "NPC" + className + "dApp"
            DirectoryInfo di = new DirectoryInfo(ctx.listModuleInfo[0].moduleTargetFullyQualifiedProjectFolder);
            FileInfo[] files = di.GetFiles("*.dll");
            foreach (FileInfo fi in files)
            {
                if (Trace.Info) Console.WriteLine("**INFO*** ValidateEnvironment:\t" + fi.FullName);
            }

            if (files.Length != 1)
            {
                string message = "**ERROR** Project needs to be a standard NeoContract project with containing 1 DLL in the project folder. Not " + files.Length.ToString();
                if (Trace.Error) Console.WriteLine(message);
                success = false;
            }

            if (files[0].Name != NPCCompilerContext.NeoConvertTaskDllName)
            {
                string message = "**ERROR** Project needs to be a standard NeoContract project with containing " + NPCCompilerContext.NeoConvertTaskDllName + ". Not " + files[0].Name;
                if (Trace.Error) Console.WriteLine(message);
                success = false;
            }

            return success;
        }
    }
}
