﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LethalRed.ModArtifacts
{
    public class ConfigsForLethalMods
    {

        public static void WriteConfigForLethal(string lethalconfigdir)
        {
            WriteConfigForTestAccount666ShipWindows(lethalconfigdir);
        }

        public static void WriteConfigForTestAccount666ShipWindows(string lethalconfigdir) 
        {
            string configText = @"## Settings file was created by plugin ShipWindows v1.5.1
## Plugin GUID: TestAccount666.ShipWindows

[Fixes]

## If set to true, will add a check to enemy's ai to prevent them from killing you through the windows. Enabling this might cause some issues though.
# Setting type: Boolean
# Default value: true
Enable Enemy Fix = true

[General]

## Enable this to preserve vanilla network compatability. This will disable unlockables and the shutter toggle switch. (default = false)
# Setting type: Boolean
# Default value: false
VanillaMode = false

## Defines what material will be used for the glass. Iridescence will give you some nice rainbow colors. They are more visible with Refraction, but Refraction breaks some VFX.
# Setting type: WindowMaterial
# Default value: NO_REFRACTION_IRIDESCENCE
# Acceptable values: NO_REFRACTION, NO_REFRACTION_IRIDESCENCE, REFRACTION, REFRACTION_IRIDESCENCE
WindowMaterial = NO_REFRACTION_IRIDESCENCE

## Enable the window shutter to hide transitions between space and the current moon. (default = true)
# Setting type: Boolean
# Default value: true
EnableWindowShutter = true

## Should the planet and moon outside the ship be hidden?
# Setting type: Boolean
# Default value: false
HideSpaceProps = false

## Set this value to control how the outside space looks.
# Setting type: SpaceOutside
# Default value: SPACE_HDRI
# Acceptable values: OTHER_MODS, SPACE_HDRI, BLACK_AND_STARS
SpaceOutside = SPACE_HDRI

## Disable the flood lights added under the ship if you have the floor window enabled.
# Setting type: Boolean
# Default value: false
DisableUnderLights = false

## Don't move the poster that blocks the second window if set to true.
# Setting type: Boolean
# Default value: false
DontMovePosters = false

## Sets the rotation speed of the space skybox for visual effect. Requires 'SpaceOutside' to be set to 1 or 2.
# Setting type: Single
# Default value: 0.1
# Acceptable value range: From -1 to 1
RotateSpaceSkybox = 0.1

## OBSOLETE: Download [Ship Windows 4K Skybox] from the Thunderstore to enable!
# Setting type: Int32
# Default value: 0
SkyboxResolution = 0

## Adds the windows to the terminal as ship upgrades. Set this to false and use below settings to have them enabled by default.
# Setting type: Boolean
# Default value: true
WindowsUnlockable = true

## The base cost of the window behind the terminal / right of the switch.
# Setting type: Int32
# Default value: 60
Window1Cost = 60

## The base cost of the window across from the terminal / left of the switch.
# Setting type: Int32
# Default value: 60
Window2Cost = 60

## The base cost of the large floor window.
# Setting type: Int32
# Default value: 100
Window3Cost = 100

## The base cost of the door windows.
# Setting type: Int32
# Default value: 75
Window4Cost = 75

## Enable the window to the right of the switch, behind the terminal.
# Setting type: Boolean
# Default value: true
EnableWindow1 = true

## Enable the window to the left of the switch, across from the first window.
# Setting type: Boolean
# Default value: true
EnableWindow2 = true

## Enable the large floor window.
# Setting type: Boolean
# Default value: true
EnableWindow3 = true

## Enable the door windows.
# Setting type: Boolean
# Default value: true
EnableWindow4 = true

## If set as unlockable, start the game with window to the right of the switch unlocked already.
# Setting type: Boolean
# Default value: true
UnlockWindow1 = true

## If set as unlockable, start the game with window across from the terminal unlocked already.
# Setting type: Boolean
# Default value: false
UnlockWindow2 = false

## If set as unlockable, start the game with the floor window unlocked already.
# Setting type: Boolean
# Default value: false
UnlockWindow3 = false

## If set as unlockable, start the game with the door windows unlocked already.
# Setting type: Boolean
# Default value: false
UnlockWindow4 = false

[Misc]

## If set to true, will close the window shutters when routing to a new moon.Disabling this will look weird, if CelestialTint isn't installed.
# Setting type: Boolean
# Default value: true
Shutters hide moon transitions = true

## If set to true, will change the tool tip for the light switch to match the shutter's tool tip.
# Setting type: Boolean
# Default value: true
Change light switch tool tip = true

## If set to true, will load and use Wesley's voice lines for opening/closing the window shutters.
# Setting type: Boolean
# Default value: true
Enable Wesley shutter voice lines = false

## If set to true, will play the voice lines, if opening/closing the window shutters is caused by a transition.
# Setting type: Boolean
# Default value: true
Play Wesley shutter voice lines on transitions = false

## If set to true, will enable the scan node for the shutter switch.
# Setting type: Boolean
# Default value: true
Enable Shutter Switch scan node = true

[Other Mods]

## If Celestial Tint is installed, override the skybox. Only effective if skybox is set to Space HDRRI Volume.
# Setting type: Boolean
# Default value: false
CelestialTintOverrideSpace = false

";
            string file = Path.Combine(lethalconfigdir, "TestAccount666.ShipWindows.cfg");
            File.WriteAllText(file, configText);
        }
    }
}
