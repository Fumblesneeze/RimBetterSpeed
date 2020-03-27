using HarmonyLib;
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

        public static void Postfix(VerbProperties __instance, ref int __result)
        {
            if(!__instance.IsMeleeAttack)
                __result = Mathf.RoundToInt(__result / _modifier);
        }
    }
}
