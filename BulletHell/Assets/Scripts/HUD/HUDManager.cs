using UnityEngine;

public class HUDManager : Manager
{
    [SerializeField]
    private InventoryHUD _inventoryHUD;

    [SerializeField]
    private LifeHUD _lifeHUD;

    [SerializeField]
    private TimeHUD _timeHUD;

    private PlayerManager _player;
    public PlayerManager Player { set { _player = value; } }

    public override void Initialize()
    {
        if (_isInitialized)
            return;

        _lifeHUD.SetUp(_player.DamageableStats.MaxHP);
        _inventoryHUD.SetUp();
        _timeHUD.SetUp();

        _isInitialized = true;
    }

    public override void Shutdown()
    {
    }

    public void OnReset()
    {
        _lifeHUD.OnReset(_player.DamageableStats.MaxHP);
        _inventoryHUD.OnReset();
        _timeHUD.OnReset();
    }
}
