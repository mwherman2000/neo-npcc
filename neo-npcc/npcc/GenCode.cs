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
            string targetFullyQualifiedFileName = ctx.listModuleInfo[0].moduleTargetFullyQualifiedProjectFolder + "\\" +
                                    NPCCompilerContext.NeoEntityModel_csName.Replace("_cs.txt", ".cs");
            text1 = text1.Replace("#PROGRAMNAME#", Program.ProgramName);
            text1 = text1.Replace("#PROGRAMVERSION#", Assembly.GetEntryAssembly().GetName().Version.ToString());
            text1 = text1.Replace("#NOWDATETIME#", DateTime.Now.ToString());
            text1 = text1.Replace("#NAMESPACE#", "NPC.Runtime");
            text1 = text1.Replace("#CLASSNAME#", "NeoEntityModel");
            File.WriteAllText(targetFullyQualifiedFileName, text1);

            text1 = Helpers.GetTextResource(NPCCompilerContext.NeoTrace_csName);
            targetFullyQualifiedFileName = ctx.listModuleInfo[0].moduleTargetFullyQualifiedProjectFolder + "\\" +
                                    NPCCompilerContext.NeoTrace_csName.Replace("_cs.txt", ".cs");
            text1 = text1.Replace("#PROGRAMNAME#", Program.ProgramName);
            text1 = text1.Replace("#PROGRAMVERSION#", Assembly.GetEntryAssembly().GetName().Version.ToString());
            text1 = text1.Replace("#NOWDATETIME#", DateTime.Now.ToString());
            text1 = text1.Replace("#NAMESPACE#", "NPC.Runtime");
            text1 = text1.Replace("#CLASSNAME#", "NeoTrace");
            File.WriteAllText(targetFullyQualifiedFileName, text1);

            text1 = Helpers.GetTextResource(NPCCompilerContext.NPCLevel0Part1_csName);
            text3 = Helpers.GetTextResource(NPCCompilerContext.NPCLevel0Part2_csName);
            targetFullyQualifiedFileName = ctx.listModuleInfo[0].moduleTargetFullyQualifiedProjectFolder + "\\" +
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
                    text2 += "        " + "private" + "\t" + fieldType + "\t" + fieldName + ";\r\n";
                }
            }

            File.WriteAllText(targetFullyQualifiedFileName, text1 + text2 + text3);

            return success;
        }

        public static bool GenerateCodeLevel1Managed(NPCCompilerContext ctx, int classIndex)
        {
            bool success = true;

            string targetFullyQualifiedFileName = "";
            string text1 = Helpers.GetTextResource(NPCCompilerContext.NPCLevel1Part1_csName);
            targetFullyQualifiedFileName = ctx.listModuleInfo[0].moduleTargetFullyQualifiedProjectFolder + "\\" +
                                    ctx.listClassInfo[classIndex].classOutputName + NPCLevelsForFileNames.L1Managed_cs.ToString().Replace("_cs", ".cs");
            text1 = text1.Replace("#PROGRAMNAME#", Program.ProgramName);
            text1 = text1.Replace("#PROGRAMVERSION#", Assembly.GetEntryAssembly().GetName().Version.ToString());
            text1 = text1.Replace("#NOWDATETIME#", DateTime.Now.ToString());
            text1 = text1.Replace("#NAMESPACE#", ctx.listModuleInfo[0].moduleTargetProjectName);
            text1 = text1.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            File.WriteAllText(targetFullyQualifiedFileName, text1);

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
                    File.AppendAllText(targetFullyQualifiedFileName, textSetXGetX);
                }
            }

            // #CLASSNAME#, #ALLFIELDPARAMETERS#, #ALLFIELDASSIGNMENTS#
            string textSetTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel1Set_csName);
            string textSet = textSetTemplate.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            textSet = textSet.Replace("#ALLFIELDPARAMETERS#", allFieldParameters.Substring(0,allFieldParameters.Length-2)); // drop last ", "
            textSet = textSet.Replace("#ALLFIELDASSIGNMENTS#", allFieldAssignments);
            File.AppendAllText(targetFullyQualifiedFileName, textSet);

            // # CLASSNAME#, #ALLFIELDSASSIGNEDZERO#, #ALLFIELDASSIGNMENTS#, #ALLFIELDARGS#
            string text2Template = Helpers.GetTextResource(NPCCompilerContext.NPCLevel1Part2_csName);
            string text2 = text2Template.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            text2 = text2.Replace("#ALLFIELDPARAMETERS#", allFieldParameters.Substring(0, allFieldParameters.Length - 2)); // drop last ", "
            text2 = text2.Replace("#ALLFIELDSASSIGNEDZERO#", allFieldsAssignedZero);
            text2 = text2.Replace("#ALLFIELDASSIGNMENTS#", allFieldAssignments);
            text2 = text2.Replace("#ALLFIELDARGS#", allFieldArgs.Substring(0, allFieldArgs.Length - 2)); // drop last ", "
            File.AppendAllText(targetFullyQualifiedFileName, text2);

            return success;
        }

        public static bool GenerateCodeLevel2Persistable(NPCCompilerContext ctx, int classIndex)
        {
            bool success = true;

            string text;
            string targetFullyQualifiedFileName = "";

            string part1Template = Helpers.GetTextResource(NPCCompilerContext.NPCLevel2Part1_csName);
            targetFullyQualifiedFileName = ctx.listModuleInfo[0].moduleTargetFullyQualifiedProjectFolder + "\\" +
                                    ctx.listClassInfo[classIndex].classOutputName + NPCLevelsForFileNames.L2Persistable_cs.ToString().Replace("_cs", ".cs");
            string part1 = part1Template.Replace("#PROGRAMNAME#", Program.ProgramName);
            part1 = part1.Replace("#PROGRAMVERSION#", Assembly.GetEntryAssembly().GetName().Version.ToString());
            part1 = part1.Replace("#NOWDATETIME#", DateTime.Now.ToString());
            part1 = part1.Replace("#NAMESPACE#", ctx.listModuleInfo[0].moduleTargetProjectName);
            part1 = part1.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            File.WriteAllText(targetFullyQualifiedFileName, part1);

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
                    File.AppendAllText(targetFullyQualifiedFileName, text);
                }
            }

            string text2bMissingTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel2BMissing_csName);
            text = text2bMissingTemplate.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            text = text.Replace("#ALLFIELDPARAMETERS#", allFieldParameters.Substring(0, allFieldParameters.Length - 2)); // drop last ", "
            text = text.Replace("#ALLFIELDSASSIGNEDZERO#", allFieldsAssignedZero);
            text = text.Replace("#ALLFIELDASSIGNMENTS#", allFieldAssignments);
            text = text.Replace("#ALLFIELDARGS#", allFieldArgs.Substring(0, allFieldArgs.Length - 2)); // drop last ", "
            File.AppendAllText(targetFullyQualifiedFileName, text);
            
            string text2CPutTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel2CPut_csName);
            foreach (NPCFieldInfo f in ctx.listFieldInfo)
            {
                if (f.fieldClassIndex == classIndex) // TODO Performance
                {
                    string fieldPrivateName = f.fieldPrivateFieldName;
                    string fieldPublicName = f.fieldPublicFieldName;
                    text = text2CPutTemplate.Replace("#PUBLICFIELDNAME#", fieldPublicName);
                    text = text.Replace("#PRIVATEFIELDNAME#", fieldPrivateName);
                    File.AppendAllText(targetFullyQualifiedFileName, text);
                }
            }

            string text2DPutTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel2DPut_csName);
            text = text2DPutTemplate.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            text = text.Replace("#ALLFIELDPARAMETERS#", allFieldParameters.Substring(0, allFieldParameters.Length - 2)); // drop last ", "
            text = text.Replace("#ALLFIELDSASSIGNEDZERO#", allFieldsAssignedZero);
            text = text.Replace("#ALLFIELDASSIGNMENTS#", allFieldAssignments);
            text = text.Replace("#ALLFIELDARGS#", allFieldArgs.Substring(0, allFieldArgs.Length - 2)); // drop last ", "
            File.AppendAllText(targetFullyQualifiedFileName, text);

            string text2EPutTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel2EPut_csName);
            foreach (NPCFieldInfo f in ctx.listFieldInfo)
            {
                if (f.fieldClassIndex == classIndex) // TODO Performance
                {
                    string fieldPrivateName = f.fieldPrivateFieldName;
                    string fieldPublicName = f.fieldPublicFieldName;
                    text = text2EPutTemplate.Replace("#PUBLICFIELDNAME#", fieldPublicName);
                    text = text.Replace("#PRIVATEFIELDNAME#", fieldPrivateName);
                    File.AppendAllText(targetFullyQualifiedFileName, text);
                }
            }

            string text2FGetTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel2FGet_csName);
            text = text2FGetTemplate.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            text = text.Replace("#ALLFIELDPARAMETERS#", allFieldParameters.Substring(0, allFieldParameters.Length - 2)); // drop last ", "
            text = text.Replace("#ALLFIELDSASSIGNEDZERO#", allFieldsAssignedZero);
            text = text.Replace("#ALLFIELDASSIGNMENTS#", allFieldAssignments);
            text = text.Replace("#ALLFIELDARGS#", allFieldArgs.Substring(0, allFieldArgs.Length - 2)); // drop last ", "
            File.AppendAllText(targetFullyQualifiedFileName, text);

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
                    File.AppendAllText(targetFullyQualifiedFileName, text);
                }
            }

            string text2HGetTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel2HGet_csName);
            text = text2HGetTemplate.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            text = text.Replace("#ALLFIELDPARAMETERS#", allFieldParameters.Substring(0, allFieldParameters.Length - 2)); // drop last ", "
            text = text.Replace("#ALLFIELDSASSIGNEDZERO#", allFieldsAssignedZero);
            text = text.Replace("#ALLFIELDASSIGNMENTS#", allFieldAssignments);
            text = text.Replace("#ALLFIELDARGS#", allFieldArgs.Substring(0, allFieldArgs.Length - 2)); // drop last ", "
            File.AppendAllText(targetFullyQualifiedFileName, text);

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
                    File.AppendAllText(targetFullyQualifiedFileName, text);
                }
            }

            string part2Template = Helpers.GetTextResource(NPCCompilerContext.NPCLevel2Part2_csName);
            string part2 = part2Template.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            part2 = part2.Replace("#ALLFIELDPARAMETERS#", allFieldParameters.Substring(0, allFieldParameters.Length - 2)); // drop last ", "
            part2 = part2.Replace("#ALLFIELDSASSIGNEDZERO#", allFieldsAssignedZero);
            part2 = part2.Replace("#ALLFIELDASSIGNMENTS#", allFieldAssignments);
            part2 = part2.Replace("#ALLFIELDARGS#", allFieldArgs.Substring(0, allFieldArgs.Length - 2)); // drop last ", "
            File.AppendAllText(targetFullyQualifiedFileName, part2);

            return success;
        }

        public static bool GenerateCodeLevel3Deletable(NPCCompilerContext ctx, int classIndex)
        {
            bool success = true;

            string text;

            string targetFullyQualifiedFileName = "";
            string part1Template = Helpers.GetTextResource(NPCCompilerContext.NPCLevel3Part1_csName);
            targetFullyQualifiedFileName = ctx.listModuleInfo[0].moduleTargetFullyQualifiedProjectFolder + "\\" +
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
            File.WriteAllText(targetFullyQualifiedFileName, part1);

            string text3ABuryTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel3ABury_csName);
            foreach (NPCFieldInfo f in ctx.listFieldInfo)
            {
                if (f.fieldClassIndex == classIndex) // TODO Performance
                {
                    string fieldPrivateName = f.fieldPrivateFieldName;
                    string fieldPublicName = f.fieldPublicFieldName;
                    text = text3ABuryTemplate.Replace("#PUBLICFIELDNAME#", fieldPublicName);
                    text = text.Replace("#PRIVATEFIELDNAME#", fieldPrivateName);
                    File.AppendAllText(targetFullyQualifiedFileName, text);
                }
            }

            string text3BBuryTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel3BBury_csName);
            text = text3BBuryTemplate.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            File.AppendAllText(targetFullyQualifiedFileName, text);

            string text3CBuryTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel3CBury_csName);
            foreach (NPCFieldInfo f in ctx.listFieldInfo)
            {
                if (f.fieldClassIndex == classIndex) // TODO Performance
                {
                    string fieldPrivateName = f.fieldPrivateFieldName;
                    string fieldPublicName = f.fieldPublicFieldName;
                    text = text3CBuryTemplate.Replace("#PUBLICFIELDNAME#", fieldPublicName);
                    text = text.Replace("#PRIVATEFIELDNAME#", fieldPrivateName);
                    File.AppendAllText(targetFullyQualifiedFileName, text);
                }
            }

            string part2Template = Helpers.GetTextResource(NPCCompilerContext.NPCLevel3Part2_csName);
            string part2 = part2Template.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            File.AppendAllText(targetFullyQualifiedFileName, part2);

            return success;
        }

        public static bool GenerateCodeLevel4Collectible(NPCCompilerContext ctx, int classIndex)
        {
            bool success = true;

            string text = "";
            string targetFullyQualifiedFileName = "";

            text = Helpers.GetTextResource(NPCCompilerContext.NeoVersionedAppUser_csName);
            targetFullyQualifiedFileName = ctx.listModuleInfo[0].moduleTargetFullyQualifiedProjectFolder + "\\" +
                                    NPCCompilerContext.NeoVersionedAppUser_csName.Replace("_cs.txt", ".cs");
            text = text.Replace("#PROGRAMNAME#", Program.ProgramName);
            text = text.Replace("#PROGRAMVERSION#", Assembly.GetEntryAssembly().GetName().Version.ToString());
            text = text.Replace("#NOWDATETIME#", DateTime.Now.ToString());
            text = text.Replace("#NAMESPACE#", "NPC.Runtime");
            text = text.Replace("#CLASSNAME#", "NeoVersionedAppUser");
            File.WriteAllText(targetFullyQualifiedFileName, text);

            text = Helpers.GetTextResource(NPCCompilerContext.NeoStorageKey_csName);
            targetFullyQualifiedFileName = ctx.listModuleInfo[0].moduleTargetFullyQualifiedProjectFolder + "\\" +
                                    NPCCompilerContext.NeoStorageKey_csName.Replace("_cs.txt", ".cs");
            text = text.Replace("#PROGRAMNAME#", Program.ProgramName);
            text = text.Replace("#PROGRAMVERSION#", Assembly.GetEntryAssembly().GetName().Version.ToString());
            text = text.Replace("#NOWDATETIME#", DateTime.Now.ToString());
            text = text.Replace("#NAMESPACE#", "NPC.Runtime");
            text = text.Replace("#CLASSNAME#", "NeoStorageKey");
            File.WriteAllText(targetFullyQualifiedFileName, text);

            string part1Template = Helpers.GetTextResource(NPCCompilerContext.NPCLevel4Part1_csName);
            targetFullyQualifiedFileName = ctx.listModuleInfo[0].moduleTargetFullyQualifiedProjectFolder + "\\" +
                                    ctx.listClassInfo[classIndex].classOutputName + NPCLevelsForFileNames.L4Collectible_cs.ToString().Replace("_cs", ".cs");
            string part1 = part1Template.Replace("#PROGRAMNAME#", Program.ProgramName);
            part1 = part1.Replace("#PROGRAMVERSION#", Assembly.GetEntryAssembly().GetName().Version.ToString());
            part1 = part1.Replace("#NOWDATETIME#", DateTime.Now.ToString());
            part1 = part1.Replace("#NAMESPACE#", ctx.listModuleInfo[0].moduleTargetProjectName);
            part1 = part1.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            File.WriteAllText(targetFullyQualifiedFileName, part1);

            string text4APutElementTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel4APutElement_csName);
            foreach (NPCFieldInfo f in ctx.listFieldInfo)
            {
                if (f.fieldClassIndex == classIndex) // TODO Performance
                {
                    string fieldPrivateName = f.fieldPrivateFieldName;
                    string fieldPublicName = f.fieldPublicFieldName;
                    text = text4APutElementTemplate.Replace("#PUBLICFIELDNAME#", fieldPublicName);
                    text = text.Replace("#PRIVATEFIELDNAME#", fieldPrivateName);
                    File.AppendAllText(targetFullyQualifiedFileName, text);
                }
            }

            string text4BGetElementTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel4BGetElement_csName);
            text = text4BGetElementTemplate.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            File.AppendAllText(targetFullyQualifiedFileName, text);

            string allFieldParameters = "";
            string allFieldAssignments = "";
            string allFieldsAssignedZero = "";
            string allFieldArgs = "";
            string text4CGetElementTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel4CGetElement_csName);
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

                    text = text4CGetElementTemplate.Replace("#PUBLICFIELDNAME#", fieldPublicName);
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
                    File.AppendAllText(targetFullyQualifiedFileName, text);
                }
            }

            string text4DBuryElementTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel4DBuryElement_csName);
            text = text4DBuryElementTemplate.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            text = text.Replace("#ALLFIELDPARAMETERS#", allFieldParameters.Substring(0, allFieldParameters.Length - 2)); // drop last ", "
            text = text.Replace("#ALLFIELDSASSIGNEDZERO#", allFieldsAssignedZero);
            text = text.Replace("#ALLFIELDASSIGNMENTS#", allFieldAssignments);
            text = text.Replace("#ALLFIELDARGS#", allFieldArgs.Substring(0, allFieldArgs.Length - 2)); // drop last ", "
            File.AppendAllText(targetFullyQualifiedFileName, text);

            string text4EBuryElementTemplate = Helpers.GetTextResource(NPCCompilerContext.NPCLevel4EBuryElement_csName);
            foreach (NPCFieldInfo f in ctx.listFieldInfo)
            {
                if (f.fieldClassIndex == classIndex) // TODO Performance
                {
                    string fieldPrivateName = f.fieldPrivateFieldName;
                    string fieldPublicName = f.fieldPublicFieldName;
                    text = text4EBuryElementTemplate.Replace("#PUBLICFIELDNAME#", fieldPublicName);
                    text = text.Replace("#PRIVATEFIELDNAME#", fieldPrivateName);
                    File.AppendAllText(targetFullyQualifiedFileName, text);
                }
            }

            string part2Template = Helpers.GetTextResource(NPCCompilerContext.NPCLevel4Part2_csName);
            string part2 = part2Template.Replace("#PROGRAMNAME#", Program.ProgramName);
            part2 = part2.Replace("#PROGRAMVERSION#", Assembly.GetEntryAssembly().GetName().Version.ToString());
            part2 = part2.Replace("#NOWDATETIME#", DateTime.Now.ToString());
            part2 = part2.Replace("#NAMESPACE#", ctx.listModuleInfo[0].moduleTargetProjectName);
            part2 = part2.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
            File.AppendAllText(targetFullyQualifiedFileName, part2);

            return success;
        }


        /// <summary>
        /// Generates the code for custom methods.
        /// 
        /// This is the trickiest of the GenerateCode* methods because there is more than one scenario to deal with:
        /// 1. If #CLASSNAME#L4CustomMethods.cs already exist in the Target Project, return and don't do anything
        /// 2. If #CLASSNAME#L4CustomMethods.cs doesn't exist in the Target Project but does exist in the Model Class project,
        ///    copy it to the Target Project
        /// 3. Else use the NPCLevel4CustomMethods_cs.txt template to create a new #CLASSNAME#L4CustomMethods.cs file in the 
        ///    Target Project
        /// </summary>
        /// <param name="ctx">The CTX.</param>
        /// <param name="classIndex">Index of the class.</param>
        /// <param name="listClassInterfaces">The list of *CustomMethods class interfaces.</param>
        /// <returns></returns>
        public static bool GenerateCodeCustomMethods(NPCCompilerContext ctx, int classIndex, List<NPCClassInterfaceInfo> listClassInterfaces)
        {
            bool success = true;

            string text = "";
            string textTemplate = "";
            string classModelFullyQualifiedFileName = "";
            string targetFullyQualifiedFileName = "";

            foreach (NPCClassInterfaceInfo cii in listClassInterfaces)
            {
                if (cii.interfaceClassIndex != classIndex) break;

                string targetFileName = ctx.listClassInfo[classIndex].classOutputName + cii.interfaceOutputName.Replace("NPCLevel", "L") + ".cs";

                classModelFullyQualifiedFileName = ctx.listModuleInfo[0].moduleModelClassFullyQualifiedProjectFolder + "\\" + targetFileName;
                targetFullyQualifiedFileName = ctx.listModuleInfo[0].moduleTargetFullyQualifiedProjectFolder + "\\" + targetFileName;

                if (File.Exists(targetFullyQualifiedFileName))
                {
                    // Scenario #1 above
                    if (Trace.Warning) Console.WriteLine("*WARNING* Custom methods implementation already exists in target project. Processing skipped.\t" + targetFullyQualifiedFileName);
                    break;
                }

                if (File.Exists(classModelFullyQualifiedFileName)) // Read the file from the Model Class project
                {
                    if (Trace.Warning) Console.WriteLine("**INFO*** Using custom methods implementation found in model class project.\t" + classModelFullyQualifiedFileName);
                    textTemplate = File.ReadAllText(classModelFullyQualifiedFileName);
                }
                else // Read the template from the Embedded Resources 
                {                    
                    string resname = cii.interfaceOutputName + "_cs.txt";
                    if (Trace.Warning) Console.WriteLine("**INFO*** Using embedded custom methods implementation template.\t" + resname);
                    textTemplate = Helpers.GetTextResource(resname);
                }

                text = textTemplate.Replace("#PROGRAMNAME#", Program.ProgramName);
                text = text.Replace("#PROGRAMVERSION#", Assembly.GetEntryAssembly().GetName().Version.ToString());
                text = text.Replace("#NOWDATETIME#", DateTime.Now.ToString());

                text = text.Replace(ctx.listModuleInfo[0].moduleModelClassProjectName, "#NAMESPACE#");
                text = text.Replace("#NAMESPACE#", ctx.listModuleInfo[0].moduleTargetProjectName);

                text = text.Replace("#CLASSNAME#", ctx.listClassInfo[classIndex].classOutputName);
                text = text.Replace(ctx.listClassInfo[classIndex].classOutputName + ":", ctx.listClassInfo[classIndex].classOutputName); // Replace ':' too
                text = text.Replace(cii.interfaceOutputName, "");
                File.WriteAllText(targetFullyQualifiedFileName, text);
            }

            return success;
        }
    }
}
