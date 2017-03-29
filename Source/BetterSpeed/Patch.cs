using Harmony;
using HugsLib.Source.Detour;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using UnityEngine;
using Verse;

namespace BetterSpeed
{
    [HarmonyPatch(typeof(UI_BackgroundMain))]
    [HarmonyPatch(nameof(UI_BackgroundMain.BackgroundOnGUI))]
    [StaticConstructorOnStartup]
    public class Patch
    {
        public static readonly Texture2D BGPlanet = ContentFinder<Texture2D>.Get("NewBackground", true);

        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> original)
        {
            var oldBg = typeof(UI_BackgroundMain).GetField("BGPlanet", BindingFlags.NonPublic | BindingFlags.Static);
            var newBg = typeof(Patch).GetField("BGPlanet", BindingFlags.Public | BindingFlags.Static);

            foreach(var op in original)
            {
                if (op.operand == oldBg)
                {
                    op.operand = newBg;
                }
                yield return op;
            }
            
        }
    }
}
