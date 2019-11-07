// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Templates.Core;

namespace Microsoft.Templates.Fakes
{
    public class FakeSolution
    {
        private const string ProjectConfigurationPlatformsText = "GlobalSection(ProjectConfigurationPlatforms) = postSolution";

        private const string UwpProjectConfigurationTemplate = @"		{0}.Debug|Any CPU.ActiveCfg = Debug|x86
		{0}.Debug|Any CPU.Build.0 = Debug|x86
		{0}.Debug|Any CPU.Deploy.0 = Debug|x86
        {0}.Debug|ARM.ActiveCfg = Debug|ARM
		{0}.Debug|ARM.Build.0 = Debug|ARM
		{0}.Debug|ARM.Deploy.0 = Debug|ARM
		{0}.Debug|ARM64.ActiveCfg = Debug|ARM64
		{0}.Debug|ARM64.Build.0 = Debug|ARM64
		{0}.Debug|ARM64.Deploy.0 = Debug|ARM64
		{0}.Debug|x64.ActiveCfg = Debug|x64
		{0}.Debug|x64.Build.0 = Debug|x64
		{0}.Debug|x64.Deploy.0 = Debug|x64
		{0}.Debug|x86.ActiveCfg = Debug|x86
		{0}.Debug|x86.Build.0 = Debug|x86
		{0}.Debug|x86.Deploy.0 = Debug|x86
        {0}.Release|Any CPU.ActiveCfg = Release|x86
		{0}.Release|Any CPU.Build.0 = Release|x86
		{0}.Release|Any CPU.Deploy.0 = Release|x86
		{0}.Release|ARM.ActiveCfg = Release|ARM
		{0}.Release|ARM.Build.0 = Release|ARM
		{0}.Release|ARM.Deploy.0 = Release|ARM
        {0}.Release|ARM64.ActiveCfg = Release|ARM64
		{0}.Release|ARM64.Build.0 = Release|ARM64
		{0}.Release|ARM64.Deploy.0 = Release|ARM64
		{0}.Release|x64.ActiveCfg = Release|x64
		{0}.Release|x64.Build.0 = Release|x64
		{0}.Release|x64.Deploy.0 = Release|x64
		{0}.Release|x86.ActiveCfg = Release|x86
		{0}.Release|x86.Build.0 = Release|x86
		{0}.Release|x86.Deploy.0 = Release|x86
";

        private const string UwpProjectConfigurationTemplateForAnyCpu = @"		{0}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{0}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{0}.Debug|ARM.ActiveCfg = Debug|Any CPU
		{0}.Debug|ARM.Build.0 = Debug|Any CPU
        {0}.Debug|ARM64.ActiveCfg = Debug|Any CPU
		{0}.Debug|ARM64.Build.0 = Debug|Any CPU
		{0}.Debug|x64.ActiveCfg = Debug|Any CPU
		{0}.Debug|x64.Build.0 = Debug|Any CPU
		{0}.Debug|x86.ActiveCfg = Debug|Any CPU
		{0}.Debug|x86.Build.0 = Debug|Any CPU
		{0}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{0}.Release|Any CPU.Build.0 = Release|Any CPU
		{0}.Release|ARM.ActiveCfg = Release|Any CPU
		{0}.Release|ARM.Build.0 = Release|Any CPU
        {0}.Release|ARM64.ActiveCfg = Release|Any CPU
		{0}.Release|ARM64.Build.0 = Release|Any CPU
		{0}.Release|x64.ActiveCfg = Release|Any CPU
		{0}.Release|x64.Build.0 = Release|Any CPU
		{0}.Release|x86.ActiveCfg = Release|Any CPU
		{0}.Release|x86.Build.0 = Release|Any CPU
";

        private const string SharedProjectTemplate = @"GlobalSection(SharedMSBuildProjectFiles) = preSolution
		{0}\{0}.projitems*{1}*SharedItemsImports = {2}
	EndGlobalSection
	";

