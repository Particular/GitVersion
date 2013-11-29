﻿namespace GitFlowVersion
{
    using System;
    using Integration;

    public class TeamCity : IntegrationBase
    {
        public override bool CanApplyToCurrentContext()
        {
            return !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TEAMCITY_VERSION"));
        }

        public override AnalysisResult PerformPreProcessingSteps(ILogger logger, string gitDirectory)
        {
            logger.LogInfo("Executing inside a TeamCityVersionBuilder build agent");

            if (string.IsNullOrEmpty(gitDirectory))
            {
                // ReSharper disable once StringLiteralTypo
                logger.LogError(
                    "Failed to find .git directory on agent. Please make sure agent checkout mode is enabled for you VCS roots - http://confluence.jetbrains.com/display/TCD8/VCS+Checkout+Mode");
                return AnalysisResult.FatalError;
            }

            GitHelper.NormalizeGitDirectory(gitDirectory);

            return AnalysisResult.Ok;
        }

        protected override string GenerateBuildParameter(string name, string value)
        {
            return string.Format("##teamcity[setParameter name='GitFlowVersion.{0}' value='{1}']", name, value);
        }
    }
}
