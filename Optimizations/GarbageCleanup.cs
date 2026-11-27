using System.Collections;
using HarmonyLib;
using UnityEngine;

namespace ItsFrosti.OptimizationsMod.Optimizations
{
    [HarmonyPatch(typeof(MainGameController), "UpdateAutoAssetCollection")]
    internal static class DeferControlRoomCleanup
    {
        private static bool Prefix()
        {
            return Player.main == null || Player.main.GetBiomeString() != "Precursor_Gun_ControlRoom";
        }
    }

    [HarmonyPatch(typeof(EnzymeBall), "OnHandClick")]
    internal static class CleanupDuringCure
    {
        private static void Prefix(EnzymeBall __instance, bool ___playerCured, out bool __state)
        {
            __state = !___playerCured && !__instance.playerCinematicController.cinematicModeActive;
        }

        private static void Postfix(EnzymeBall __instance, bool __state)
        {
            if (__state && __instance.playerCinematicController.cinematicModeActive)
                __instance.StartCoroutine(CollectAfterAnimationStarts(__instance.playerCinematicController));
        }

        private static IEnumerator CollectAfterAnimationStarts(PlayerCinematicController cinematic)
        {
            while (cinematic != null && cinematic.cinematicModeActive && !cinematic.animState)
                yield return null;

            // Let the animation render before the synchronous part of collection.
            yield return new WaitForEndOfFrame();
            yield return null;
            if (cinematic != null && cinematic.cinematicModeActive && cinematic.animState && MainGameController.Instance != null)
                MainGameController.Instance.PerformGarbageAndAssetCollection();
        }
    }
}
