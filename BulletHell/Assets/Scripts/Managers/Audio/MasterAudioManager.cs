using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public enum PLAYER_SOUNDS
{
    DASH,
    SHOOT,
    TAKE_DAMAGE,
    HEAL,
    WALK,
    TAKE_FOOD,
}

public enum CLIENT_SOUND : int
{
    NEW_CLIENT,
    CORRECT_DELIVERY,
    INCORRECT_DELIVERY,

}

public enum THROWEABLE_SOUND : int
{
    THROW,
    BOUNCE,
    DAMAGE,
    PICK,
    POP,
}

public class MasterAudioManager : MonoBehaviour
{
    public static MasterAudioManager Instance;

    [Header("Emitter Settings")]
    public int _poolSize;
    public GameObject _emitterPrefab;

    private List<FMODAudioEmitter> _emitters = new();
    private int _currentIndex = 0;

    [Header("Player´s sounds")]
    [SerializeField]
    private EventReference _dashSound, _takeDamage;

    [Header("Customer´s sounds")]

    [SerializeField]
    private EventReference _newClientSound, _correctDeliverySound, _incorrectDeliverySound;

    [Header("Throweable´s sounds")]

    [SerializeField]
    private EventReference _bounceSound, _throw, _pick, _pop;

    [Header("OST")]
    [SerializeField]
    private EventReference _ostSong, _mainMenuSong, _gameOverSong;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeEmitters();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeEmitters()
    {
        _emitters = new List<FMODAudioEmitter>();

        for (int i = 0; i < _poolSize; i++)
        {
            GameObject emitterObj = Instantiate(_emitterPrefab, transform);
            var emitter = emitterObj.GetComponent<FMODAudioEmitter>();
            if (emitter == null)
                emitter = emitterObj.AddComponent<FMODAudioEmitter>();

            _emitters.Add(emitter);
        }
    }

    private FMODAudioEmitter GetNextAvailableEmitter()
    {
        var emitter = _emitters[_currentIndex];
        _currentIndex = (_currentIndex + 1) % _emitters.Count;
        return emitter;
    }

    public void PlayOneShot(CLIENT_SOUND cLIENT_SOUND, Transform target)
    {
        FMODAudioEmitter emitter = GetNextAvailableEmitter();
        EventReference eventReference = new EventReference();
        switch (cLIENT_SOUND)
        {
            case CLIENT_SOUND.NEW_CLIENT:
                eventReference = _newClientSound;
                break;
            case CLIENT_SOUND.CORRECT_DELIVERY:
                eventReference = _correctDeliverySound;
                break;
            case CLIENT_SOUND.INCORRECT_DELIVERY:
                eventReference = _incorrectDeliverySound;
                break;
        }
        emitter.Play(eventReference, target.position);
    }

    public void PlayOneShot(THROWEABLE_SOUND cLIENT_SOUND, Transform target)
    {
        FMODAudioEmitter emitter = GetNextAvailableEmitter();
        EventReference eventReference = new EventReference();
        switch (cLIENT_SOUND)
        {
            case THROWEABLE_SOUND.THROW:
                eventReference = _throw;
                break;
            case THROWEABLE_SOUND.BOUNCE:
                eventReference = _bounceSound;
                break;
            case THROWEABLE_SOUND.DAMAGE:
                break;
            case THROWEABLE_SOUND.PICK:
                eventReference = _pick;
                break;
            case THROWEABLE_SOUND.POP:
                eventReference = _pop;
                break;
        }
        emitter.Play(eventReference, target.position);
    }

    public void PlayOneShot(PLAYER_SOUNDS cLIENT_SOUND, Transform target)
    {
        FMODAudioEmitter emitter = GetNextAvailableEmitter();
        EventReference eventReference = new EventReference();
        switch (cLIENT_SOUND)
        {
            case PLAYER_SOUNDS.HEAL:
                break;
            case PLAYER_SOUNDS.WALK:
                break;
            case PLAYER_SOUNDS.TAKE_FOOD:
                break;
            case PLAYER_SOUNDS.DASH:
                eventReference = _dashSound;
                break;
            case PLAYER_SOUNDS.SHOOT:
                break;
            case PLAYER_SOUNDS.TAKE_DAMAGE:
                eventReference = _takeDamage;
                break;
        }
        emitter.Play(eventReference, target.position);
    }

    //public void PlayOneShotAttached(EventReference sound, GameObject target)
    //{
    //    FMODAudioEmitter emitter = GetNextAvailableEmitter();
    //    emitter.PlayAttached(sound, target);
    //}
}
