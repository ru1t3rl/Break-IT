using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Padle : MonoBehaviour
{
    [SerializeField] float acceleration;

    private void Update()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal") * acceleration * Time.deltaTime, 0, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Ball ball = collision.collider.GetComponent<Ball>();

        if(ball != null)
        {
            ball.velocity.y *= -1;

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                if (ball.velocity.x > 0)
                    ball.velocity.x *= -1 - 0.3f;
                else
                    ball.velocity.x -= 0.2f;
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) {
                if (ball.velocity.x > 0)
                    ball.velocity.x *= -1 + 0.3f;
                else
                    ball.velocity.x += 0.3f; 
            }
        }

    }
}