        private const string XplatDroidTemplate = @"		{0}.Ad-Hoc|Any CPU.ActiveCfg = Release|Any CPU
		{0}.Ad-Hoc|Any CPU.Build.0 = Release|Any CPU
		{0}.Ad-Hoc|Any CPU.Deploy.0 = Release|Any CPU
		{0}.Ad-Hoc|ARM.ActiveCfg = Release|Any CPU
		{0}.Ad-Hoc|ARM.Build.0 = Release|Any CPU
		{0}.Ad-Hoc|ARM.Deploy.0 = Release|Any CPU
		{0}.Ad-Hoc|iPhone.ActiveCfg = Release|Any CPU
		{0}.Ad-Hoc|iPhone.Build.0 = Release|Any CPU
		{0}.Ad-Hoc|iPhone.Deploy.0 = Release|Any CPU
		{0}.Ad-Hoc|iPhoneSimulator.ActiveCfg = Release|Any CPU
		{0}.Ad-Hoc|iPhoneSimulator.Build.0 = Release|Any CPU
		{0}.Ad-Hoc|iPhoneSimulator.Deploy.0 = Release|Any CPU
		{0}.Ad-Hoc|x64.ActiveCfg = Release|Any CPU
		{0}.Ad-Hoc|x64.Build.0 = Release|Any CPU
		{0}.Ad-Hoc|x64.Deploy.0 = Release|Any CPU
		{0}.Ad-Hoc|x86.ActiveCfg = Release|Any CPU
		{0}.Ad-Hoc|x86.Build.0 = Release|Any CPU
		{0}.Ad-Hoc|x86.Deploy.0 = Release|Any CPU
		{0}.AppStore|Any CPU.ActiveCfg = Release|Any CPU
		{0}.AppStore|Any CPU.Build.0 = Release|Any CPU
		{0}.AppStore|Any CPU.Deploy.0 = Release|Any CPU
		{0}.AppStore|ARM.ActiveCfg = Release|Any CPU
		{0}.AppStore|ARM.Build.0 = Release|Any CPU
		{0}.AppStore|ARM.Deploy.0 = Release|Any CPU
		{0}.AppStore|iPhone.ActiveCfg = Release|Any CPU
		{0}.AppStore|iPhone.Build.0 = Release|Any CPU
		{0}.AppStore|iPhone.Deploy.0 = Release|Any CPU
		{0}.AppStore|iPhoneSimulator.ActiveCfg = Release|Any CPU
		{0}.AppStore|iPhoneSimulator.Build.0 = Release|Any CPU
		{0}.AppStore|iPhoneSimulator.Deploy.0 = Release|Any CPU
		{0}.AppStore|x64.ActiveCfg = Release|Any CPU
		{0}.AppStore|x64.Build.0 = Release|Any CPU
		{0}.AppStore|x64.Deploy.0 = Release|Any CPU
		{0}.AppStore|x86.ActiveCfg = Release|Any CPU
		{0}.AppStore|x86.Build.0 = Release|Any CPU
		{0}.AppStore|x86.Deploy.0 = Release|Any CPU
		{0}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{0}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{0}.Debug|Any CPU.Deploy.0 = Debug|Any CPU
		{0}.Debug|ARM.ActiveCfg = Debug|Any CPU
		{0}.Debug|ARM.Build.0 = Debug|Any CPU
		{0}.Debug|ARM.Deploy.0 = Debug|Any CPU
		{0}.Debug|iPhone.ActiveCfg = Debug|Any CPU
		{0}.Debug|iPhone.Build.0 = Debug|Any CPU
		{0}.Debug|iPhone.Deploy.0 = Debug|Any CPU
		{0}.Debug|iPhoneSimulator.ActiveCfg = Debug|Any CPU
		{0}.Debug|iPhoneSimulator.Build.0 = Debug|Any CPU
		{0}.Debug|iPhoneSimulator.Deploy.0 = Debug|Any CPU
		{0}.Debug|x64.ActiveCfg = Debug|Any CPU
		{0}.Debug|x64.Build.0 = Debug|Any CPU
		{0}.Debug|x64.Deploy.0 = Debug|Any CPU
		{0}.Debug|x86.ActiveCfg = Debug|Any CPU
		{0}.Debug|x86.Build.0 = Debug|Any CPU
		{0}.Debug|x86.Deploy.0 = Debug|Any CPU
		{0}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{0}.Release|Any CPU.Build.0 = Release|Any CPU
		{0}.Release|Any CPU.Deploy.0 = Release|Any CPU
		{0}.Release|ARM.ActiveCfg = Release|Any CPU
		{0}.Release|ARM.Build.0 = Release|Any CPU
		{0}.Release|ARM.Deploy.0 = Release|Any CPU
		{0}.Release|iPhone.ActiveCfg = Release|Any CPU
		{0}.Release|iPhone.Build.0 = Release|Any CPU
		{0}.Release|iPhone.Deploy.0 = Release|Any CPU
		{0}.Release|iPhoneSimulator.ActiveCfg = Release|Any CPU
		{0}.Release|iPhoneSimulator.Build.0 = Release|Any CPU
		{0}.Release|iPhoneSimulator.Deploy.0 = Release|Any CPU
		{0}.Release|x64.ActiveCfg = Release|Any CPU
		{0}.Release|x64.Build.0 = Release|Any CPU
		{0}.Release|x64.Deploy.0 = Release|Any CPU
		{0}.Release|x86.ActiveCfg = Release|Any CPU
		{0}.Release|x86.Build.0 = Release|Any CPU
		{0}.Release|x86.Deploy.0 = Release|Any CPU
";

