using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace SRW
{
  public class SRWSettings : ModSettings
  {
    public static List<StuffWeight> stuffDefs;

    public override void ExposeData()
    {
      base.ExposeData();
      Scribe_Collections.Look(ref stuffDefs, "stuffDefs", LookMode.Value);
    }
  }

  public class SRWMod : Mod
  {
    SRWSettings settings;
    public SRWMod(ModContentPack con) : base(con)
    {
      this.settings = GetSettings<SRWSettings>();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
      Rect stuffPanel = new Rect(inRect.width / 2 - (inRect.width / 12), inRect.y, inRect.width / 6, inRect.height);

      Listing_Standard listing = new Listing_Standard();
      listing.Begin(inRect);

      for (int i = 0; i < SRWSettings.stuffDefs.Count(); i++)
      {
        float mult = SRWSettings.stuffDefs[i].StuffMultiplier;
        string label = SRWSettings.stuffDefs[i].StuffDef.label;

        listing.Begin(stuffPanel);
        listing.Settings_SliderLabeled("{0}".Formatted(label), "%", ref mult, 0f, 1f);
        listing.End();
        SRWSettings.stuffDefs[i] = new StuffWeight()
        {
          StuffDef = SRWSettings.stuffDefs[i].StuffDef,
          OriginalWeight = SRWSettings.stuffDefs[i].OriginalWeight,
          StuffMultiplier = mult
        };
      }

      listing.End();

      base.DoSettingsWindowContents(inRect);
    }

    public override void WriteSettings()
    {
      UpdateChanges();

      base.WriteSettings();
    }

    public override string SettingsCategory()
    {
      return "MenuTitle".Translate();
    }

    public static void UpdateChanges()
    {
      // *My own references, ignore*
      // DefDatabase<HediffDef>.GetNamed("SmokeleafHigh").stages[0].capMods[0].offset = LLLModSettings.amountPenaltyConsciousness;
      // HediffDef.Named("SmokeleafHigh").stages.Where((HediffStage stage) => stage.capMods.Any((PawnCapacityModifier mod) => mod.capacity == PawnCapacityDefOf.Consciousness)).First().capMods.Where((PawnCapacityModifier mod) => mod.capacity == PawnCapacityDefOf.Consciousness).First().offset = RSModSettings.amountCramped;
      // ThingDef.Named("NEC_ReinforcedWall").statBases.Where((StatModifier statBase) => statBase.stat == StatDefOf.MaxHitPoints).First().value = RWModSettings.WallHitPoints;
      // SRWSettings.ReinforcedWall.statBases.Where((StatModifier statBase) => statBase.stat == StatDefOf.MaxHitPoints).First().value = SRWSettings.WallHitPoints;
      foreach (StuffWeight item in SRWSettings.stuffDefs)
      {
        float newWeight = item.OriginalWeight * item.StuffMultiplier;
        item.StuffDef.statBases.Where((StatModifier statBase) => statBase.stat == StatDefOf.Mass).First().value = newWeight;
      }
    }

    private static float RoundToNearestHundredth(float val)
    {
      return (float)Math.Round(val * 100, MidpointRounding.AwayFromZero) / 100;
    }
  }
}
