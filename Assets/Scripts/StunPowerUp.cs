using UnityEngine;

public class StunPowerUp : MonoBehaviour
{
    public float stunDuration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            var enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            
            
            foreach (var enemy in enemies)
            {
                enemy.Stun(stunDuration);
            }

            Debug.Log("Enemies Stunned for 5 seconds!");
            
            
            Destroy(gameObject);
        }
    }
}