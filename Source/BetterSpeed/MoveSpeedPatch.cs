using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace BetterSpeed
{
    [HarmonyPatch(typeof(Pawn))]
    [HarmonyPatch("TicksPerMove")]
    public class MoveSpeedPatch
    {
        public static float _modifier = 3;
        public static float _carrymodifier = 2;

        [HarmonyPostfix]
        public static void TicksPerMove(bool diagonal, Pawn __instance, ref int __result)
        {
            var c = (float)__result;

            // undo carry pawn modifier
            if (__instance.carryTracker?.CarriedThing?.def.category == ThingCategory.Pawn)
                c /= 100f;

            // add our own modifiers

            var carriedThing = __instance.carryTracker.CarriedThing;
            var carryingMass = 1f;
            if (carriedThing != null)
            {
                carryingMass = carriedThing.GetInnerIfMinified().GetStatValue(StatDefOf.Mass, false);
            }

            var carryingCount = (float)__instance.carryTracker.GetInnerContainer().TotalStackCount;


            var massPercent = (MassUtility.GearAndInventoryMass(__instance) + carryingMass * carryingCount) / MassUtility.Capacity(__instance);

            c *= (1 + massPercent) * _carrymodifier;

            __result = Mathf.Clamp(Mathf.RoundToInt(c / _modifier), 1, 450);
        }
    }
}
