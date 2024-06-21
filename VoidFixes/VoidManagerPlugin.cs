using VoidManager.MPModChecks;

namespace VoidFixes
{
    public class VoidManagerPlugin : VoidManager.VoidPlugin
    {
        public override MultiplayerType MPType => MultiplayerType.Client;

        public override string Author => "Dragon, 18107";

        public override string Description => "Provides bug fixes, Log Error eliminations, and Optimizations. Client/Host-Side";
    }
}
// Bug fix idea: fix fighters not carring about distance or obstacles when shooting players. - reqires removing current damage system and allowing fighter projectiles to damage player
//
//
//
//
//
//
//
//
//