        private const string XplatIosTemplate = @"		{0}.Ad-Hoc|Any CPU.ActiveCfg = Ad-Hoc|iPhone
		{0}.Ad-Hoc|ARM.ActiveCfg = Ad-Hoc|iPhone
		{0}.Ad-Hoc|iPhone.ActiveCfg = Ad-Hoc|iPhone
		{0}.Ad-Hoc|iPhone.Build.0 = Ad-Hoc|iPhone
		{0}.Ad-Hoc|iPhoneSimulator.ActiveCfg = Ad-Hoc|iPhoneSimulator
		{0}.Ad-Hoc|iPhoneSimulator.Build.0 = Ad-Hoc|iPhoneSimulator
		{0}.Ad-Hoc|x64.ActiveCfg = Ad-Hoc|iPhone
		{0}.Ad-Hoc|x86.ActiveCfg = Ad-Hoc|iPhone
		{0}.AppStore|Any CPU.ActiveCfg = AppStore|iPhone
		{0}.AppStore|ARM.ActiveCfg = AppStore|iPhone
		{0}.AppStore|iPhone.ActiveCfg = AppStore|iPhone
		{0}.AppStore|iPhone.Build.0 = AppStore|iPhone
		{0}.AppStore|iPhoneSimulator.ActiveCfg = AppStore|iPhoneSimulator
		{0}.AppStore|iPhoneSimulator.Build.0 = AppStore|iPhoneSimulator
		{0}.AppStore|x64.ActiveCfg = AppStore|iPhone
		{0}.AppStore|x86.ActiveCfg = AppStore|iPhone
		{0}.Debug|Any CPU.ActiveCfg = Debug|iPhone
		{0}.Debug|ARM.ActiveCfg = Debug|iPhone
		{0}.Debug|iPhone.ActiveCfg = Debug|iPhone
		{0}.Debug|iPhone.Build.0 = Debug|iPhone
		{0}.Debug|iPhoneSimulator.ActiveCfg = Debug|iPhoneSimulator
		{0}.Debug|iPhoneSimulator.Build.0 = Debug|iPhoneSimulator
		{0}.Debug|x64.ActiveCfg = Debug|iPhone
		{0}.Debug|x86.ActiveCfg = Debug|iPhone
		{0}.Release|Any CPU.ActiveCfg = Release|iPhone
		{0}.Release|ARM.ActiveCfg = Release|iPhone
		{0}.Release|iPhone.ActiveCfg = Release|iPhone
		{0}.Release|iPhone.Build.0 = Release|iPhone
		{0}.Release|iPhoneSimulator.ActiveCfg = Release|iPhoneSimulator
		{0}.Release|iPhoneSimulator.Build.0 = Release|iPhoneSimulator
		{0}.Release|x64.ActiveCfg = Release|iPhone
		{0}.Release|x86.ActiveCfg = Release|iPhone
";

