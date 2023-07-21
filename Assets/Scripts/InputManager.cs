using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Singleton input manager for new input system.
/// Manages touch input.
/// </summary>
[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{

    #region Events
        public delegate void StartTouch(Vector2 position, float time);
        public event StartTouch OnStartTouch;
        public delegate void EndTouch(Vector2 position, float time);
        public event EndTouch OnEndTouch;
    #endregion

    private PlayerControls playerControls;
    private Camera mainCamera;

    private void Awake() {
        playerControls = new PlayerControls();
        mainCamera = Camera.main;
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
    }

    void Start()
    {
        // Subscribe to input action touch events.
        playerControls.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx);
        playerControls.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
    }

    /// <summary>
    /// Called when the touch action has started.
    /// </summary>
    /// <param name="context">Information regarding the event, you can read the value of the touch.</param>
    private void StartTouchPrimary(InputAction.CallbackContext context) {
        if (OnStartTouch != null) OnStartTouch(Utils.ScreenToWorld(mainCamera, playerControls.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)context.startTime);
    }

    /// <summary>
    /// Called when the touch action has ended.
    /// </summary>
    /// <param name="context">Information regarding the event, you can read the value of the touch.</param>
    private void EndTouchPrimary(InputAction.CallbackContext context) {
        if (OnEndTouch != null) OnEndTouch(Utils.ScreenToWorld(mainCamera, playerControls.Touch.PrimaryPosition.ReadValue<Vector2>()), (float)context.time);
    }

    /// <summary>
    /// Get current position of primary touch.
    /// </summary>
    /// <returns>Vector2 touch position in world coordinates.</returns>
    public Vector2 PrimaryPosition() {
        return Utils.ScreenToWorld(mainCamera, playerControls.Touch.PrimaryPosition.ReadValue<Vector2>());
    }

}
