using UnityEngine;

public class SwipeToForce : MonoBehaviour
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
            mainCamera.transform.position = new Vector3(0, 1.6f, -2.5f);
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
            //Debug.Log("Swipe started at: " + startPosition);
        }

        if (Input.GetMouseButtonUp(0) && swipeInProgress)
        {
            Vector2 endPosition = Input.mousePosition;
            float endTime = Time.time;
            Vector2 swipeVector = endPosition - startPosition;
            float swipeDuration = endTime - startTime;
            //Debug.Log("Swipe ended at: " + endPosition + ", Swipe vector: " + swipeVector);
            // Check if the Shift key is held down
            // Use the swipe analysis to determine the action
            if (IsSwipeForThrow(swipeVector, swipeDuration))
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

    bool IsSwipeForThrow(Vector2 swipeVector, float duration)
    {
        Debug.Log($"Swipe Y: {swipeVector.y}");
        if (swipeVector.y < 0)
            return false;
        float swipeLength = swipeVector.magnitude; // Length of the swipe
        float swipeSpeed = swipeLength / duration; // Speed of the swipe

        // Define thresholds for a throw
        float lengthThreshold = 200.0f; // Adjust this value based on your game's scale
        float speedThreshold = 1400.0f; // Adjust this value based on expected swipe speeds
        Debug.Log($"Swipe Length: {swipeLength}");
        Debug.Log($"Swipe Speed: {swipeSpeed}");
        // Check if the swipe meets the criteria for a throw
        return swipeLength > lengthThreshold && swipeSpeed > speedThreshold;
    }


    void ProcessSwipeThrow(Vector2 swipeVector, float duration)
    {
        float speed = swipeVector.magnitude / duration; // Calculate the speed of the swipe
        float forceMagnitude = speed * forceMultiplier * sensitivity; // Calculate the overall force magnitude

        // Add z-axis force based on the speed of the swipe
        // You can adjust the multiplier for zForceMultiplier to control how much influence the speed has
        float zForceMultiplier = 0.03f; // This is a scaling factor for how much the z-force is affected by speed
        float zForce = speed * zForceMultiplier;

        // Calculate force directions
        Vector3 forceDirection = new Vector3(swipeVector.x * 0.3f, swipeVector.y * 5f * zForceMultiplier, zForce).normalized;

        // Apply the calculated force
        Vector3 forceApplied = forceDirection * forceMagnitude;

        //Debug.Log($"Swipe vector: {swipeVector}");
        // Debug.Log($"Force direction (normalized): {forceDirection}");
        // Debug.Log($"Force applied: {forceApplied}");

        rigidbodyComponent.AddForce(forceApplied);
    }


    void ProcessSwipeRoll(Vector2 swipeVector, float duration)
    {
        float speed = swipeVector.magnitude / duration; // Calculate the speed of the swipe
        float forceMagnitude = speed * forceMultiplier * sensitivity; // Calculate the overall force magnitude

        // Directly map swipe vector to force directions:
        // x-component affects the world's x-axis (left-right).
        // y-component affects the world's z-axis (forward-backward), but needs correct mapping:
        Vector3 forceDirection = new Vector3(swipeVector.x, 0, swipeVector.y).normalized; // Negate y to z mapping to fit camera perspective

        // Apply the calculated force
        Vector3 forceApplied = forceDirection * forceMagnitude;

        //Debug.Log($"Swipe vector: {swipeVector}");
        // Debug.Log($"Force direction (normalized): {forceDirection}");
        //Debug.Log($"Force applied: {forceApplied}");

        rigidbodyComponent.AddForce(forceApplied);
    }








}
