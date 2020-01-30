using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public int health;
    [SerializeField] List<Material> materials;
    Renderer rend;
    public Vector2Int id;
    bool toggled = false;

    bool startedDisolve = false;
    [SerializeField] float dieTime = 0.5f;
    float time2Die;

    AudioSource explosion;

    Ball ball;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material = materials[health - 1];

        explosion = GetComponent<AudioSource>();
        if (GetComponent<Collider>().isTrigger)
        {
            GetComponent<Collider>().isTrigger = false;
        }

        if (gameObject.activeSelf)
        {
            transform.parent.GetComponent<Level_Creater>().activeBricks += 1;
            transform.parent.GetComponent<Level_Creater>().addedBrick = true;
        }
    }

    private void Update()
    {
        if (health <= 0)
        {
            if (!startedDisolve)
            {
                explosion.Stop();
                explosion.Play();

                if (ball != null)
                    ball.shake.Shake();

                GetComponent<Collider>().enabled = false;
                startedDisolve = true;
                time2Die = Time.time + dieTime;
            }

            if (Time.time < time2Die)
            {
                rend.material.SetFloat("_DissolveValue", rend.material.GetFloat("_DissolveValue") + (100 / (dieTime * 10) / 100));
            }
            else if (Time.time >= time2Die)
            {
                transform.parent.GetComponent<Level_Creater>().activeBricks -= 1;

                GetComponent<Collider>().enabled = true;
                startedDisolve = false;

                gameObject.SetActive(false);
            }
        }
    }

    public void DoDamage(int lives)
    {
        health -= lives;
        if (health > 0)
        {
            rend.material = materials[health - 1];
        }
        else if (health > materials.Count - 1)
            rend.material = materials[materials.Count - 1];
    }

    public void AddHealht(int lives)
    {
        health += lives;
        if (health < materials.Count - 1 && health >= 0)
            rend.material = materials[health - 1];
        else if (health > materials.Count - 1)
        {
            rend.material = materials[materials.Count - 1];
        }
        else
            rend.material = materials[0];
    }

    public void ToggleVisble(bool status)
    {
        gameObject.SetActive(status);

        toggled = true;
    }

    public bool Toggled { get => toggled; }

    void OnCollisionEnter(Collision collision)
    {
        ball = collision.collider.GetComponent<Ball>();
        ball.AddScore(10);
        if (ball != null)
        {
            bool hitX = ball.transform.position.x > this.transform.position.x + this.transform.localScale.x / 2 || ball.transform.position.x < this.transform.position.x - this.transform.localScale.x / 2;
            bool hitY = ball.transform.position.y > this.transform.position.y + this.transform.localScale.y / 2 || ball.transform.position.y < this.transform.position.y - this.transform.localScale.y / 2;

            if (hitX)
            {
                ball.velocity.x *= -1;
                ball.effects[ball.SelectedEffect].transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (hitY)
                ball.velocity.y *= -1;
            ball.effects[ball.SelectedEffect].transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }
}