        private const string XplatUwpTemplate = @"		{0}.Ad-Hoc|Any CPU.ActiveCfg = Release|x64
		{0}.Ad-Hoc|Any CPU.Build.0 = Release|x64
		{0}.Ad-Hoc|Any CPU.Deploy.0 = Release|x64
		{0}.Ad-Hoc|ARM.ActiveCfg = Release|ARM
		{0}.Ad-Hoc|ARM.Build.0 = Release|ARM
		{0}.Ad-Hoc|ARM.Deploy.0 = Release|ARM
		{0}.Ad-Hoc|iPhone.ActiveCfg = Release|x64
		{0}.Ad-Hoc|iPhone.Build.0 = Release|x64
		{0}.Ad-Hoc|iPhone.Deploy.0 = Release|x64
		{0}.Ad-Hoc|iPhoneSimulator.ActiveCfg = Release|x64
		{0}.Ad-Hoc|iPhoneSimulator.Build.0 = Release|x64
		{0}.Ad-Hoc|iPhoneSimulator.Deploy.0 = Release|x64
		{0}.Ad-Hoc|x64.ActiveCfg = Release|x64
		{0}.Ad-Hoc|x64.Build.0 = Release|x64
		{0}.Ad-Hoc|x64.Deploy.0 = Release|x64
		{0}.Ad-Hoc|x86.ActiveCfg = Release|x86
		{0}.Ad-Hoc|x86.Build.0 = Release|x86
		{0}.Ad-Hoc|x86.Deploy.0 = Release|x86
		{0}.AppStore|Any CPU.ActiveCfg = Release|x64
		{0}.AppStore|Any CPU.Build.0 = Release|x64
		{0}.AppStore|Any CPU.Deploy.0 = Release|x64
		{0}.AppStore|ARM.ActiveCfg = Release|ARM
		{0}.AppStore|ARM.Build.0 = Release|ARM
		{0}.AppStore|ARM.Deploy.0 = Release|ARM
		{0}.AppStore|iPhone.ActiveCfg = Release|x64
		{0}.AppStore|iPhone.Build.0 = Release|x64
		{0}.AppStore|iPhone.Deploy.0 = Release|x64
		{0}.AppStore|iPhoneSimulator.ActiveCfg = Release|x64
		{0}.AppStore|iPhoneSimulator.Build.0 = Release|x64
		{0}.AppStore|iPhoneSimulator.Deploy.0 = Release|x64
		{0}.AppStore|x64.ActiveCfg = Release|x64
		{0}.AppStore|x64.Build.0 = Release|x64
		{0}.AppStore|x64.Deploy.0 = Release|x64
		{0}.AppStore|x86.ActiveCfg = Release|x86
		{0}.AppStore|x86.Build.0 = Release|x86
		{0}.AppStore|x86.Deploy.0 = Release|x86
		{0}.Debug|Any CPU.ActiveCfg = Debug|x86
		{0}.Debug|Any CPU.Build.0 = Debug|x86
		{0}.Debug|Any CPU.Deploy.0 = Debug|x86
		{0}.Debug|ARM.ActiveCfg = Debug|ARM
		{0}.Debug|ARM.Build.0 = Debug|ARM
		{0}.Debug|ARM.Deploy.0 = Debug|ARM
		{0}.Debug|iPhone.ActiveCfg = Debug|x86
		{0}.Debug|iPhone.Build.0 = Debug|x86
		{0}.Debug|iPhone.Deploy.0 = Debug|x86
		{0}.Debug|iPhoneSimulator.ActiveCfg = Debug|x86
		{0}.Debug|iPhoneSimulator.Build.0 = Debug|x86
		{0}.Debug|iPhoneSimulator.Deploy.0 = Debug|x86
		{0}.Debug|x64.ActiveCfg = Debug|x64
		{0}.Debug|x64.Build.0 = Debug|x64
		{0}.Debug|x64.Deploy.0 = Debug|x64
		{0}.Debug|x86.ActiveCfg = Debug|x86
		{0}.Debug|x86.Build.0 = Debug|x86
		{0}.Debug|x86.Deploy.0 = Debug|x86
		{0}.Release|Any CPU.ActiveCfg = Release|x86
		{0}.Release|Any CPU.Build.0 = Release|x86
		{0}.Release|Any CPU.Deploy.0 = Release|x86
		{0}.Release|ARM.ActiveCfg = Release|ARM
		{0}.Release|ARM.Build.0 = Release|ARM
		{0}.Release|ARM.Deploy.0 = Release|ARM
		{0}.Release|iPhone.ActiveCfg = Release|x86
		{0}.Release|iPhone.Build.0 = Release|x86
		{0}.Release|iPhone.Deploy.0 = Release|x86
		{0}.Release|iPhoneSimulator.ActiveCfg = Release|x86
		{0}.Release|iPhoneSimulator.Build.0 = Release|x86
		{0}.Release|iPhoneSimulator.Deploy.0 = Release|x86
		{0}.Release|x64.ActiveCfg = Release|x64
		{0}.Release|x64.Build.0 = Release|x64
		{0}.Release|x64.Deploy.0 = Release|x64
		{0}.Release|x86.ActiveCfg = Release|x86
		{0}.Release|x86.Build.0 = Release|x86
		{0}.Release|x86.Deploy.0 = Release|x86
";

