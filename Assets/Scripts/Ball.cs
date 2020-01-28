using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class Ball : MonoBehaviour
{
    public Vector3 velocity;
    [SerializeField] bool useRandomStartForce;
    [SerializeField] Vector3[] startForceBounds = new Vector3[2];
    [SerializeField] Vector3 startForce;
    [SerializeField] float maxVelocity;

    Vector3 startPos;

    [SerializeField] float dissolveSpeed;
    bool hitBottom = false;
    Material mat;

    public VisualEffect sparks;

    private void Start()
    {
        AddForce();

        startPos = transform.position;
        mat = GetComponent<Renderer>().material;

        sparks.transform.position = startPos;
    }

    void AddForce()
    {
        if (useRandomStartForce)
        {
            velocity += new Vector3(Random.Range(startForceBounds[0].x, startForceBounds[1].x),
            Random.Range(startForceBounds[0].y, startForceBounds[1].y),
            Random.Range(startForceBounds[0].z, startForceBounds[1].z));
        }
        else
        {
            velocity += startForce;
        }
    }

    void Update()
    {
        if (!hitBottom)
        {
            ReverseTruncate(ref velocity, maxVelocity);
            transform.position += velocity * Time.deltaTime;
        }
        else
        {
            mat.SetFloat("_DissolveValue", mat.GetFloat("_DissolveValue") + dissolveSpeed);

            if (mat.GetFloat("_DissolveValue") >= 1)
            {
                transform.position = startPos;

                AddForce();

                mat.SetFloat("_DissolveValue", 0);
                hitBottom = false;
            }
        }
    }

    void ReverseTruncate(ref Vector3 velocity, float maxSpeed)
    {
        if (velocity.magnitude != maxSpeed)
        {
            velocity.Normalize();
            velocity *= maxSpeed;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        sparks.gameObject.transform.position = this.transform.position;
        sparks.SetVector3(Shader.PropertyToID("BallVelocity"), velocity);
        sparks.Stop();
        sparks.Play();

        if (collision.collider.CompareTag("WallBottom"))
        {
            hitBottom = true;
        }
        else
        {
            Brick brick = collision.collider.GetComponent<Brick>();
            if (brick != null)
            {
                brick.DoDamage(1);
            }
        }
    }
}
