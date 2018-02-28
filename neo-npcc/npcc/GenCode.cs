using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

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
            text1 = text1.Replace("#PROGRAMNAME#", Program.ProgramName);
            text1 = text1.Replace("#PROGRAMVERSION#", Assembly.GetEntryAssembly().GetName().Version.ToString());
            text1 = text1.Replace("#NOWDATETIME#", DateTime.Now.ToString());
            text1 = text1.Replace("#NAMESPACE#", "NPC.Runtime");
            text1 = text1.Replace("#CLASSNAME#", "NeoEntityModel");
            File.WriteAllText(targetFileName, text1);

            text1 = Helpers.GetTextResource(NPCCompilerContext.NeoTrace_csName);
            targetFileName = ctx.listModuleInfo[0].moduleTargetFullyQualifiedProjectFolder + "\\" +
                                    NPCCompilerContext.NeoTrace_csName.Replace("_cs.txt", ".cs");
            text1 = text1.Replace("#PROGRAMNAME#", Program.ProgramName);
            text1 = text1.Replace("#PROGRAMVERSION#", Assembly.GetEntryAssembly().GetName().Version.ToString());
            text1 = text1.Replace("#NOWDATETIME#", DateTime.Now.ToString());
            text1 = text1.Replace("#NAMESPACE#", "NPC.Runtime");
            text1 = text1.Replace("#CLASSNAME#", "NeoTrace");
            File.WriteAllText(targetFileName, text1);

            text1 = Helpers.GetTextResource(NPCCompilerContext.NPCLevel0Part1_csName);
            text3 = Helpers.GetTextResource(NPCCompilerContext.NPCLevel0Part2_csName);
            targetFileName = ctx.listModuleInfo[0].moduleTargetFullyQualifiedProjectFolder + "\\" +
                                    ctx.listClassInfo[classIndex].classOutputName + NPCLevelsForFileNames.L0Basic_cs.ToString().Replace("_cs", ".cs");
            text1 = text1.Replace("#PROGRAMNAME#", Program.ProgramName);
            text1 = text1.Replace("#PROGRAMVERSION#", Assembly.GetEntryAssembly().GetName().Version.ToString());
            text1 = text1.Replace("#NOWDATETIME#", DateTime.Now.ToString());
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

            string targetFileName = "";
            string text1 = Helpers.GetTextResource(NPCCompilerContext.NPCLevel1Part1_csName);
            targetFileName = ctx.listModuleInfo[0].moduleTargetFullyQualifiedProjectFolder + "\\" +
                                    ctx.listClassInfo[classIndex].classOutputName + NPCLevelsForFileNames.L1Managed_cs.ToString().Replace("_cs", ".cs");
            text1 = text1.Replace("#PROGRAMNAME#", Program.ProgramName);
            text1 = text1.Replace("#PROGRAMVERSION#", Assembly.GetEntryAssembly().GetName().Version.ToString());
            text1 = text1.Replace("#NOWDATETIME#", DateTime.Now.ToString());
            text1 = text1.Replace("#NAMESPACE#", ctx.listModuleInfo[0].moduleTargetProjectName);
            text1 = text1.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            File.WriteAllText(targetFileName, text1);

            // #CLASSNAME#, #FIELDTYPE#, #PUBLICFIELDNAME#,  #PRIVATEFIELDNAME# 
            string textSetXGetXTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel1SetXGetX_csName);
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
                    string fieldAssignment = "e." + fieldPrivateName + " = " + fieldPublicName + "; ";
                    allFieldAssignments += fieldAssignment;

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

            // #CLASSNAME#, #ALLFIELDPARAMETERS#, #ALLFIELDASSIGNMENTS#
            string textSetTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel1Set_csName);
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

            string text;
            string targetFileName = "";

            string part1Template = Helpers.GetTextResource(NPCCompilerContext.NPCLevel2Part1_csName);
            targetFileName = ctx.listModuleInfo[0].moduleTargetFullyQualifiedProjectFolder + "\\" +
                                    ctx.listClassInfo[classIndex].classOutputName + NPCLevelsForFileNames.L2Persistable_cs.ToString().Replace("_cs", ".cs");
            string part1 = part1Template.Replace("#PROGRAMNAME#", Program.ProgramName);
            part1 = part1.Replace("#PROGRAMVERSION#", Assembly.GetEntryAssembly().GetName().Version.ToString());
            part1 = part1.Replace("#NOWDATETIME#", DateTime.Now.ToString());
            part1 = part1.Replace("#NAMESPACE#", ctx.listModuleInfo[0].moduleTargetProjectName);
            part1 = part1.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            File.WriteAllText(targetFileName, part1);

            // #CLASSNAME#, #FIELDTYPE#, #PUBLICFIELDNAME#,  #PRIVATEFIELDNAME# 
            string textFieldConstsTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel2AFieldConsts_csName);
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
                    string fieldAssignment = "e." + fieldPrivateName + " = " + fieldPublicName + "; ";
                    allFieldAssignments += fieldAssignment;

                    fieldAssignment = "e." + fieldPrivateName + " = " + Helpers.ZeroByType(f.fieldInputType) + ";";
                    allFieldsAssignedZero += fieldAssignment + " ";

                    allFieldArgs += "e." + fieldPrivateName + ", ";

                    text = textFieldConstsTemplate.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
                    text = text.Replace("#FIELDTYPE#", fieldType);
                    text = text.Replace("#PUBLICFIELDNAME#", fieldPublicName);
                    text = text.Replace("#PRIVATEFIELDNAME#", fieldPrivateName);
                    File.AppendAllText(targetFileName, text);
                }
            }

            string text2bMissingTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel2BMissing_csName);
            text = text2bMissingTemplate.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            text = text.Replace("#ALLFIELDPARAMETERS#", allFieldParameters.Substring(0, allFieldParameters.Length - 2)); // drop last ", "
            text = text.Replace("#ALLFIELDSASSIGNEDZERO#", allFieldsAssignedZero);
            text = text.Replace("#ALLFIELDASSIGNMENTS#", allFieldAssignments);
            text = text.Replace("#ALLFIELDARGS#", allFieldArgs.Substring(0, allFieldArgs.Length - 2)); // drop last ", "
            File.AppendAllText(targetFileName, text);
            
            string text2CPutTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel2CPut_csName);
            foreach (NPCFieldInfo f in ctx.listFieldInfo)
            {
                if (f.fieldClassIndex == classIndex) // TODO Performance
                {
                    string fieldPrivateName = f.fieldPrivateFieldName;
                    string fieldPublicName = f.fieldPublicFieldName;
                    text = text2CPutTemplate.Replace("#PUBLICFIELDNAME#", fieldPublicName);
                    text = text.Replace("#PRIVATEFIELDNAME#", fieldPrivateName);
                    File.AppendAllText(targetFileName, text);
                }
            }

            string text2DPutTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel2DPut_csName);
            text = text2DPutTemplate.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            text = text.Replace("#ALLFIELDPARAMETERS#", allFieldParameters.Substring(0, allFieldParameters.Length - 2)); // drop last ", "
            text = text.Replace("#ALLFIELDSASSIGNEDZERO#", allFieldsAssignedZero);
            text = text.Replace("#ALLFIELDASSIGNMENTS#", allFieldAssignments);
            text = text.Replace("#ALLFIELDARGS#", allFieldArgs.Substring(0, allFieldArgs.Length - 2)); // drop last ", "
            File.AppendAllText(targetFileName, text);

            string text2EPutTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel2EPut_csName);
            foreach (NPCFieldInfo f in ctx.listFieldInfo)
            {
                if (f.fieldClassIndex == classIndex) // TODO Performance
                {
                    string fieldPrivateName = f.fieldPrivateFieldName;
                    string fieldPublicName = f.fieldPublicFieldName;
                    text = text2EPutTemplate.Replace("#PUBLICFIELDNAME#", fieldPublicName);
                    text = text.Replace("#PRIVATEFIELDNAME#", fieldPrivateName);
                    File.AppendAllText(targetFileName, text);
                }
            }

            string text2FGetTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel2FGet_csName);
            text = text2FGetTemplate.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            text = text.Replace("#ALLFIELDPARAMETERS#", allFieldParameters.Substring(0, allFieldParameters.Length - 2)); // drop last ", "
            text = text.Replace("#ALLFIELDSASSIGNEDZERO#", allFieldsAssignedZero);
            text = text.Replace("#ALLFIELDASSIGNMENTS#", allFieldAssignments);
            text = text.Replace("#ALLFIELDARGS#", allFieldArgs.Substring(0, allFieldArgs.Length - 2)); // drop last ", "
            File.AppendAllText(targetFileName, text);

            string text2GGetTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel2GGet_csName);
            foreach (NPCFieldInfo f in ctx.listFieldInfo)
            {
                if (f.fieldClassIndex == classIndex) // TODO Performance
                {
                    string fieldPrivateName = f.fieldPrivateFieldName;
                    string fieldPublicName = f.fieldPublicFieldName;
                    string fieldType = f.fieldOutputType;
                    text = text2GGetTemplate.Replace("#PUBLICFIELDNAME#", fieldPublicName);
                    text = text.Replace("#PRIVATEFIELDNAME#", fieldPrivateName);
                    text = text.Replace("#FIELDTYPE#", fieldType);
                    if (fieldType == "BigInteger")
                    {
                        text = text.Replace("#ASBIGINTEGER#", ".AsBigInteger()");
                    }
                    else
                    {
                        text = text.Replace("#ASBIGINTEGER#", "");
                    }
                    File.AppendAllText(targetFileName, text);
                }
            }

            string text2HGetTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel2HGet_csName);
            text = text2HGetTemplate.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            text = text.Replace("#ALLFIELDPARAMETERS#", allFieldParameters.Substring(0, allFieldParameters.Length - 2)); // drop last ", "
            text = text.Replace("#ALLFIELDSASSIGNEDZERO#", allFieldsAssignedZero);
            text = text.Replace("#ALLFIELDASSIGNMENTS#", allFieldAssignments);
            text = text.Replace("#ALLFIELDARGS#", allFieldArgs.Substring(0, allFieldArgs.Length - 2)); // drop last ", "
            File.AppendAllText(targetFileName, text);

            string text2IGetTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel2IGet_csName);
            foreach (NPCFieldInfo f in ctx.listFieldInfo)
            {
                if (f.fieldClassIndex == classIndex) // TODO Performance
                {
                    string fieldPrivateName = f.fieldPrivateFieldName;
                    string fieldPublicName = f.fieldPublicFieldName;
                    string fieldType = f.fieldOutputType;
                    text = text2IGetTemplate.Replace("#PUBLICFIELDNAME#", fieldPublicName);
                    text = text.Replace("#PRIVATEFIELDNAME#", fieldPrivateName);
                    text = text.Replace("#FIELDTYPE#", fieldType);
                    if (fieldType == "BigInteger")
                    {
                        text = text.Replace("#ASBIGINTEGER#", ".AsBigInteger()");
                    }
                    else
                    {
                        text = text.Replace("#ASBIGINTEGER#", "");
                    }
                    File.AppendAllText(targetFileName, text);
                }
            }

            // # CLASSNAME#, #ALLFIELDSASSIGNEDZERO#, #ALLFIELDASSIGNMENTS#, #ALLFIELDARGS#
            string part2Template = Helpers.GetTextResource(NPCCompilerContext.NPCLevel2Part2_csName);
            string part2 = part2Template.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            part2 = part2.Replace("#ALLFIELDPARAMETERS#", allFieldParameters.Substring(0, allFieldParameters.Length - 2)); // drop last ", "
            part2 = part2.Replace("#ALLFIELDSASSIGNEDZERO#", allFieldsAssignedZero);
            part2 = part2.Replace("#ALLFIELDASSIGNMENTS#", allFieldAssignments);
            part2 = part2.Replace("#ALLFIELDARGS#", allFieldArgs.Substring(0, allFieldArgs.Length - 2)); // drop last ", "
            File.AppendAllText(targetFileName, part2);

            return success;
        }

        public static bool GenerateCodeLevel3Deletable(NPCCompilerContext ctx, int classIndex)
        {
            bool success = true;

            string text;

            string targetFileName = "";
            string part1Template = Helpers.GetTextResource(NPCCompilerContext.NPCLevel3Part1_csName);
            targetFileName = ctx.listModuleInfo[0].moduleTargetFullyQualifiedProjectFolder + "\\" +
                                    ctx.listClassInfo[classIndex].classOutputName + NPCLevelsForFileNames.L3Deletable_cs.ToString().Replace("_cs", ".cs");
            string part1 = part1Template.Replace("#PROGRAMNAME#", Program.ProgramName);
            part1 = part1.Replace("#PROGRAMVERSION#", Assembly.GetEntryAssembly().GetName().Version.ToString());
            part1 = part1.Replace("#NOWDATETIME#", DateTime.Now.ToString());
            part1 = part1.Replace("#NAMESPACE#", ctx.listModuleInfo[0].moduleTargetProjectName);
            part1 = part1.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            string allFieldsAssignedZero = "";
            foreach (NPCFieldInfo f in ctx.listFieldInfo)
            {
                if (f.fieldClassIndex == classIndex) // TODO Performance
                {
                    string fieldPrivateName = f.fieldPrivateFieldName;
                    string fieldPublicName = f.fieldPublicFieldName;

                    string fieldAssignment = "e." + fieldPrivateName + " = " + Helpers.ZeroByType(f.fieldInputType) + ";";
                    allFieldsAssignedZero += fieldAssignment + " ";
                }
            }
            part1 = part1.Replace("#ALLFIELDSASSIGNEDZERO#", allFieldsAssignedZero);
            File.WriteAllText(targetFileName, part1);

            string text3ABuryTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel3ABury_csName);
            foreach (NPCFieldInfo f in ctx.listFieldInfo)
            {
                if (f.fieldClassIndex == classIndex) // TODO Performance
                {
                    string fieldPrivateName = f.fieldPrivateFieldName;
                    string fieldPublicName = f.fieldPublicFieldName;
                    text = text3ABuryTemplate.Replace("#PUBLICFIELDNAME#", fieldPublicName);
                    text = text.Replace("#PRIVATEFIELDNAME#", fieldPrivateName);
                    File.AppendAllText(targetFileName, text);
                }
            }

            string text3BBuryTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel3BBury_csName);
            text = text3BBuryTemplate.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            File.AppendAllText(targetFileName, text);

            string text3CBuryTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel3CBury_csName);
            foreach (NPCFieldInfo f in ctx.listFieldInfo)
            {
                if (f.fieldClassIndex == classIndex) // TODO Performance
                {
                    string fieldPrivateName = f.fieldPrivateFieldName;
                    string fieldPublicName = f.fieldPublicFieldName;
                    text = text3CBuryTemplate.Replace("#PUBLICFIELDNAME#", fieldPublicName);
                    text = text.Replace("#PRIVATEFIELDNAME#", fieldPrivateName);
                    File.AppendAllText(targetFileName, text);
                }
            }

            string part2Template = Helpers.GetTextResource(NPCCompilerContext.NPCLevel3Part2_csName);
            string part2 = part2Template.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            File.AppendAllText(targetFileName, part2);

            return success;
        }

        public static bool GenerateCodeLevel4Collectible(NPCCompilerContext ctx, int classIndex)
        {
            bool success = true;

            string text1 = "";
            string targetFileName = "";

            text1 = Helpers.GetTextResource(NPCCompilerContext.NeoVersionedAppUser_csName);
            targetFileName = ctx.listModuleInfo[0].moduleTargetFullyQualifiedProjectFolder + "\\" +
                                    NPCCompilerContext.NeoVersionedAppUser_csName.Replace("_cs.txt", ".cs");
            text1 = text1.Replace("#PROGRAMNAME#", Program.ProgramName);
            text1 = text1.Replace("#PROGRAMVERSION#", Assembly.GetEntryAssembly().GetName().Version.ToString());
            text1 = text1.Replace("#NOWDATETIME#", DateTime.Now.ToString());
            text1 = text1.Replace("#NAMESPACE#", "NPC.Runtime");
            text1 = text1.Replace("#CLASSNAME#", "NeoVersionedAppUser");
            File.WriteAllText(targetFileName, text1);

            text1 = Helpers.GetTextResource(NPCCompilerContext.NeoStorageKey_csName);
            targetFileName = ctx.listModuleInfo[0].moduleTargetFullyQualifiedProjectFolder + "\\" +
                                    NPCCompilerContext.NeoStorageKey_csName.Replace("_cs.txt", ".cs");
            text1 = text1.Replace("#PROGRAMNAME#", Program.ProgramName);
            text1 = text1.Replace("#PROGRAMVERSION#", Assembly.GetEntryAssembly().GetName().Version.ToString());
            text1 = text1.Replace("#NOWDATETIME#", DateTime.Now.ToString());
            text1 = text1.Replace("#NAMESPACE#", "NPC.Runtime");
            text1 = text1.Replace("#CLASSNAME#", "NeoStorageKey");
            File.WriteAllText(targetFileName, text1);

            string part1Template = Helpers.GetTextResource(NPCCompilerContext.NPCLevel4Part1_csName);
            targetFileName = ctx.listModuleInfo[0].moduleTargetFullyQualifiedProjectFolder + "\\" +
                                    ctx.listClassInfo[classIndex].classOutputName + NPCLevelsForFileNames.L4Collectible_cs.ToString().Replace("_cs", ".cs");
            string part1 = part1Template.Replace("#PROGRAMNAME#", Program.ProgramName);
            part1 = part1.Replace("#PROGRAMVERSION#", Assembly.GetEntryAssembly().GetName().Version.ToString());
            part1 = part1.Replace("#NOWDATETIME#", DateTime.Now.ToString());
            part1 = part1.Replace("#NAMESPACE#", ctx.listModuleInfo[0].moduleTargetProjectName);
            part1 = part1.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            File.WriteAllText(targetFileName, part1);

            string part2Template = Helpers.GetTextResource(NPCCompilerContext.NPCLevel4Part2_csName);
            string part2 = part2Template.Replace("#PROGRAMNAME#", Program.ProgramName);
            part2 = part2.Replace("#PROGRAMVERSION#", Assembly.GetEntryAssembly().GetName().Version.ToString());
            part2 = part2.Replace("#NOWDATETIME#", DateTime.Now.ToString());
            part2 = part2.Replace("#NAMESPACE#", ctx.listModuleInfo[0].moduleTargetProjectName);
            part2 = part2.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            File.AppendAllText(targetFileName, part2);

            return success;
        }
    }
}
