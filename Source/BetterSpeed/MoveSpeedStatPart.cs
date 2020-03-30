using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace BetterSpeed
{
    public class MoveSpeedStatPart : StatPart
    {
        public static float _modifier = 3;
        public static float _carrymodifier = 2;

        public override string ExplanationPart(StatRequest req)
        {
            if (_modifier <= 0f)
                return string.Empty;

            var sb = new StringBuilder();
            if (req.Thing is Pawn pawn)
            {
                if (Compatibility_GiddyUp.GetMount(pawn) != null)
                {
                    return string.Empty;
                }

                sb.AppendLine($"Global speed factor: x{_modifier.ToStringByStyle(ToStringStyle.FloatMaxOne)}");

                if (!MassUtility.CanEverCarryAnything(pawn) || _carrymodifier <= 0f)
                {
                    return sb.ToString();
                }

                var pawnMass = pawn.GetStatValue(StatDefOf.Mass);
                var ignoredMass = pawnMass * (pawn.RaceProps.packAnimal ? 0.5f : 0.2f);
                // add our own modifiers
                var mass = MassUtility.GearAndInventoryMass(pawn);

                if (Compatibility_GiddyUp.GetRider(pawn) is Pawn rider)
                {
                    sb.AppendLine($"Mounted by {rider}, adding {rider.GetStatValue(StatDefOf.Mass, true)} to carried mass.");

                    mass += rider.GetStatValue(StatDefOf.Mass, true);
                    var riderCarriedThing = rider.carryTracker.CarriedThing;
                    if (riderCarriedThing != null)
                    {
                        mass += riderCarriedThing.stackCount * riderCarriedThing.GetStatValue(StatDefOf.Mass, true);
                        if (riderCarriedThing.def.category == ThingCategory.Pawn)
                        {
                            // undo carry pawn modifier
                            sb.AppendLine($"Carrying {riderCarriedThing}: Undoing hardcoded carrying penalty: x1.6)");
                        }
                    }
                }


                var capacity = pawn.GetStatValue(StatDefOf.CarryingCapacity, true);

                var pawnCarriedThing = pawn.carryTracker.CarriedThing;
                if (pawnCarriedThing != null)
                {
                    mass += pawnCarriedThing.stackCount * pawnCarriedThing.GetStatValue(StatDefOf.Mass, true);
                    if (pawnCarriedThing.def.category == ThingCategory.Pawn)
                    {
                        // undo carry pawn modifier
                        sb.AppendLine($"Carrying {pawnCarriedThing}: Undoing hardcoded carrying penalty: x1.6)");
                    }
                }

                
                var encumbrance = Mathf.Max(mass - ignoredMass, 0f) / capacity;

                var modifier = 1 / (1 + (encumbrance * _carrymodifier));

                sb.AppendLine($"Carrying {(int)mass} ({(int)ignoredMass} free) with capacity {(int)capacity} (encumbrance {encumbrance.ToStringByStyle(ToStringStyle.FloatMaxTwo)}) and global factor {_carrymodifier}: x{modifier.ToStringByStyle(ToStringStyle.PercentOne)}");


            }
            return sb.ToString();
        }

        public override void TransformValue(StatRequest req, ref float val)
        {
            if (req.Thing is Pawn pawn)
            {
                if (Compatibility_GiddyUp.GetMount(pawn) != null)
                {
                    return;
                }
                val = Mathf.Clamp(Mathf.RoundToInt(val * _modifier), 0.1f, 100f);

                if (!MassUtility.CanEverCarryAnything(pawn) || _carrymodifier <= 0f)
                {
                    return;
                }

                var pawnMass = pawn.GetStatValue(StatDefOf.Mass);
                var ignoredMass = pawnMass * (pawn.RaceProps.packAnimal ? 0.5f : 0.2f);

                // add our own modifiers
                var mass = MassUtility.GearAndInventoryMass(pawn);

                if (Compatibility_GiddyUp.GetRider(pawn) is Pawn rider)
                {
                    mass += rider.GetStatValue(StatDefOf.Mass, true);
                    var riderCarriedThing = rider.carryTracker.CarriedThing;
                    if (riderCarriedThing != null)
                    {
                        mass += riderCarriedThing.stackCount * riderCarriedThing.GetStatValue(StatDefOf.Mass, true);
                        if (riderCarriedThing.def.category == ThingCategory.Pawn)
                        {
                            // undo carry pawn modifier
                            val *= 1.666f;
                        }
                    }
                }


                var capacity = pawn.GetStatValue(StatDefOf.CarryingCapacity, true);

                var pawnCarriedThing = pawn.carryTracker.CarriedThing;
                if (pawnCarriedThing != null)
                {
                    mass += pawnCarriedThing.stackCount * pawnCarriedThing.GetStatValue(StatDefOf.Mass, true);
                    if (pawnCarriedThing.def.category == ThingCategory.Pawn)
                    {
                        // undo carry pawn modifier
                        val *= 1.666f;
                    }
                }

                var encumbrance = Mathf.Max(mass - ignoredMass, 0f) / capacity;

                var modifier = 1/(1 + (encumbrance * _carrymodifier));

                val *= modifier;


            }

        }
    }
}