using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public Vector3 velocity;
    [SerializeField] bool useRandomStartForce;
    [SerializeField] Vector3 startForce;
    [SerializeField] float maxVelocity;

    private void Start()
    {
        if (useRandomStartForce)
        {
            velocity += new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-maxVelocity, -2));
        }
        else
        {
            velocity += startForce;
        }
    }

    private void Update()
    {
        ReverseTruncate(ref velocity, maxVelocity);
        transform.position += velocity * Time.deltaTime;
    }

    void ReverseTruncate(ref Vector3 velocity, float maxSpeed)
    {
        if (velocity.magnitude != maxSpeed)
        {
            velocity.Normalize();
            velocity *= maxSpeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.collider.CompareTag("Wall") && collision.collider.transform.position.x != 0)
        //velocity.x *= -1;
        if (collision.collider.CompareTag("Wall") && collision.collider.transform.rotation.eulerAngles.y != 90)
        {
            velocity.x *= -1;
        }
        else
            velocity.y *= -1;

        Brick brick = collision.collider.GetComponent<Brick>();
        if (brick != null)
        {
            brick.DoDamage(1);
        }
    }
}
