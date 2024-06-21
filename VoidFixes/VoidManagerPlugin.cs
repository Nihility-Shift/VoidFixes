using VoidManager.MPModChecks;

namespace VoidFixes
{
    public class VoidManagerPlugin : VoidManager.VoidPlugin
    {
        public override MultiplayerType MPType => MultiplayerType.Client;

        public override string Author => "Dragon";

        public override string Description => "Fixes or provides the ability to fix bugs. Client-Side";
    }
}
// Bug fix idea: fix fighters not carring about distance or obstacles when shooting players. - reqires removing current damage system and 
//
// Supply drops don't delete when leaving sector
// Spawner Prefabs don't delete when leaving sector
//
//
//
//
//
//
//
//