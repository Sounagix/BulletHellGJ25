using UnityEngine;

public class HUDManager : Manager
{
    [SerializeField]
    Material _lifeShaderMaterial;

    private PlayerManager _playerManager;
    public PlayerManager Player { set { _playerManager = value; } }

    private const string LIFE_SHADER_PARAM = "_LifePoints";
    private const string MAX_LIFE_SHADER_PARAM = "_MaxLifePoints";

    public override void Initialize()
    {
        if (_playerManager && _lifeShaderMaterial && _lifeShaderMaterial.HasProperty(LIFE_SHADER_PARAM)
            && _lifeShaderMaterial.HasProperty(MAX_LIFE_SHADER_PARAM)) 
        {
            _lifeShaderMaterial.SetFloat(LIFE_SHADER_PARAM, _playerManager.DamageableStats.MaxHP);
            _lifeShaderMaterial.SetFloat(MAX_LIFE_SHADER_PARAM, _playerManager.DamageableStats.MaxHP);
        }
    }

    public override void Shutdown()
    {
    }

    private void OnEnable()
    {
        PlayerManager.OnPlayerTakeDamage += OnUpdateLife;
    }

    private void OnDisable()
    {
        PlayerManager.OnPlayerTakeDamage -= OnUpdateLife;
    }

    private void OnUpdateLife() 
    {
        if (!_lifeShaderMaterial || !_lifeShaderMaterial.HasProperty(LIFE_SHADER_PARAM) ||
            !_playerManager || _playerManager.DamageableStats.CurrentHP < 0)
            return;

        _lifeShaderMaterial.SetFloat(LIFE_SHADER_PARAM, _playerManager.DamageableStats.CurrentHP);
    }
}
