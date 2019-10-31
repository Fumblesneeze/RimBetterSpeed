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


            // add our own modifiers
            var mass = MassUtility.GearAndInventoryMass(__instance);


            var thing = __instance.carryTracker.CarriedThing;
            if (thing != null)
            {
                mass += thing.stackCount * thing.GetStatValue(StatDefOf.Mass);
                if (thing is Pawn p)
                {
                    mass += MassUtility.GearAndInventoryMass(p);


                    // undo carry pawn modifier
                    if(_carrymodifier > 0f)
                        c *= 1.666f;
                }
            }

            var capacity = __instance.BodySize * MassUtility.MassCapacityPerBodySize;
            var encumbrance = mass / capacity;

            var modifier = 1 + (encumbrance * _carrymodifier);

            c *= modifier;

            __result = Mathf.Clamp(Mathf.RoundToInt(c / _modifier), 1, 450);
        }
    }
}
