using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            PlayerController player = other.collider.GetComponent<PlayerController>();
            player?.Die();
        }
    }
}
