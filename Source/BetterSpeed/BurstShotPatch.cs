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
    [HarmonyPatch("TryCastNextBurstShot")]
    public class BurstShotPatch
    {
        public static float _modifier = 1 / 2;

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> source)
        {
            var field = AccessTools.Field(typeof(VerbProperties), nameof(VerbProperties.ticksBetweenBurstShots));
            var modifierField = AccessTools.Field(typeof(BurstShotPatch), nameof(BurstShotPatch._modifier));
            return source.ReplaceMatchingSequence(
                new Func<CodeInstruction, bool>[]
                {
                    (o => o.operand == field)
                },
                i => i.Concat(
                    new[]
                    {
                        new CodeInstruction(OpCodes.Conv_R4),
                        new CodeInstruction(OpCodes.Ldsfld, modifierField),
                        new CodeInstruction(OpCodes.Mul),
                        new CodeInstruction(OpCodes.Conv_I)
                    }
                )
            );
        }
    }
}
