using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace npcc
{
    public class Helpers
    {
        // Reference: https://support.microsoft.com/en-ca/help/319292/how-to-embed-and-access-resources-by-using-visual-c
        public static string GetTextResource(string resName)
        {
            string text;

            Assembly _assembly;
            StreamReader _textStreamReader;

            _assembly = Assembly.GetExecutingAssembly();
            _textStreamReader = new StreamReader(_assembly.GetManifestResourceStream("npcc.Resources." + resName));

            text = _textStreamReader.ReadToEnd();

            return text;
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
            if (method.Body != null && method.Body.Instructions != null) foreach (var instruction in method.Body.Instructions)
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

        public static string ZeroByType(string fieldInputType)
        {
            string zeroString = "";

            switch (fieldInputType)
            {
                case "System.Numerics.BigInteger":
                case "System.Int32":
                    {
                        zeroString = "0";
                        break;
                    }
                case "System.String":
                    {
                        zeroString = "\"\"";
                        break;
                    }
                case "System.Byte[]":
                    {
                        zeroString = "NeoEntityModel.NullByteArray";
                        break;
                    }
                default:
                    {
                        string message = "**ERROR** Field type '" + fieldInputType + "' is not supported in C#.NPC. Use BigInteger, byte[], or string.";
                        Console.WriteLine(message);
                        throw new ArgumentOutOfRangeException(fieldInputType, message);
                        //break;
                    }
            }

            return zeroString;
        }
    }
}
