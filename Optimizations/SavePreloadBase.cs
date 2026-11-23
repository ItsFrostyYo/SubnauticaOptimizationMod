using System;
using System.Collections;
using HarmonyLib;
using UnityEngine;

namespace ItsFrosti.OptimizationsMod.Optimizations
{
    [HarmonyPatch(typeof(Base), "Deinitialize")]
    internal static class KeepBasePrefabs
    {
        internal static Transform[] Prefabs;

        private static void Prefix(Array ___pieces)
        {
            if (!PluginSettings.SavePreloadBase || ___pieces == null) return;
            var prefab = AccessTools.Field(___pieces.GetType().GetElementType(), "prefab");
            Prefabs = new Transform[___pieces.Length]; 
            for (int i = 0; i < Prefabs.Length; i++)
            Prefabs[i] = (Transform)prefab.GetValue(___pieces.GetValue(i));
        }
    }

    [HarmonyPatch(typeof(AssetBundleManager), "Deinitialize")]
    internal static class KeepBaseBundle
    {
        private const string Bundle = "basegeneratorpieces";
        private static bool retained;

        internal static void ReleaseRetained()
        {
            KeepBasePrefabs.Prefabs = null;
            if (!retained || AccessTools.Field(typeof(Base), "pieces").GetValue(null) != null) return;
            retained = false;
            AssetBundleManager.UnloadBundle(Bundle);
        }

        private static void Prefix(IDictionary ___loadedBundles, out object __state)
        {
            __state = null;
            retained = false;
            if (!PluginSettings.SavePreloadBase || KeepBasePrefabs.Prefabs == null || !___loadedBundles.Contains(Bundle)) return;
            __state = ___loadedBundles[Bundle];
            ___loadedBundles.Remove(Bundle);
        }

        private static void Finalizer(IDictionary ___loadedBundles, object __state)
        {
            if (__state == null) return;
            AccessTools.Field(__state.GetType(), "numReferences").SetValue(__state, 1);
            ___loadedBundles[Bundle] = __state;
            retained = true;
        }
    }
}
