using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace BetterSpeed
{
    [HarmonyPatch(typeof(Projectile))]
    [HarmonyPatch("StartingTicksToImpact", MethodType.Getter)]
    public class ProjectilePatch
    {
        public static float _modifier = 2;

        public static void Postfix(ref float __result)
        {
            __result = __result / _modifier;
        }
    }
}
