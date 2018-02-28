using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace npcc
{
    public class GenCode
    {
        public static bool GenerateCodeLevel0Basic(NPCCompilerContext ctx, int classIndex)
        {
            bool success = true;

            string text1 = "";
            string text2 = "";
            string text3 = "";

            text1 = Helpers.GetTextResource(NPCCompilerContext.NeoEntityModel_csName);
            string targetFileName = ctx.listModuleInfo[0].moduleTargetFullyQualifiedProjectFolder + "\\" +
                                    NPCCompilerContext.NeoEntityModel_csName.Replace("_cs.txt", ".cs");
            File.WriteAllText(targetFileName, text1);

            text1 = Helpers.GetTextResource(NPCCompilerContext.NeoTrace_csName);
            targetFileName = ctx.listModuleInfo[0].moduleTargetFullyQualifiedProjectFolder + "\\" +
                                    NPCCompilerContext.NeoTrace_csName.Replace("_cs.txt", ".cs");
            File.WriteAllText(targetFileName, text1);

            text1 = Helpers.GetTextResource(NPCCompilerContext.NPCLevel0Part1_csName);
            text3 = Helpers.GetTextResource(NPCCompilerContext.NPCLevel0Part2_csName);
            targetFileName = ctx.listModuleInfo[0].moduleTargetFullyQualifiedProjectFolder + "\\" +
                                    ctx.listClassInfo[classIndex].classOutputName + NPCLevelsForFileNames.L0Basic_cs.ToString().Replace("_cs", ".cs");

            text1 = text1.Replace("#NAMESPACE#", ctx.listModuleInfo[0].moduleTargetProjectName);
            text1 = text1.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);

            foreach (NPCFieldInfo f in ctx.listFieldInfo)
            {
                if (f.fieldClassIndex == classIndex) // TODO Performance
                {
                    string fieldName = f.fieldPrivateFieldName;
                    string fieldType = f.fieldOutputType;
                    text2 += "\t\t" + "private" + "\t" + fieldType + "\t" + fieldName + ";\r\n";
                }
            }

            File.WriteAllText(targetFileName, text1 + text2 + text3);

            return success;
        }

        public static bool GenerateCodeLevel1Managed(NPCCompilerContext ctx, int classIndex)
        {
            bool success = true;

            string textSetTemplate = "";
            string textSetXGetXTemplate = "";
            string targetFileName = "";

            string text1 = Helpers.GetTextResource(NPCCompilerContext.NPCLevel1Part1_csName);
            targetFileName = ctx.listModuleInfo[0].moduleTargetFullyQualifiedProjectFolder + "\\" +
                                    ctx.listClassInfo[classIndex].classOutputName + NPCLevelsForFileNames.L1Managed_cs.ToString().Replace("_cs", ".cs");
            text1 = text1.Replace("#NAMESPACE#", ctx.listModuleInfo[0].moduleTargetProjectName);
            text1 = text1.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            File.WriteAllText(targetFileName, text1);

            // #CLASSNAME#, #FIELDTYPE#, #PUBLICFIELDNAME#,  #PRIVATEFIELDNAME# 
            textSetXGetXTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel1SetXGetX_csName);

            // #CLASSNAME#, #ALLFIELDPARAMETERS#, #ALLFIELDASSIGNMENTS#
            textSetTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel1Set_csName);

            string textSetXGetX = "";
            string allFieldParameters = "";
            string allFieldAssignments = "";
            string allFieldsAssignedZero = "";
            string allFieldArgs = "";
            foreach (NPCFieldInfo f in ctx.listFieldInfo)
            {
                if (f.fieldClassIndex == classIndex) // TODO Performance
                {
                    string fieldPrivateName = f.fieldPrivateFieldName;
                    string fieldPublicName = f.fieldPublicFieldName;
                    string fieldType = f.fieldOutputType;

                    string fieldParameter = fieldType + " " + fieldPublicName;
                    allFieldParameters += fieldParameter + ", ";
                    string fieldAssignment = "e." + fieldPrivateName + " = " + fieldPublicName + ";";
                    allFieldAssignments += " " + fieldAssignment;

                    fieldAssignment = "e." + fieldPrivateName + " = " + Helpers.ZeroByType(f.fieldInputType) + ";";
                    allFieldsAssignedZero += fieldAssignment + " ";

                    allFieldArgs += "e." + fieldPrivateName + ", ";

                    textSetXGetX = textSetXGetXTemplate.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
                    textSetXGetX = textSetXGetX.Replace("#FIELDTYPE#", fieldType);
                    textSetXGetX = textSetXGetX.Replace("#PUBLICFIELDNAME#", fieldPublicName);
                    textSetXGetX = textSetXGetX.Replace("#PRIVATEFIELDNAME#", fieldPrivateName);
                    File.AppendAllText(targetFileName, textSetXGetX);
                }
            }
            string textSet = textSetTemplate.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            textSet = textSet.Replace("#ALLFIELDPARAMETERS#", allFieldParameters.Substring(0,allFieldParameters.Length-2)); // drop last ", "
            textSet = textSet.Replace("#ALLFIELDASSIGNMENTS#", allFieldAssignments);
            File.AppendAllText(targetFileName, textSet);

            // # CLASSNAME#, #ALLFIELDSASSIGNEDZERO#, #ALLFIELDASSIGNMENTS#, #ALLFIELDARGS#
            string text2Template = Helpers.GetTextResource(NPCCompilerContext.NPCLevel1Part2_csName);
            string text2 = text2Template.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            text2 = text2.Replace("#ALLFIELDPARAMETERS#", allFieldParameters.Substring(0, allFieldParameters.Length - 2)); // drop last ", "
            text2 = text2.Replace("#ALLFIELDSASSIGNEDZERO#", allFieldsAssignedZero);
            text2 = text2.Replace("#ALLFIELDASSIGNMENTS#", allFieldAssignments);
            text2 = text2.Replace("#ALLFIELDARGS#", allFieldArgs.Substring(0, allFieldArgs.Length - 2)); // drop last ", "
            File.AppendAllText(targetFileName, text2);

            return success;
        }

        public static bool GenerateCodeLevel2Persistable(NPCCompilerContext ctx, int classIndex)
        {
            bool success = true;

            //throw new NotImplementedException();

            return success;
        }

        public static bool GenerateCodeLevel3Deletable(NPCCompilerContext ctx, int classIndex)
        {
            bool success = true;

            //throw new NotImplementedException();

            return success;
        }

        public static bool GenerateCodeLevel4Collectible(NPCCompilerContext ctx, int classIndex)
        {
            bool success = true;

            //throw new NotImplementedException();

            return success;
        }
    }
}
