using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletVelocity = 20f;
    float xSpeed;
    Rigidbody2D bulletRigidBody;
    PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        bulletRigidBody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * bulletVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        bulletRigidBody.velocity = new Vector2(xSpeed, 0f);
        transform.localScale = new Vector2(Mathf.Sign(bulletRigidBody.velocity.x), 1f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }

        Destroy(gameObject, 4f);
    }
}
