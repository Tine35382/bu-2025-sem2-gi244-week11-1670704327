using UnityEngine;
using System.Collections; 

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    private Rigidbody rb;
    private GameObject player;
    
    private bool isStunned = false; 

    void Awake()
    {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        
        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
        

        if (isStunned) return; 

        var dir = player.transform.position - transform.position;
        dir = dir.normalized;
        rb.AddForce(dir * speed);
    }

    public void Stun(float duration)
    {
        StartCoroutine(StunRoutine(duration));
    }

    private IEnumerator StunRoutine(float duration)
    {
        isStunned = true;
        rb.linearVelocity = Vector3.zero; 
        
        yield return new WaitForSeconds(duration); 
        
        isStunned = false; 
    }
}