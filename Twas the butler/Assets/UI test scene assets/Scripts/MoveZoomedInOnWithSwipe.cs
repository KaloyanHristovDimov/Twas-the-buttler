using UnityEngine;
using UnityEngine.InputSystem;

public class MoveZoomedInOnWithSwipe : MonoBehaviour

{
    [Header("Rotation")]
    public Vector3 rotationAxis = Vector3.up;   // axis to rotate around
    public float degreesPerPixel = 0.15f;      // drag sensitivity
    public float maxFlickDegrees = 120f;      // cap flick-induced rotation
    public float speedForMaxFlick = 2000f;     // pixels/sec where flick hits maxFlickDegrees

    [Header("Smoothing")]
    public float followSpeed = 12f; // higher = snappier

    [Header("Detection")]
    public float minSwipePixels = 25f; // ignore tiny moves
    public float maxSampleTime = 0.25f; // avoid stale deltas

    Quaternion targetRotation;

    // previous touch sample
    Vector2 lastPos;
    float lastTime;
    bool tracking;

    void OnEnable()
    {
        targetRotation = transform.rotation;
    }

    void Update()
    {
        var touch = Touchscreen.current != null ? Touchscreen.current.primaryTouch : null;
        if (touch == null) return;

        bool touched = touch.press.isPressed;

        if (touched)
        {
            Vector2 pos = touch.position.ReadValue();

            if (!tracking)
            {
                tracking = true;
                lastPos = pos;
                lastTime = Time.unscaledTime;
                return;
            }

            float now = Time.unscaledTime;
            float dt = now - lastTime;
            if (dt <= 0f || dt > maxSampleTime) // prevent crazy speed spikes / stale samples
            {
                lastPos = pos;
                lastTime = now;
                return;
            }

            Vector2 delta = pos - lastPos;
            float deltaPixels = delta.magnitude;
            float speedPxPerSec = deltaPixels / dt;

            // Direction (horizontal-only example): left/right
            float dir = Mathf.Sign(delta.x); // -1 left, +1 right

            // Optional: if you want to ignore diagonals when detecting swipe:
            // require abs(x) > abs(y) or use angle thresholds.

            // Decide whether this sample counts as a "meaningful" swipe
            if (deltaPixels >= minSwipePixels)
            {
                // Drag-driven rotation (feels like it tracks your finger)
                float dragDegrees = delta.x * degreesPerPixel; // using delta.x only

                // Optional flick boost using speed
                float flickT = Mathf.Clamp01(speedPxPerSec / speedForMaxFlick);
                float flickDegrees = dir * flickT * maxFlickDegrees;

                float degreesToApply = dragDegrees + flickDegrees;

                // Update target rotation
                targetRotation = Quaternion.AngleAxis(degreesToApply, rotationAxis) * targetRotation;
            }

            // Update tracking sample
            lastPos = pos;
            lastTime = now;
        }
        else
        {
            tracking = false;
        }

        // Smoothly follow target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, followSpeed * Time.deltaTime);
    }
}

