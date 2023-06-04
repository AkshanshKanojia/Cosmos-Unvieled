using AkshanshKanojia.Inputs.Mobile;
using UnityEngine;

public class TopDownCameraController : MobileInputs
{
    // Camera movement parameters
    private const float CameraMoveFactor = 0.01f;
    [SerializeField] private float cameraMoveSpeed = 2f;
    [SerializeField] private float cameraMaxZoom = 60f;
    [SerializeField] private float cameraMinZoom = 30f;
    [SerializeField] private float cameraDragSensitivity = 3f;
    [SerializeField] float cameraZoomSpeed =2f;

    // Camera bounds
    [SerializeField] private Transform minCamBounds;
    [SerializeField] private Transform maxCamBounds;

    // Temporary touch position for drag calculations
    private Vector3 tempTouchPos;

    public override void Start()
    {
        base.Start();
        if (minCamBounds == null || maxCamBounds == null)
        {
            Debug.LogWarning("Missing bounds found for camera clamping, Ignoring clamp for camera: " + gameObject.name);
        }
    }

    /// <summary>
    /// Moves camera when dargged on screen and not pinching (ie using 2 taps together)
    /// </summary>
    /// <param name="_data"></param>
    private void MoveCamera(MobileInputManager.TouchData _data)
    {
        // Exclude camera pinching when two fingers are tapped
#if !UNITY_EDITOR
        if (Input.touchCount == 2)
            return;
#endif

        // Calculate drag distance and move the camera accordingly
        if (Vector2.Distance(tempTouchPos, _data.TouchPosition) > cameraDragSensitivity)
        {
            Vector3 changedPos = tempTouchPos - _data.TouchPosition;
            transform.position += CameraMoveFactor * cameraMoveSpeed * new Vector3(changedPos.y, 0f, -changedPos.x);
            tempTouchPos = _data.TouchPosition;

            // Check if minCamBounds and maxCamBounds are assigned
            if (minCamBounds != null && maxCamBounds != null)
            {
                // Clamp camera position within the specified bounds
                Vector3 cameraPos = transform.position;
                cameraPos.x = Mathf.Clamp(cameraPos.x, minCamBounds.position.x, maxCamBounds.position.x);
                cameraPos.z = Mathf.Clamp(cameraPos.z, minCamBounds.position.z, maxCamBounds.position.z);
                transform.position = cameraPos;
            }
        }
    }

    private void ZoomCamera(MobileInputManager.PinchData _pinchData)
    {
        var _tempLength = Camera.main.focalLength;
        _tempLength += _pinchData.NormalizedDirection * cameraZoomSpeed * Time.deltaTime;
        _tempLength = Mathf.Clamp(_tempLength, cameraMinZoom, cameraMaxZoom);
        Camera.main.focalLength = _tempLength;
    }
    #region Inputs
    public override void OnTapEnd(MobileInputManager.TouchData _data)
    {
        // TODO: Add implementation for tap end behavior, if needed
    }

    public override void OnTapMove(MobileInputManager.TouchData _data)
    {
        MoveCamera(_data);
    }

    public override void OnTapped(MobileInputManager.TouchData _data)
    {
        // Store the initial touch position
        tempTouchPos = _data.TouchPosition;
    }

    public override void OnTapStay(MobileInputManager.TouchData _data)
    {
        // TODO: Add implementation for tap stay behavior, if needed
    }

    public override void OnPinchBegin(MobileInputManager.PinchData _pinchData)
    {
    }

    public override void OnPinchMove(MobileInputManager.PinchData _pinchData)
    {
        ZoomCamera(_pinchData);
    }

    public override void OnPinchEnd(MobileInputManager.PinchData _pinchData)
    {
    }
    #endregion
}