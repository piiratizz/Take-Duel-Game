using Mirror;
using UnityEngine;
using Zenject;

public class BalanceRewardService : RewardServiceBase
{
    private decimal _moneyReward = 100m;
    [Inject] private PlayerBankService _playerBankService;
    
    [TargetRpc]
    public override void RewardPlayer(NetworkConnectionToClient conn)
    {
        _playerBankService.Add(_moneyReward);
    }
}