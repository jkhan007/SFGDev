using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Check if it collided with left/right wall by tag or name
        if (other.CompareTag("LeftWall") || other.CompareTag("RightWall"))
        {
            Collide();
        }
    }

    void Collide()
    {
        Debug.Log("Player collided with wall!");
        // Add your response here (e.g., stop movement, play effect, etc.)
    }
}
