using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Verse;

namespace BetterSpeed
{
    [HarmonyPatch(typeof(Verb))]
    [HarmonyPatch(nameof(Verb.TryStartCastOn))]
    public class WarmupPatch
    {
        public static float _modifier = 1 / 2;

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> source)
        {
            var field = AccessTools.Field(typeof(StatDefOf), nameof(StatDefOf.AimingDelayFactor));
            var modifierField = AccessTools.Field(typeof(WarmupPatch), nameof(WarmupPatch._modifier));
            return source.ReplaceMatchingSequence(
                new Func<CodeInstruction, bool>[]
                {
                    (o => o.operand == field),
                    (o => true),
                    (o => true)
                },
                i => i.Concat(
                    new[]
                    {
                        new CodeInstruction(OpCodes.Ldsfld, modifierField),
                        new CodeInstruction(OpCodes.Mul)
                    }
                )
            );
        }
    }
}
