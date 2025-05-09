using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class FMODAudioEmitter : MonoBehaviour
{
    private EventInstance instance;

    public void Play(EventReference sound, Vector3 position)
    {
        instance = RuntimeManager.CreateInstance(sound);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        instance.start();
        instance.release();
    }

    //public void PlayAttached(EventReference sound, GameObject target)
    //{
    //    instance = RuntimeManager.CreateInstance(sound);
    //    RuntimeManager.AttachInstanceToGameObject(instance, target.transform, target.GetComponent<Rigidbody>());
    //    instance.start();
    //    instance.release();
    //}

}
