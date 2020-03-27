using HarmonyLib;
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
        public override string ModIdentifier { get; } = "Fumble.BetterSpeed";

        public override void Initialize()
        {
            var harmony = new Harmony("Fumble.BetterSpeed");
            harmony.PatchAll();
        }

        private SettingHandle<float> moveSpeed;
        private SettingHandle<float> doorSpeed;
        private SettingHandle<float> cooldownSpeed;
        private SettingHandle<float> projectileSpeed;
        private SettingHandle<float> carrySpeedPenalty;

        public override void DefsLoaded()
        {
            moveSpeed = Settings.GetHandle("moveSpeed", "moveSpeed_title".Translate(), "moveSpeed_desc".Translate(), 2f);
            doorSpeed = Settings.GetHandle("doorSpeed", "doorSpeed_title".Translate(), "doorSpeed_desc".Translate(), 2f);
            projectileSpeed = Settings.GetHandle("projectileSpeed", "projectileSpeed_title".Translate(), "projectileSpeed_desc".Translate(), 1.5f);
            cooldownSpeed = Settings.GetHandle("cooldownSpeed", "cooldownSpeed_title".Translate(), "cooldownSpeed_desc".Translate(), 1.5f);
            carrySpeedPenalty = Settings.GetHandle("carrySpeedPenalty", "carrySpeedPenalty_title".Translate(), "carrySpeedPenalty_desc".Translate(), 0.75f);

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
