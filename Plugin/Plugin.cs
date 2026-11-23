using SNSRModLoader;
using System.Reflection;
using System.Runtime.InteropServices;
using ModPlugin = ItsFrosti.OptimizationsMod.Plugin;

[assembly: AssemblyTitle(ModPlugin.PluginName)]
[assembly: AssemblyProduct(ModPlugin.PluginName)]
[assembly: AssemblyVersion(ModPlugin.PluginVersion)]
[assembly: AssemblyFileVersion(ModPlugin.PluginVersion)]
[assembly: AssemblyInformationalVersion(ModPlugin.PluginVersion)]
[assembly: AssemblyDescription("Optimization Mod for Subnautica using SNSRModLoader | Specifically built for the September 2018 Speedrun Version.")]
[assembly: AssemblyCopyright("Copyright © 2026 ItsFrosti")]
[assembly: Guid("c7bc4d63-b1be-4dda-968d-a89d88c47032")]
[assembly: ComVisible(false)]

namespace ItsFrosti.OptimizationsMod
{
    [SNSRModLoader(PluginId, PluginName, PluginVersion,
        AutomaticUpdate = Plugin.AutomaticUpdateEnabled,
        UpdateRepository = Plugin.UpdateRepository,
        UpdateManifestUrl = Plugin.UpdateManifestUrl)]
    [TargetVersion(GameVersion.September2018)]
    public sealed class Plugin : SNSRModLoader.Plugin
    {
        public const string PluginId = "ItsFrosti.OptimizationsMod";
        public const string PluginName = "Optimizations Mod";
        public const string PluginVersion = "1.0.2";
        public const bool AutomaticUpdateEnabled = true;
        public const string UpdateRepository = "ItsFrostyYo/SubnauticaOptimizationMod";
        public const string UpdateManifestUrl = "https://raw.githubusercontent.com/" + UpdateRepository + "/main/dist/mod-update.xml";

        public override void Load(PluginContext context)
        {
            PluginSettings.Initialize(context.Settings);
            context.Harmony.PatchAll(typeof(Plugin).Assembly);
            context.Logger.Info("Subnautica Optimization Mod Loaded");
        }

        public override void Unload()
        {
            Optimizations.KeepBaseBundle.ReleaseRetained();
        }
    }
}
