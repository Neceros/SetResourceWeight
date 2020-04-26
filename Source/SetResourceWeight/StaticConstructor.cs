using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace SRW
{
  [StaticConstructorOnStartup]
  public static class StartUp
  {
    static StartUp()
    {
      // Loads right before main menu
      SetOriginalValues();

      SRWMod.UpdateChanges();
    }

    public static void SetOriginalValues()
    {
      foreach (ThingDef item in DefDatabase<ThingDef>.AllDefs)
      {
        if (item.IsStuff)
        {
          StuffWeight tempStuff = new StuffWeight
          {
            StuffDef = item,
            OriginalWeight = item.BaseMass,
            StuffMultiplier = 1f
          };
          SRWSettings.stuffDefs.Add(tempStuff);
        }
      }
    }
  }
}
