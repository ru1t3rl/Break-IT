using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Padle : MonoBehaviour
{
    [SerializeField] float acceleration;
    [SerializeField] float maxSpeed;
    float speed;
    [SerializeField] float friction;
    [SerializeField] float mass;
    Vector3 velocity;

    private void Update()
    {
        speed += Input.GetAxis("Horizontal") * acceleration * Time.deltaTime;

        speed = (speed > maxSpeed) ? maxSpeed : speed;
        velocity.x = speed;
        transform.position += velocity * Time.fixedDeltaTime;

        speed /= friction;

        if (speed <= 0.01f && speed >= -0.01f)
            speed = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Ball ball = collision.collider.GetComponent<Ball>();

        if(ball != null && speed != 0)
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                if (ball.velocity.y > 0)
                    ball.velocity.y *= -1 - 0.3f;
                else
                    ball.velocity.y -= 0.2f;
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
                if (ball.velocity.y > 0)
                    ball.velocity.y *= -1 + 0.3f;
                else
                    ball.velocity.y += 0.3f; 
            }
        }

    }
}
