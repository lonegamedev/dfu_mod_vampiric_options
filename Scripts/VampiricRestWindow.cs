using UnityEngine;
using System.Collections;

using DaggerfallWorkshop;
using DaggerfallWorkshop.Game.UserInterface;
using DaggerfallWorkshop.Game.UserInterfaceWindows;

namespace LGDmods
{
  //Overriding because currentRestMode and RestModes are protected!
  public class VampiricRestWindow : DaggerfallRestWindow
  {
    public VampiricRestWindow(IUserInterfaceManager uiManager, bool ignoreAllocatedBed = false)
    : base(uiManager)
    {}
    public bool IsResting() {
      switch (currentRestMode) {
        case RestModes.Selection:
        case RestModes.Loiter:
          return false;
      }
      return true;
    }
  }
}
