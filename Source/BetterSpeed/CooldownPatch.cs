using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace BetterSpeed
{
    [HarmonyPatch(typeof(VerbProperties))]
    [HarmonyPatch(nameof(VerbProperties.AdjustedCooldownTicks))]
    public class CooldownPatch
    {
        public static float _modifier = 2;

        public static void Postfix(object equipment, VerbProperties __instance, ref int __result)
        {
            if(equipment != null && !__instance.MeleeRange)
                __result = Mathf.RoundToInt(__result / _modifier);
        }
    }
}
