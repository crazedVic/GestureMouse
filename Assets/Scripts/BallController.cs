using UnityEngine;

public class BallController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 originalPosition = new Vector3(0, 0, 0);  // Set this to your desired reset position

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on the object!");
        }
    }

    void FixedUpdate()
    {
        if (rb.position.y < -2.4f)  // Check if the y-coordinate is below -1.2
        {
            rb.Sleep();
            rb.MovePosition(originalPosition);  // Move the Rigidbody to the original position
            rb.velocity = Vector3.zero;  // Reset the velocity to zero
        }
    }
}
