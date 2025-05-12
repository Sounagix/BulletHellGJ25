using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class FMODAudioEmitter : MonoBehaviour
{
    private EventInstance instance;

    public void Play(EventReference sound, Vector3 position, float volumen)
    {
        instance = RuntimeManager.CreateInstance(sound);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        instance.setVolume(volumen);
        instance.start();
        instance.release();
    }

    public void Stop()
    {
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
    }

    private void OnDestroy()
    {
        Stop();
    }

    public bool IsPlaying()
    {
        instance.getPlaybackState(out PLAYBACK_STATE state);
        return state == PLAYBACK_STATE.PLAYING;
    }

    public EventInstance GetEventReference()
    {
        return instance;
    }


    //public void PlayAttached(EventReference sound, GameObject target)
    //{
    //    instance = RuntimeManager.CreateInstance(sound);
    //    RuntimeManager.AttachInstanceToGameObject(instance, target.transform, target.GetComponent<Rigidbody>());
    //    instance.start();
    //    instance.release();
    //}

}
