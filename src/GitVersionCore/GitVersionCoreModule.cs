using GitVersion.BuildServers;
using GitVersion.Common;
using GitVersion.Configuration;
using GitVersion.Configuration.Init;
using GitVersion.Extensions;
using GitVersion.Logging;
using GitVersion.VersionCalculation;
using GitVersion.VersionCalculation.Cache;
using LibGit2Sharp;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GitVersion
{
    public class GitVersionCoreModule : IGitVersionModule
    {
        public void RegisterTypes(IServiceCollection services)
        {
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<IEnvironment, Environment>();
            services.AddSingleton<ILog, Log>();
            services.AddSingleton<IConsole, ConsoleAdapter>();
            services.AddSingleton<IGitVersionCache, GitVersionCache>();

            services.AddSingleton<IGitVersionCacheKeyFactory, GitVersionCacheKeyFactory>();
            services.AddSingleton<IGitVersionContextFactory, GitVersionContextFactory>();
            services.AddSingleton<IConfigFileLocatorFactory, ConfigFileLocatorFactory>();

            services.AddSingleton<IConfigProvider, ConfigProvider>();
            services.AddSingleton<IVariableProvider, VariableProvider>();

            services.AddSingleton<IBaseVersionCalculator, BaseVersionCalculator>();
            services.AddSingleton<IMainlineVersionCalculator, MainlineVersionCalculator>();
            services.AddSingleton<INextVersionCalculator, NextVersionCalculator>();
            services.AddSingleton<IGitVersionCalculator, GitVersionCalculator>();
            services.AddSingleton<IBranchConfigurationCalculator, BranchConfigurationCalculator>();

            services.AddSingleton<IBuildServerResolver, BuildServerResolver>();
            services.AddSingleton<IGitPreparer, GitPreparer>();
            services.AddSingleton<IGitRepoMetadataProvider, GitRepoMetadataProvider>();

            services.AddSingleton(sp => sp.GetService<IConfigFileLocatorFactory>().Create());

            services.AddSingleton<IRepository>(sp =>
            {
                var options = sp.GetService<IOptions<Arguments>>();
                return new Repository(options.Value.DotGitDirectory);
            });

            services.AddSingleton(sp =>
            {
                var options = sp.GetService<IOptions<Arguments>>();
                var repository = sp.GetService<IRepository>();
                var contextFactory = sp.GetService<IGitVersionContextFactory>();
                return Options.Create(contextFactory.Create(options.Value, repository));
            });

            services.AddModule(new BuildServerModule());
            services.AddModule(new GitVersionInitModule());
            services.AddModule(new VersionStrategyModule());
        }
    }
}
