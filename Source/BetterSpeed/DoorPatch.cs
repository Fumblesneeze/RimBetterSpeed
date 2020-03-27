using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BetterSpeed
{
    [HarmonyPatch(typeof(Building_Door))]
    [HarmonyPatch(nameof(Building_Door.TicksToOpenNow), MethodType.Getter)]
    public class DoorPatch
    {
        public static float _modifier = 2;
        
        public static void Postfix(ref int __result)
        {
            __result = Mathf.Clamp(Mathf.RoundToInt(__result / _modifier), 1, 450);
        }
    }
}
