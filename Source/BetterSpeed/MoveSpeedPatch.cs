using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace BetterSpeed
{
    //[HarmonyPatch(typeof(Pawn))]
    //[HarmonyPatch("TicksPerMove")]
    public class MoveSpeedPatch
    {
        public static float _modifier = 3;
        public static float _carrymodifier = 2;

        //[HarmonyPostfix]
        public static void TicksPerMove(bool diagonal, Pawn __instance, ref int __result)
        {
            if (_carrymodifier == 0f)
                return;
            if (!MassUtility.CanEverCarryAnything(__instance))
            {
                __result = Mathf.Clamp(Mathf.RoundToInt(__result / _modifier), 1, 450);
                return;
            }
            var c = (float)__result;
            var pawnMass = __instance.GetStatValue(StatDefOf.Mass);
            var ignoredMass = pawnMass * (__instance.RaceProps.packAnimal ? 0.5f : 0.2f);

            if (Compatibility_GiddyUp.GetMount(__instance) is Pawn mount)
            {
                __result = diagonal ? mount.TicksPerMoveDiagonal : mount.TicksPerMoveCardinal;
                return;
            }

            // add our own modifiers
            var mass = ignoredMass - MassUtility.GearAndInventoryMass(__instance);

            if (Compatibility_GiddyUp.GetRider(__instance) is Pawn rider)
            {
                mass += MassUtility.GearAndInventoryMass(rider);

                mass += rider.GetStatValue(StatDefOf.Mass, true);
                var riderCarriedThing = rider.carryTracker.CarriedThing;
                if (riderCarriedThing != null)
                {
                    mass += riderCarriedThing.stackCount * riderCarriedThing.GetStatValue(StatDefOf.Mass);
                    if (riderCarriedThing is Pawn p)
                    {
                        mass += MassUtility.GearAndInventoryMass(p);

                        // undo carry pawn modifier
                        c /= 1.666f;
                    }
                }
            }


            var capacity = MassUtility.Capacity(__instance, null);

            var pawnCarriedThing = __instance.carryTracker.CarriedThing;
            if (pawnCarriedThing != null)
            {
                mass += pawnCarriedThing.stackCount * pawnCarriedThing.GetStatValue(StatDefOf.Mass);
                if (pawnCarriedThing is Pawn p)
                {
                    mass += MassUtility.GearAndInventoryMass(p);

                    // undo carry pawn modifier
                    c /= 1.666f;
                }
            }

            var encumbrance = Mathf.Clamp(mass / capacity, 0f, 1f);

            var modifier = 1 + (encumbrance * _carrymodifier);

            c *= modifier;

            __result = Mathf.Clamp(Mathf.RoundToInt(c / _modifier), 1, 450);
        }
    }
}