        private const string StdLibTemplate = @"		{0}.Ad-Hoc|Any CPU.ActiveCfg = Debug|Any CPU
		{0}.Ad-Hoc|Any CPU.Build.0 = Debug|Any CPU
		{0}.Ad-Hoc|Any CPU.Deploy.0 = Debug|Any CPU
		{0}.Ad-Hoc|ARM.ActiveCfg = Debug|Any CPU
		{0}.Ad-Hoc|ARM.Build.0 = Debug|Any CPU
		{0}.Ad-Hoc|ARM.Deploy.0 = Debug|Any CPU
		{0}.Ad-Hoc|iPhone.ActiveCfg = Debug|Any CPU
		{0}.Ad-Hoc|iPhone.Build.0 = Debug|Any CPU
		{0}.Ad-Hoc|iPhone.Deploy.0 = Debug|Any CPU
		{0}.Ad-Hoc|iPhoneSimulator.ActiveCfg = Debug|Any CPU
		{0}.Ad-Hoc|iPhoneSimulator.Build.0 = Debug|Any CPU
		{0}.Ad-Hoc|iPhoneSimulator.Deploy.0 = Debug|Any CPU
		{0}.Ad-Hoc|x64.ActiveCfg = Debug|Any CPU
		{0}.Ad-Hoc|x64.Build.0 = Debug|Any CPU
		{0}.Ad-Hoc|x64.Deploy.0 = Debug|Any CPU
		{0}.Ad-Hoc|x86.ActiveCfg = Debug|Any CPU
		{0}.Ad-Hoc|x86.Build.0 = Debug|Any CPU
		{0}.Ad-Hoc|x86.Deploy.0 = Debug|Any CPU
		{0}.AppStore|Any CPU.ActiveCfg = Debug|Any CPU
		{0}.AppStore|Any CPU.Build.0 = Debug|Any CPU
		{0}.AppStore|Any CPU.Deploy.0 = Debug|Any CPU
		{0}.AppStore|ARM.ActiveCfg = Debug|Any CPU
		{0}.AppStore|ARM.Build.0 = Debug|Any CPU
		{0}.AppStore|ARM.Deploy.0 = Debug|Any CPU
		{0}.AppStore|iPhone.ActiveCfg = Debug|Any CPU
		{0}.AppStore|iPhone.Build.0 = Debug|Any CPU
		{0}.AppStore|iPhone.Deploy.0 = Debug|Any CPU
		{0}.AppStore|iPhoneSimulator.ActiveCfg = Debug|Any CPU
		{0}.AppStore|iPhoneSimulator.Build.0 = Debug|Any CPU
		{0}.AppStore|iPhoneSimulator.Deploy.0 = Debug|Any CPU
		{0}.AppStore|x64.ActiveCfg = Debug|Any CPU
		{0}.AppStore|x64.Build.0 = Debug|Any CPU
		{0}.AppStore|x64.Deploy.0 = Debug|Any CPU
		{0}.AppStore|x86.ActiveCfg = Debug|Any CPU
		{0}.AppStore|x86.Build.0 = Debug|Any CPU
		{0}.AppStore|x86.Deploy.0 = Debug|Any CPU
		{0}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{0}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{0}.Debug|Any CPU.Deploy.0 = Debug|Any CPU
		{0}.Debug|ARM.ActiveCfg = Debug|Any CPU
		{0}.Debug|ARM.Build.0 = Debug|Any CPU
		{0}.Debug|ARM.Deploy.0 = Debug|Any CPU
		{0}.Debug|iPhone.ActiveCfg = Debug|Any CPU
		{0}.Debug|iPhone.Build.0 = Debug|Any CPU
		{0}.Debug|iPhone.Deploy.0 = Debug|Any CPU
		{0}.Debug|iPhoneSimulator.ActiveCfg = Debug|Any CPU
		{0}.Debug|iPhoneSimulator.Build.0 = Debug|Any CPU
		{0}.Debug|iPhoneSimulator.Deploy.0 = Debug|Any CPU
		{0}.Debug|x64.ActiveCfg = Debug|Any CPU
		{0}.Debug|x64.Build.0 = Debug|Any CPU
		{0}.Debug|x64.Deploy.0 = Debug|Any CPU
		{0}.Debug|x86.ActiveCfg = Debug|Any CPU
		{0}.Debug|x86.Build.0 = Debug|Any CPU
		{0}.Debug|x86.Deploy.0 = Debug|Any CPU
		{0}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{0}.Release|Any CPU.Build.0 = Release|Any CPU
		{0}.Release|Any CPU.Deploy.0 = Release|Any CPU
		{0}.Release|ARM.ActiveCfg = Release|Any CPU
		{0}.Release|ARM.Build.0 = Release|Any CPU
		{0}.Release|ARM.Deploy.0 = Release|Any CPU
		{0}.Release|iPhone.ActiveCfg = Release|Any CPU
		{0}.Release|iPhone.Build.0 = Release|Any CPU
		{0}.Release|iPhone.Deploy.0 = Release|Any CPU
		{0}.Release|iPhoneSimulator.ActiveCfg = Release|Any CPU
		{0}.Release|iPhoneSimulator.Build.0 = Release|Any CPU
		{0}.Release|iPhoneSimulator.Deploy.0 = Release|Any CPU
		{0}.Release|x64.ActiveCfg = Release|Any CPU
		{0}.Release|x64.Build.0 = Release|Any CPU
		{0}.Release|x64.Deploy.0 = Release|Any CPU
		{0}.Release|x86.ActiveCfg = Release|Any CPU
		{0}.Release|x86.Build.0 = Release|Any CPU
		{0}.Release|x86.Deploy.0 = Release|Any CPU
";

