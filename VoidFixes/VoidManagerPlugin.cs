using VoidManager.MPModChecks;

namespace VoidFixes
{
    public class VoidManagerPlugin : VoidManager.VoidPlugin
    {
        public override MultiplayerType MPType => MultiplayerType.Client;

        public override string Author => MyPluginInfo.PLUGIN_AUTHORS;

        public override string Description => MyPluginInfo.PLUGIN_DESCRIPTION;

        public override string ThunderstoreID => MyPluginInfo.PLUGIN_THUNDERSTORE_ID;
    }
}
// Bug fix idea: fix fighters not carring about distance or obstacles when shooting players. - reqires removing current damage system and allowing fighter projectiles to damage player