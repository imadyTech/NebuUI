using imady.NebuEvent;

namespace imady.NebuUI
{
    public class NebulogServerInitiateMsg : NebuUnityButtonInput, INebuInput
    {
        public NebulogServerInitiateMsg() { msg = "InitNebulogServerMsg"; }
    }
    public class NebulogServerShutdownMsg : NebuUnityButtonInput, INebuInput
    {
        public NebulogServerShutdownMsg() { msg = "ShutdownNebulogServerMsg"; }
    }
}
