using HarmonyLib;
using SNSRModLoader;

namespace ItsFrosti.OptimizationsMod
{
    [HarmonyPatch(typeof(uGUI_OptionsPanel), "AddGraphicsTab")]
    internal static class PluginSettings
    {
        private static SettingsFile settings;
        internal static bool SavePreloadBase { get; private set; } = true;

        internal static void Initialize(SettingsFile file)
        {
            settings = file;
            SavePreloadBase = settings.Get("SavePreloadBase", true);
            settings.Set("SavePreloadBase", SavePreloadBase);
            settings.Save();
        }

        private static void Postfix(uGUI_OptionsPanel __instance)
        {
            int tab = __instance.AddTab("Optimizations");
            __instance.AddToggleOption(tab, "Save Preload Base", SavePreloadBase, SetSavePreloadBase);
        }

        private static void SetSavePreloadBase(bool enabled)
        {
            if (SavePreloadBase == enabled) return;
            SavePreloadBase = enabled;
            if (!enabled) Optimizations.KeepBaseBundle.ReleaseRetained();
            settings.Set("SavePreloadBase", enabled);
            settings.Save();
        }
    }
}
