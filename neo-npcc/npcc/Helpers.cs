using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace npcc
{
    public class Helpers
    {
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
    }
}
