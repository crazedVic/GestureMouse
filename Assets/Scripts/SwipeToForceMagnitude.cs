using UnityEngine;

public class SwipeToForceMagnitude : MonoBehaviour
{
    public Rigidbody rigidbodyComponent;
    public Camera mainCamera;
    public float forceMultiplier = 10f;  // Adjust this multiplier to scale the force appropriately
    public float sensitivity = 1.0f;     // Sensitivity multiplier to fine-tune the response to swipe speed

    private Vector2 startPosition;
    private float startTime;
    private bool swipeInProgress = false;

    void Awake()
    {
        // Set camera and rigidbody for top-down view
        if (mainCamera != null)
        {
            mainCamera.orthographic = true;
            mainCamera.transform.position = new Vector3(0, 10, 0);
            mainCamera.transform.rotation = Quaternion.Euler(90, 0, 0);
        }
        else
        {
            Debug.LogError("Main camera reference not set in the inspector.");
        }

        if (rigidbodyComponent != null)
        {
            rigidbodyComponent.transform.position = Vector3.zero;
            rigidbodyComponent.transform.rotation = Quaternion.identity;
        }
        else
        {
            Debug.LogError("Rigidbody component reference not set in the inspector.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Start of swipe
        {
            startPosition = Input.mousePosition;
            startTime = Time.time;
            swipeInProgress = true;
            Debug.Log("Swipe started at: " + startPosition);
        }

        if (Input.GetMouseButtonUp(0) && swipeInProgress) // End of swipe
        {
            Vector2 endPosition = Input.mousePosition;
            float endTime = Time.time;
            Vector2 swipeVector = endPosition - startPosition;
            float swipeDuration = endTime - startTime;
            Debug.Log("Swipe ended at: " + endPosition + ", Swipe vector: " + swipeVector);
            ProcessSwipe(swipeVector, swipeDuration);
            swipeInProgress = false;
        }
    }

    void ProcessSwipe(Vector2 swipeVector, float duration)
    {
        // Compute the swipe vector in world space
        Vector3 worldSwipeStart = mainCamera.ScreenToWorldPoint(new Vector3(startPosition.x, startPosition.y, mainCamera.nearClipPlane));
        Vector3 worldSwipeEnd = mainCamera.ScreenToWorldPoint(new Vector3(startPosition.x + swipeVector.x, startPosition.y + swipeVector.y, mainCamera.nearClipPlane));

        Vector3 swipeDirection = new Vector3(worldSwipeEnd.x - worldSwipeStart.x, 0, worldSwipeEnd.z - worldSwipeStart.z);

        if (swipeDirection.magnitude > 0.01f)
        {
            swipeDirection.Normalize();
        }

        float speed = swipeVector.magnitude / duration; // Speed of the swipe
        float forceMagnitude = speed * forceMultiplier * sensitivity; // Calculate force based on swipe speed, multiplier, and sensitivity

        Debug.Log($"Swipe duration: {duration}s, Speed: {speed}, Force to apply: {forceMagnitude}");
        Debug.Log($"World swipe start: {worldSwipeStart}, World swipe end: {worldSwipeEnd}");

        rigidbodyComponent.AddForce(swipeDirection * forceMagnitude);
    }
}
