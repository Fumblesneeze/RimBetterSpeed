using Harmony;
using HugsLib;
using HugsLib.Settings;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Verse;

namespace BetterSpeed
{
    public class Mod : ModBase
    {
        public override string ModIdentifier { get; } = "BetterSpeed";

        public override void Initialize()
        {
            var harmony = HarmonyInstance.Create("Fumblesneeze.BetterSpeed");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        private SettingHandle<float> moveSpeed;
        private SettingHandle<float> doorSpeed;
        private SettingHandle<float> cooldownSpeed;
        private SettingHandle<float> projectileSpeed;
        private SettingHandle<float> carrySpeedPenalty;

        public override void DefsLoaded()
        {
            moveSpeed = Settings.GetHandle("moveSpeed", "moveSpeed_title".Translate(), "moveSpeed_desc".Translate(), 3f);
            doorSpeed = Settings.GetHandle("doorSpeed", "doorSpeed_title".Translate(), "doorSpeed_desc".Translate(), 2f);
            projectileSpeed = Settings.GetHandle("projectileSpeed", "projectileSpeed_title".Translate(), "projectileSpeed_desc".Translate(), 2f);
            cooldownSpeed = Settings.GetHandle("cooldownSpeed", "cooldownSpeed_title".Translate(), "cooldownSpeed_desc".Translate(), 2f);
            carrySpeedPenalty = Settings.GetHandle("carrySpeedPenalty", "carrySpeedPenalty_title".Translate(), "carrySpeedPenalty_desc".Translate(), 2f);

            SettingsChanged();
        }

        public override void SettingsChanged()
        {
            MoveSpeedPatch._modifier = moveSpeed;
            MoveSpeedPatch._carrymodifier = carrySpeedPenalty;
            DoorPatch._modifier = doorSpeed;
            ProjectilePatch._modifier = projectileSpeed;
            CooldownPatch._modifier = cooldownSpeed;
            WarmupPatch._modifier = 1 / cooldownSpeed;
            BurstShotPatch._modifier = 1 / cooldownSpeed;
        }
    }
}