        private const string XplatWasmTemplate = @"		{0}.Ad-Hoc|Any CPU.ActiveCfg = Debug|Any CPU
		{0}.Ad-Hoc|Any CPU.Build.0 = Debug|Any CPU
		{0}.Ad-Hoc|ARM.ActiveCfg = Debug|Any CPU
		{0}.Ad-Hoc|ARM.Build.0 = Debug|Any CPU
		{0}.Ad-Hoc|iPhone.ActiveCfg = Debug|Any CPU
		{0}.Ad-Hoc|iPhone.Build.0 = Debug|Any CPU
		{0}.Ad-Hoc|iPhoneSimulator.ActiveCfg = Debug|Any CPU
		{0}.Ad-Hoc|iPhoneSimulator.Build.0 = Debug|Any CPU
		{0}.Ad-Hoc|x64.ActiveCfg = Debug|Any CPU
		{0}.Ad-Hoc|x64.Build.0 = Debug|Any CPU
		{0}.Ad-Hoc|x86.ActiveCfg = Debug|Any CPU
		{0}.Ad-Hoc|x86.Build.0 = Debug|Any CPU
		{0}.AppStore|Any CPU.ActiveCfg = Debug|Any CPU
		{0}.AppStore|Any CPU.Build.0 = Debug|Any CPU
		{0}.AppStore|ARM.ActiveCfg = Debug|Any CPU
		{0}.AppStore|ARM.Build.0 = Debug|Any CPU
		{0}.AppStore|iPhone.ActiveCfg = Debug|Any CPU
		{0}.AppStore|iPhone.Build.0 = Debug|Any CPU
		{0}.AppStore|iPhoneSimulator.ActiveCfg = Debug|Any CPU
		{0}.AppStore|iPhoneSimulator.Build.0 = Debug|Any CPU
		{0}.AppStore|x64.ActiveCfg = Debug|Any CPU
		{0}.AppStore|x64.Build.0 = Debug|Any CPU
		{0}.AppStore|x86.ActiveCfg = Debug|Any CPU
		{0}.AppStore|x86.Build.0 = Debug|Any CPU
		{0}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{0}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{0}.Debug|ARM.ActiveCfg = Debug|Any CPU
		{0}.Debug|ARM.Build.0 = Debug|Any CPU
		{0}.Debug|iPhone.ActiveCfg = Debug|Any CPU
		{0}.Debug|iPhone.Build.0 = Debug|Any CPU
		{0}.Debug|iPhoneSimulator.ActiveCfg = Debug|Any CPU
		{0}.Debug|iPhoneSimulator.Build.0 = Debug|Any CPU
		{0}.Debug|x64.ActiveCfg = Debug|Any CPU
		{0}.Debug|x64.Build.0 = Debug|Any CPU
		{0}.Debug|x86.ActiveCfg = Debug|Any CPU
		{0}.Debug|x86.Build.0 = Debug|Any CPU
		{0}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{0}.Release|Any CPU.Build.0 = Release|Any CPU
		{0}.Release|ARM.ActiveCfg = Release|Any CPU
		{0}.Release|ARM.Build.0 = Release|Any CPU
		{0}.Release|iPhone.ActiveCfg = Release|Any CPU
		{0}.Release|iPhone.Build.0 = Release|Any CPU
		{0}.Release|iPhoneSimulator.ActiveCfg = Release|Any CPU
		{0}.Release|iPhoneSimulator.Build.0 = Release|Any CPU
		{0}.Release|x64.ActiveCfg = Release|Any CPU
		{0}.Release|x64.Build.0 = Release|Any CPU
		{0}.Release|x86.ActiveCfg = Release|Any CPU
		{0}.Release|x86.Build.0 = Release|Any CPU
";
        private const string WpfProjectConfigurationTemplate = @"		{0}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{0}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{0}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{0}.Release|Any CPU.Build.0 = Release|Any CPU
";

        private const string ProjectTemplate = @"Project(""{{guid}}"") = ""{name}"", ""{path}"", ""{id}""
EndProject
";

