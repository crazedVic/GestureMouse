using UnityEngine;

public class SwipeToForceBasic : MonoBehaviour
{
    public Rigidbody rigidbodyComponent;
    public Camera mainCamera;
    public float forceMagnitude = 100f;

    private Vector2 startPosition;
    private bool swipeInProgress = false;

    void Awake()
    {
        // Position and orient the camera for a top-down orthographic view
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

        // Reset rigidbody position and orientation
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
            swipeInProgress = true;
            Debug.Log("Swipe started at: " + startPosition);
        }

        if (Input.GetMouseButtonUp(0) && swipeInProgress) // End of swipe
        {
            Vector2 endPosition = Input.mousePosition;
            Vector2 swipeVector = endPosition - startPosition;
            Debug.Log("Swipe ended at: " + endPosition + ", Swipe vector: " + swipeVector);
            ProcessSwipe(swipeVector);
            swipeInProgress = false;
        }
    }

    void ProcessSwipe(Vector2 swipeVector)
    {
        // Compute the swipe vector in world space
        Vector3 worldSwipeStart = mainCamera.ScreenToWorldPoint(new Vector3(startPosition.x, startPosition.y, mainCamera.nearClipPlane));
        Vector3 worldSwipeEnd = mainCamera.ScreenToWorldPoint(new Vector3(startPosition.x + swipeVector.x, startPosition.y + swipeVector.y, mainCamera.nearClipPlane));

        Vector3 swipeDirection = new Vector3(worldSwipeEnd.x - worldSwipeStart.x, 0, worldSwipeEnd.z - worldSwipeStart.z);

        // Normalize the direction if needed, ensuring we do not normalize a very small magnitude vector
        if (swipeDirection.magnitude > 0.01f)
        {
            swipeDirection.Normalize();
        }

        Debug.Log($"World swipe start: {worldSwipeStart}, World swipe end: {worldSwipeEnd}");
        Debug.Log($"Swipe direction before normalize: {swipeDirection}, Force to apply: {swipeDirection * forceMagnitude}");

        rigidbodyComponent.AddForce(swipeDirection * forceMagnitude);
    }

}
