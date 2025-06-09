using Mirror;

public abstract class RewardServiceBase : NetworkBehaviour
{
    [TargetRpc]
    public virtual void RewardPlayer(NetworkConnectionToClient conn) { }
}