        private const string ProjectFilter = @"Project\(\""(?<typeGuid>.*?)\""\)\s+=\s+\""(?<name>.*?)\"",\s+\""(?<path>.*?)\"",\s+\""(?<guid>.*?)\""(?<content>.*?)\bEndProject\b";

        private readonly string _path;

        private FakeSolution(string path)
        {
            _path = path;
        }

        public static FakeSolution LoadOrCreate(string platform, string path)
        {
            if (!File.Exists(path))
            {
                var solutionTemplate = ReadTemplate(platform);

                File.WriteAllText(path, solutionTemplate, Encoding.UTF8);
            }

            return new FakeSolution(path);
        }

        public void AddProjectToSolution(string platform, string projectName, string projectGuid, string projectRelativeToSolutionPath, bool isCPSProject)
        {
            var slnContent = GetSolutionFileContent();

            if (slnContent.IndexOf(projectRelativeToSolutionPath, StringComparison.Ordinal) == -1)
            {
                var globalIndex = slnContent.IndexOf("Global", StringComparison.Ordinal);
                var projectTypeGuid = GetProjectGuid(Path.GetExtension(projectRelativeToSolutionPath), isCPSProject);
                projectGuid = projectGuid.Contains("{") ? projectGuid : "{" + projectGuid + "}";
                var projectContent = ProjectTemplate
                                            .Replace("{guid}", projectTypeGuid)
                                            .Replace("{name}", projectName)
                                            .Replace("{path}", projectRelativeToSolutionPath)
                                            .Replace("{id}", projectGuid);

                slnContent = slnContent.Insert(globalIndex, projectContent);

                if (platform == Platforms.Xplat && projectName.EndsWith(".Shared", StringComparison.InvariantCultureIgnoreCase))
                {
                    var sharedProjInsertPoint = slnContent.IndexOf("GlobalSection(SolutionConfigurationPlatforms) = preSolution", StringComparison.Ordinal);

                    // TODO ML: calculate number of items???
                    var sharedItemCount = 13;

                    var sharedProjectContent = string.Format(SharedProjectTemplate, projectName, projectGuid, sharedItemCount);

                    slnContent = slnContent.Insert(sharedProjInsertPoint, sharedProjectContent);
                }
                else
                {
                    var projectConfigurationTemplate = GetProjectConfigurationTemplate(platform, projectName, isCPSProject);
                    if (!string.IsNullOrEmpty(projectConfigurationTemplate))
                    {
                        var globalSectionIndex = slnContent.IndexOf(ProjectConfigurationPlatformsText, StringComparison.Ordinal);

                        var endGobalSectionIndex = slnContent.IndexOf("EndGlobalSection", globalSectionIndex, StringComparison.Ordinal);

                        var projectConfigContent = string.Format(projectConfigurationTemplate, projectGuid);

                        slnContent = slnContent.Insert(endGobalSectionIndex - 1, projectConfigContent);
                    }
                }

                if (platform == Platforms.Uwp && isCPSProject)
                {
                    slnContent = AddAnyCpuSolutionConfigurations(slnContent);
                    slnContent = AddAnyCpuProjectConfigutations(slnContent);
                }
            }

            File.WriteAllText(_path, slnContent, Encoding.UTF8);
        }

        public Dictionary<string, string> GetProjectGuids()
        {
            var result = new Dictionary<string, string>();

            var projectPattern = new Regex(ProjectFilter, RegexOptions.ExplicitCapture | RegexOptions.Singleline);
            var solutionContent = GetSolutionFileContent();
            var match = projectPattern.Match(solutionContent);

            while (match.Success)
            {
                result.Add(match.Groups["name"].Value, match.Groups["guid"].Value);
                match = match.NextMatch();
            }

            return result;
        }

