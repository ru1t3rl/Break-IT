using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;
using TMPro;

public class Ball : MonoBehaviour
{
    public Vector3 velocity;
    [SerializeField] bool useRandomStartForce;
    [SerializeField] Vector3[] startForceBounds = new Vector3[2];
    [SerializeField] Vector3 startForce;
    [SerializeField] float maxVelocity;
    [SerializeField] float maxVelocity_bak;
    [SerializeField] float velocityIncrement = 0.2f;

    public ScreenShake shake;

    Vector3 startPos;

    [SerializeField] float dissolveSpeed;
    bool hitBottom = false;
    Material mat;

    public GameObject sparkEffect;
    public int effectPoolSize = 15;
    int selectedEffect = 0;

    [HideInInspector]
    public List<GameObject> effects = new List<GameObject>();

    public int countDown = 3;
    public GameObject timeCanvas;
    [SerializeField] TextMeshProUGUI text;
    public bool frozen = true;

    AudioSource bounce;

    // Stats
    [SerializeField] TextMeshPro scoreText;
    int score = 0;
    [SerializeField] List<GameObject> liveObjects;
    int lives = 0;

    private void Start()
    {
        AddForce();

        maxVelocity_bak = maxVelocity;

        startPos = transform.position;
        mat = GetComponent<Renderer>().material;

        for (int iEffect = 0; iEffect < effectPoolSize; iEffect++)
        {
            effects.Add(Instantiate(sparkEffect, new Vector3(0, 0, 0), Quaternion.identity));
            effects[effects.Count - 1].hideFlags = HideFlags.HideInHierarchy;
        }

        StartCoroutine(CountDown());

        bounce = GetComponent<AudioSource>();

        lives = liveObjects.Count;
        scoreText.text = score.ToString();
    }

    IEnumerator CountDown()
    {
        timeCanvas.SetActive(true);

        for(int iTime = countDown; iTime-- > 0;){
            text.text = (iTime + 1).ToString();
            yield return new WaitForSeconds(1);
        }

        timeCanvas.SetActive(false);
        frozen = false;
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
        if (!frozen)
        {
            if (!hitBottom)
            {
                ReverseTruncate(ref velocity, maxVelocity);
                transform.position += velocity * Time.deltaTime;
                maxVelocity += velocityIncrement;
            }
            else
            {
                Dissolve(mat);

                if(lives - 1 >= 0)
                    Dissolve(liveObjects[lives - 1].GetComponent<Renderer>().material);

                if (mat.GetFloat("_DissolveValue") >= 1)
                {
                    transform.position = startPos;

                    AddForce();

                    mat.SetFloat("_DissolveValue", 0);
                    hitBottom = false;
                    maxVelocity = maxVelocity_bak;

                    TakeALive();

                    foreach(GameObject effect in effects)
                    {
                        effect.transform.rotation = Quaternion.identity;
                    }
                }
            }
        }
    }

    void Dissolve(Material mat)
    {
        mat.SetFloat("_DissolveValue", mat.GetFloat("_DissolveValue") + dissolveSpeed);
    }

    void ReverseTruncate(ref Vector3 velocity, float maxSpeed)
    {
        if (velocity.magnitude != maxSpeed)
        {
            velocity.Normalize();
            velocity *= maxSpeed;
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = score.ToString();
    }

    public void TakeALive()
    {
        lives--;
        if (lives >= 0)
            liveObjects[lives].SetActive(false);        
    }

    void OnCollisionEnter(Collision collision)
    {
        selectedEffect = (selectedEffect < effects.Count - 1) ? selectedEffect + 1 : 0;

        if (!collision.collider.CompareTag("WallBottom"))
        {
            VisualEffect effect = effects[selectedEffect].GetComponent<VisualEffect>();
            effect.gameObject.transform.position = this.transform.position;
            effect.SetVector3(Shader.PropertyToID("BallVelocity"), velocity);
            effect.Stop();
            effect.Play();
        }

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

        bounce.Stop();
        bounce.Play();
    }

    public int SelectedEffect { get => selectedEffect; }
}
