using UnityEngine;

public class SwipeToForceRollOrThrow : MonoBehaviour
{
    public Rigidbody rigidbodyComponent;
    public Camera mainCamera;
    public float forceMultiplier = 10f;
    public float sensitivity = 1.0f;

    private Vector2 startPosition;
    private float startTime;
    private bool swipeInProgress = false;

    void Start()
    {
        if (mainCamera != null)
        {
            mainCamera.transform.position = new Vector3(0, 2.5f, -11.5f);
            mainCamera.transform.rotation = Quaternion.LookRotation(Vector3.forward);
            mainCamera.transform.rotation = Quaternion.Euler(15f, 0, 0);// Ensure it looks towards the positive x-axis
        }
        else
        {
            Debug.LogError("Main camera reference not set in the inspector.");
        }

        if (rigidbodyComponent != null)
        {
            rigidbodyComponent.useGravity = true;  // Make sure gravity is enabled
        }
        else
        {
            Debug.LogError("Rigidbody component reference not set in the inspector.");
        }
    }



    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
            startTime = Time.time;
            swipeInProgress = true;
            Debug.Log("Swipe started at: " + startPosition);
        }

        if (Input.GetMouseButtonUp(0) && swipeInProgress)
        {
            Vector2 endPosition = Input.mousePosition;
            float endTime = Time.time;
            Vector2 swipeVector = endPosition - startPosition;
            float swipeDuration = endTime - startTime;
            Debug.Log("Swipe ended at: " + endPosition + ", Swipe vector: " + swipeVector);
            // Check if the Shift key is held down
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                ProcessSwipeThrow(swipeVector, swipeDuration);
            }
            else
            {
                ProcessSwipeRoll(swipeVector, swipeDuration);
            }
            swipeInProgress = false;
        }
    }

    void ProcessSwipeThrow(Vector2 swipeVector, float duration)
    {
        float speed = swipeVector.magnitude / duration; // Calculate the speed of the swipe
        float forceMagnitude = speed * forceMultiplier * sensitivity; // Calculate the overall force magnitude

        // Correct mapping of swipe directions to world axes:
        // Horizontal swipe (x) affects the world's x-axis (left-right).
        // Vertical swipe (y) affects the world's y-axis (up-down).
        Vector3 forceDirection = new Vector3(swipeVector.x, swipeVector.y, 0).normalized;

        // Apply the calculated force
        Vector3 forceApplied = forceDirection * forceMagnitude;

        Debug.Log($"Swipe vector: {swipeVector}");
        Debug.Log($"Force direction (normalized): {forceDirection}");
        Debug.Log($"Force applied: {forceApplied}");

        rigidbodyComponent.AddForce(forceApplied);
    }

    void ProcessSwipeRoll(Vector2 swipeVector, float duration)
    {
        float speed = swipeVector.magnitude / duration; // Calculate the speed of the swipe
        float forceMagnitude = speed * forceMultiplier * sensitivity; // Calculate the overall force magnitude

        // Map swipe directions to world axes:
        // Horizontal swipe (x) affects the world's x-axis (left-right).
        // Vertical swipe (y) affects the world's z-axis (forward-backward).
        Vector3 forceDirection = new Vector3(swipeVector.x, 0, swipeVector.y).normalized;

        // Apply the calculated force
        Vector3 forceApplied = forceDirection * forceMagnitude;

        Debug.Log($"Swipe vector: {swipeVector}");
        Debug.Log($"Force direction (normalized): {forceDirection}");
        Debug.Log($"Force applied: {forceApplied}");

        rigidbodyComponent.AddForce(forceApplied);
    }






}
