﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using VsTemplates.Test.Models;

namespace VsTemplates.Test.Validator
{
    public class VsTemplateValidator
    {
        public static VerifierResultTestModel VerifyTemplateId(string filePath)
        {
            var success = false;
            var message = "TemplateID not defined or defined more than once";

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            
            var templateIdNodes = xmlDoc.GetElementsByTagName("TemplateID");
            if (templateIdNodes.Count == 1)
            {
                var templateId = templateIdNodes.Item(0)?.InnerText;
                if (!string.IsNullOrEmpty(templateId))
                {
                    success = templateId.EndsWith("WTS.local");
                    message = success ? string.Empty : "TemplateID not ends with WTS.local";
                }
                else
                {
                    message = "TemplateID not found";
                }
            }

            return new VerifierResultTestModel(success, message);
        }

        public static VerifierResultTestModel VerifyTemplateName(string filePath)
        {
            var success = false;
            var message = "Name not defined or defined more than once";

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            var templateIdNodes = xmlDoc.GetElementsByTagName("Name");
            if (templateIdNodes.Count == 1)
            {
                var templateId = templateIdNodes.Item(0)?.InnerText;
                if (!string.IsNullOrEmpty(templateId))
                {
                    success = templateId.EndsWith("; local)") || templateId.EndsWith("; local) PREVIEW");
                    message = success ? string.Empty : "Template Name not ends with ; local)";
                }
                else
                {
                    message = "Template Name not found";
                }
            }

            return new VerifierResultTestModel(success, message);
        }

        public static VerifierResultTestModel VerifyProjectTemplatesIncludeWtsTag(string filePath)
        {
            var success = false;
            var message = "ProjectTypeTag not defined";

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            var templateIdNodes = xmlDoc.GetElementsByTagName("ProjectTypeTag");

            if (templateIdNodes.Count > 0)
            {
                if (templateIdNodes.Cast<XmlNode>().Any(n => n.InnerText == "Windows Template Studio"))
                {
                    success = true;
                }
                else
                {
                    message = "ProjectTypeTag does not incude -Windows Template Studio-";
                }
            }
            
            return new VerifierResultTestModel(success, message);
        }

        public static VerifierResultTestModel VerifyWizardProjectTemplatesIncludePlatformTag(string filePath)
        {
            var success = false;
            var message = "CustomParameter $wts.platform$ not defined";

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            var templateIdNodes = xmlDoc.GetElementsByTagName("CustomParameter");
            var list = templateIdNodes.Cast<XmlNode>().Where(n => n.Attributes.Cast<XmlAttribute>().Any(a => a.Name == "Name" && a.InnerText == "$wts.platform$"));
            if (list.Count() == 1)
            {
                success = true;
            }
            else if (list.Count() > 1)
            {
                message = "CustomParameter $wts.platform$ defined more than once";
            }

            return new VerifierResultTestModel(success, message);
        }

        public static VerifierResultTestModel VerifyAllLocalizedTemplatesHaveSameDefinition(IEnumerable<string> filePaths)
        {
            var success = true;
            var message = string.Empty;
            var rootFile = filePaths.First(p => p.Length == filePaths.Min(f => f.Length)) ;
            var rootFileLines = File.ReadAllLines(rootFile);
            foreach (var filePath in filePaths.Where(f => f != rootFile))
            {
                var fileLines = File.ReadAllLines(filePath);
                if (rootFileLines.Length == fileLines.Length)
                {
                    for (var i = 0; i < rootFileLines.Length; i++)
                    {
                        var rootFileLine = rootFileLines.ElementAt(i);
                        var fileLine = fileLines.ElementAt(i);
                        if (rootFileLine != fileLine && !rootFileLine.Contains("<Description>") && !fileLine.Contains("<Description>"))
                        {
                            success = false;
                            message = $"File {Path.GetFileName(filePath)} not match file {Path.GetFileName(rootFile)}, line: {rootFileLine}";
                            break;
                        }
                    }
                }
                else
                {
                    success = false;
                    message = $"File {Path.GetFileName(filePath)} ({fileLines.Length}) not match lines count {Path.GetFileName(rootFile)} ({rootFileLines.Length})";
                }

                if (!success)
                {
                    break;
                }
            }

            return new VerifierResultTestModel(success, message);
        }
    }
}
