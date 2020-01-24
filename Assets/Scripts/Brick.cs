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

    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material = materials[health - 1];
    }

    private void Update()
    {
        if (health <= 0)
            gameObject.SetActive(false);
    }

    public void DoDamage(int lives)
    {
        health -= lives;
        if (health >= 0)
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
}
