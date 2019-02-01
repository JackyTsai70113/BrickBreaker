﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseCollider : MonoBehaviour
{
    //cached reference
    Frame theFrame;
    Level level;
    void Start()
    {
        theFrame = FindObjectOfType<Frame>();
        level = FindObjectOfType<Level>();
    }

    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ball")
        {
            level.MinusBallNumber();
            if (level.ReturnBallNumber() <= 0)
            {
                other.GetComponent<Ball>().StopMoving();
                GetComponent<AudioSource>().Play();
                theFrame.DropLoseFrame();
                level.StopLevelWorking();
            }
            else if (level.ReturnBallNumber() > 1)
            {
                Destroy(other.gameObject);
            }
        }
        else if(other.gameObject.tag == "Fortune Square")
        {
            Destroy(other.gameObject);
        }
        else
        {
            Debug.LogError("Lose Collider collide with " + other.gameObject.tag);
        }
    }
}
