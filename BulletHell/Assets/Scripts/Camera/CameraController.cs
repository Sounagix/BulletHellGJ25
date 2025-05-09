using UnityEngine;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float defaultSize;

    [SerializeField]
    private Resolution defaultResolution;

    [SerializeField]
    private Volume postProcessVolume;

    [SerializeField]
    private GameObject leftWall, rightWall, topWall, bottomWall;

    private Camera cam;
    private int lastScreenWidth, lastScreenHeight;

    public void Start()
    {
        cam = Camera.main;

        AdjustCameraAndWalls();
    }

    void Update()
    {
        // If the resolution changes (e.g., window resize), update the camera
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
            AdjustCameraAndWalls();
        }
    }

    void AdjustCameraAndWalls()
    {
        // Calculate screen aspect ratio
        float screenAspect = (float)Screen.width / Screen.height;
        float baseAspect = defaultResolution.width / defaultResolution.height;

        // Adjust the camera's orthographic size based on the width
        cam.orthographicSize = defaultSize * (baseAspect / screenAspect);

        // Adjust the walls based on the camera's orthographic size
        float cameraHeight = cam.orthographicSize * 2;
        float cameraWidth = cameraHeight * screenAspect;

        // Move the walls based on the camera's visible width and height in world units
        //UpdateWallPositions(cameraWidth, cameraHeight);
    }

    void UpdateWallPositions(float cameraWidth, float cameraHeight)
    {
        // Place the walls at the correct positions based on the camera's size
        leftWall.transform.position = new Vector3(-cameraWidth / 2, 0, 0);
        rightWall.transform.position = new Vector3(cameraWidth / 2, 0, 0);
        topWall.transform.position = new Vector3(0, cameraHeight / 2, 0);
        bottomWall.transform.position = new Vector3(0, -cameraHeight / 2, 0);
    }
}