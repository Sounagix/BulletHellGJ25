using FMODUnity;
using UnityEngine;

/// <summary>
/// TODO: Replace this class by ServiceLocator instance that handles the sounds
/// </summary>
public class PlayerSFXController : MonoBehaviour
{
    [SerializeField]
    private StudioEventEmitter movementSFX;
    [SerializeField]
    private StudioEventEmitter playerDeathSFX;
    [SerializeField]
    private StudioEventEmitter spawnPlayerSFX;
    [SerializeField]
    private StudioEventEmitter heartBeatSFX;

    private bool isHeartBeatPlaying = false;
    private Coroutine movementFadeIn;
    private Coroutine movementFadeOut;

    private void OnEnable()
    {
        //GameSceneManager.OnGameOver += StopMovementSFX;
        //GameSceneManager.OnGamePaused += StopMovementSFX;
    }

    private void OnDisable()
    {
        //GameSceneManager.OnGameOver -= StopMovementSFX;
        //GameSceneManager.OnGamePaused -= StopMovementSFX;
    }

    public void PlayMovementSFX()
    {
        if (movementFadeIn != null)
            StopCoroutine(movementFadeIn);
        if (movementFadeOut != null)
            StopCoroutine(movementFadeOut);

        //movementFadeIn = StartCoroutine(AudioManager.MusicFadeIn(movementSFX, false, 0.5f));
    }

    public void StopMovementSFX()
    {
        if (movementFadeIn != null)
            StopCoroutine(movementFadeIn);
        if (movementFadeOut != null)
            StopCoroutine(movementFadeOut);

        //movementFadeOut = StartCoroutine(AudioManager.MusicFadeOut(movementSFX, false, 0.5f));
    }

    public void PlayPlayerDeathSFX()
    {
        if (movementSFX)
            movementSFX.Stop();
        if (playerDeathSFX)
            playerDeathSFX.Play();
    }

    public void PlaySpawnPlayer()
    {
        if (spawnPlayerSFX)
            spawnPlayerSFX.Play();
    }

    public void PlayHeartBeat()
    {
        if (isHeartBeatPlaying)
            return;

        isHeartBeatPlaying = true;

        if (heartBeatSFX)
            heartBeatSFX.Play();
    }

    public void StopHeartBeat()
    {
        if (!isHeartBeatPlaying)
            return;

        isHeartBeatPlaying = false;

        if (heartBeatSFX)
            heartBeatSFX.Stop();
    }
}