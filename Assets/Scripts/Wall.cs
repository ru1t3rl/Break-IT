﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Ball ball = collision.collider.GetComponent<Ball>();
        if (ball != null)
        {
            if (transform.rotation.eulerAngles.z == 0)
            {
                ball.velocity.x *= -1;
                ball.effects[ball.SelectedEffect].transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (transform.rotation.eulerAngles.z == 90)
            {
                ball.velocity.y *= -1;
                ball.effects[ball.SelectedEffect].transform.rotation = Quaternion.Euler(0, 0, 90);
            }
        }
    }
}