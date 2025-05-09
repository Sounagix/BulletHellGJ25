using UnityEngine;

public class LifeHUD : MonoBehaviour
{
    [SerializeField]
    Material _lifeShaderMaterial;

    private const string LIFE_SHADER_PARAM = "_LifePoints";
    private const string MAX_LIFE_SHADER_PARAM = "_MaxLifePoints";

    public void SetUp(float maxHP)
    {
        if (!_lifeShaderMaterial || !_lifeShaderMaterial.HasProperty(LIFE_SHADER_PARAM)
            || !_lifeShaderMaterial.HasProperty(MAX_LIFE_SHADER_PARAM))
            return;

        _lifeShaderMaterial.SetFloat(LIFE_SHADER_PARAM, maxHP);
        _lifeShaderMaterial.SetFloat(MAX_LIFE_SHADER_PARAM, maxHP);
    }

    private void OnEnable()
    {
        PlayerManager.OnPlayerTakeDamage += OnUpdateLife;
    }

    private void OnDisable()
    {
        PlayerManager.OnPlayerTakeDamage -= OnUpdateLife;
    }

    private void OnUpdateLife(float currentLife)
    {
        if (!_lifeShaderMaterial || !_lifeShaderMaterial.HasProperty(LIFE_SHADER_PARAM))
            return;

        _lifeShaderMaterial.SetFloat(LIFE_SHADER_PARAM, currentLife);
    }

    public void OnReset(float maxHP)
    {
        if (!_lifeShaderMaterial || !_lifeShaderMaterial.HasProperty(LIFE_SHADER_PARAM)
        || !_lifeShaderMaterial.HasProperty(MAX_LIFE_SHADER_PARAM))
            return;

        _lifeShaderMaterial.SetFloat(LIFE_SHADER_PARAM, maxHP);
        _lifeShaderMaterial.SetFloat(MAX_LIFE_SHADER_PARAM, maxHP);
    }
}