        private string AddAnyCpuProjectConfigutations(string slnContent)
        {
            if (slnContent.Contains("|Any CPU = "))
            {
                // Ensure that all projects have 'Any CPU' platform configurations
                var slnLines = slnContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                var projectGuids = new List<string>();

                foreach (var line in slnLines)
                {
                    if (line.StartsWith("Project(\"{", StringComparison.OrdinalIgnoreCase))
                    {
                        projectGuids.Add(line.Substring(line.LastIndexOf("{", StringComparison.OrdinalIgnoreCase)).Trim(new[] { '{', '}', '"' }));
                    }

                    if (line.StartsWith("Global", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }
                }

                // see if already have an entry
                // if not, add them above the ARM entries
                foreach (var projGuid in projectGuids)
                {
                    if (!slnContent.Contains($"{{{projGuid}}}.Debug|Any CPU.ActiveCfg"))
                    {
                        slnContent = slnContent.Replace($"{{{projGuid}}}.Debug|ARM.ActiveCfg", $"{{{projGuid}}}.Debug|Any CPU.ActiveCfg = Debug|x86\r\n\t\t{{{projGuid}}}.Debug|ARM.ActiveCfg");
                    }

                    if (!slnContent.Contains($"{{{projGuid}}}.Release|Any CPU.ActiveCfg"))
                    {
                        slnContent = slnContent.Replace($"{{{projGuid}}}.Release|ARM.ActiveCfg", $"{{{projGuid}}}.Release|Any CPU.ActiveCfg = Release|x86\r\n\t\t{{{projGuid}}}.Release|ARM.ActiveCfg");
                    }
                }
            }

            return slnContent;
        }

        private string AddAnyCpuSolutionConfigurations(string slnContent)
        {
            if (!slnContent.Contains("Debug|Any CPU = Debug|Any CPU"))
            {
                slnContent = slnContent.Replace("Debug|ARM = Debug|ARM", "Debug|Any CPU = Debug|Any CPU\r\n\t\tDebug|ARM = Debug|ARM");
            }

            if (!slnContent.Contains("Release|Any CPU = Release|Any CPU"))
            {
                slnContent = slnContent.Replace("Release|ARM = Release|ARM", "Release|Any CPU = Release|Any CPU\r\n\t\tRelease|ARM = Release|ARM");
            }

            return slnContent;
        }

        private static string GetProjectGuid(string filePath, bool isCPSProject)
        {
            if (filePath.EndsWith(".wasm.csproj", StringComparison.InvariantCultureIgnoreCase))
            {
                return "9A19103F-16F7-4668-BE54-9A1E7A4F7556"; // An ASP.NET C# project
            }

            var projectExtension = Path.GetExtension(filePath);

            // See https://github.com/dotnet/project-system/blob/master/docs/opening-with-new-project-system.md
            switch (projectExtension)
            {
                case ".csproj":
                    return isCPSProject ? "9A19103F-16F7-4668-BE54-9A1E7A4F7556" : "FAE04EC0-301F-11D3-BF4B-00C04F79EFBC";
                case ".vbproj":
                    return isCPSProject ? "778DAE3C-4631-46EA-AA77-85C1314464D9" : "F184B08F-C81C-45F6-A57F-5ABD9991F28F";
                case ".shproj":
                    return "D954291E-2A0B-460D-934E-DC6B0785DB48";
            }

            return string.Empty;
        }

        private static string GetProjectConfigurationTemplate(string platform, string projectName, bool isCPSProject)
        {
            switch (platform)
            {
                case Platforms.Uwp:
                    if (isCPSProject)
                    {
                        return UwpProjectConfigurationTemplateForAnyCpu;
                    }
                    else
                    {
                        return UwpProjectConfigurationTemplate;
                    }

                case Platforms.Wpf:
                    return WpfProjectConfigurationTemplate;

                case Platforms.Xplat:
					if (projectName.EndsWith(".Droid", StringComparison.InvariantCultureIgnoreCase))
					{
						return XplatDroidTemplate;
					}
					else if (projectName.EndsWith(".Android", StringComparison.InvariantCultureIgnoreCase))
					{
						return XplatDroidTemplate;
					}
					else if (projectName.EndsWith(".iOS", StringComparison.InvariantCultureIgnoreCase))
					{
						return XplatIosTemplate;
					}
					else if (projectName.EndsWith(".UWP", StringComparison.InvariantCultureIgnoreCase))
					{
						return XplatUwpTemplate;
					}
					else if (projectName.EndsWith(".Wasm", StringComparison.InvariantCultureIgnoreCase))
					{
						return XplatWasmTemplate;
					}
					else
					{
						return StdLibTemplate;
					}

                default:
                    return string.Empty;
            }
        }

        private static string ReadTemplate(string platform)
        {
            switch (platform)
            {
                case Platforms.Uwp:
                    return File.ReadAllText(@"Solution\UwpSolutionTemplate.txt");
                case Platforms.Wpf:
                    return File.ReadAllText(@"Solution\WpfSolutionTemplate.txt");
                case Platforms.Xplat:
                    return File.ReadAllText(@"Solution\XplatSolutionTemplate.txt");
            }

            throw new InvalidDataException(nameof(platform));
        }

        private string GetSolutionFileContent()
        {
            if (!File.Exists(_path))
            {
                throw new FileNotFoundException(string.Format("Solution file {0} does not exist", _path));
            }

            return File.ReadAllText(_path);
        }
    }
}
