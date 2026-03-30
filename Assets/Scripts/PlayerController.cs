using System.Collections;

using UnityEngine;

using UnityEngine.InputSystem;



public class PlayerController : MonoBehaviour

{

    public float speed = 5f;



    public GameObject powerUpIndicator;



    private Rigidbody rb;



    private InputAction moveAction;

    private InputAction smashAction;

    private InputAction breakAction;



    private GameObject focalPoint;



    private bool hasPowerup = false;





    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()

    {

        rb = GetComponent<Rigidbody>();

        moveAction = InputSystem.actions.FindAction("Move");

        smashAction = InputSystem.actions.FindAction("Smash");

        breakAction = InputSystem.actions.FindAction("Break");



        focalPoint = GameObject.Find("FocalPoint");

    }



    // Update is called once per frame

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        
        // ---------------------------------------------------------
        // แก้ไขตรงนี้: เอาค่าแกน Y (หน้า/หลัง) และแกน X (ซ้าย/ขวา) มาคำนวณทิศทางร่วมกัน
        Vector3 moveDirection = (focalPoint.transform.forward * moveInput.y) + (focalPoint.transform.right * moveInput.x);
        
        // สั่งผลักไปตามทิศทางที่รวมกันแล้ว
        rb.AddForce(moveDirection * speed);
        // ---------------------------------------------------------

        if (breakAction.IsPressed())
        {
            rb.linearVelocity = Vector3.zero;
        }

        if (hasPowerup)
        {
            powerUpIndicator.transform.position = transform.position + new Vector3(0, 0.5f, 0);
            powerUpIndicator.transform.rotation = Quaternion.identity; // keep the indicator upright
        }

        if (smashAction.triggered && hasPowerup)
        {
            StartCoroutine(SmashActivate());
        }
    }


    void OnTriggerEnter(Collider other)

    {

        if (other.CompareTag("PowerUp"))

        {

            hasPowerup = true;

            Destroy(other.gameObject);

            StartCoroutine(PowerupCooldown());

        }

    }



    IEnumerator PowerupCooldown()

    {

        powerUpIndicator.SetActive(true);

        yield return new WaitForSeconds(10f);

        hasPowerup = false;

        Debug.Log("Powerup has ended");

        powerUpIndicator.SetActive(false);

    }



    void OnCollisionEnter(Collision other)

    {

        if (other.gameObject.CompareTag("Enemy") && hasPowerup)

        {

            var enemyRb = other.gameObject.GetComponent<Rigidbody>();

            Vector3 awayFromPlayer = other.transform.position - transform.position;

            enemyRb.AddForce(awayFromPlayer * 5f, ForceMode.Impulse);

            Debug.Log($"Player collided with {other.gameObject.name} and has powerup");

        }

    }



    IEnumerator SmashActivate()

    {

        

        float chargingTime = 0f;

        // player need to hold the smash button for 2 seconds for charing

        // then release the button to smash

        while (smashAction.IsPressed())

        {

            chargingTime += Time.deltaTime;

            Debug.Log($"smash charging time: {chargingTime}");

            if (chargingTime >= 2f)

            {

                Debug.Log("Smash activated");

                break;

            }

            yield return null;

        }



        // SEQ [2]

        if (chargingTime < 2f)

        {

            Debug.Log("Smash cancelled");

            yield break;

        }



        // SEQ [3]

        var enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);

        foreach (var enemy in enemies)

        {

            var enemyRb = enemy.GetComponent<Rigidbody>();

            enemyRb.AddExplosionForce(10f, transform.position, 20f, 0f, ForceMode.Impulse);

        }

    }

}



