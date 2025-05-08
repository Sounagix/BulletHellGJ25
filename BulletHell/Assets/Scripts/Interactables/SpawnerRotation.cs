using UnityEngine;

public class SpawnerRotation : MonoBehaviour
{
    [SerializeField]
    private float _spinSpeed;


    void Update()
    {
        transform.Rotate(Vector3.forward, _spinSpeed * Time.deltaTime);
    }
}
