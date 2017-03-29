using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace BetterSpeed
{
    [HarmonyPatch(typeof(Projectile))]
    [HarmonyPatch("StartingTicksToImpact", PropertyMethod.Getter)]
    public class ProjectilePatch
    {
        public static float _modifier = 2;

        public static void Postfix(ref int __result)
        {
            __result = Mathf.RoundToInt(__result / _modifier);
        }
    }
}
