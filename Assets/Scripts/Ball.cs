﻿using UnityEngine;

public class Ball : MonoBehaviour {
    // config params
    [SerializeField] float xPush = 2f;
    [SerializeField] float yPush = 2f;
    [SerializeField] AudioClip ballSounds;
    [SerializeField] float randomFactor = 0.15f;

    //Cached component references
    [SerializeField] GameObject ballSprite;
    float ballSizeX;
    float ballSizeY;
    float ballSizeZ;
    AudioSource myAudioSource;
    Rigidbody2D myRigidbody2D;
    Level level;
    Paddle paddle;

    // state variables
    Vector2 paddleToBallVector;
    private bool hasStarted = false;
    [SerializeField] float fortuneStartingTime;
    [SerializeField] float fortuneTimeLength;
    [SerializeField] bool ifFortuneStart;

    void Start()
    {
        ballSizeX = transform.localScale.x;
        ballSizeY = transform.localScale.y;
        ballSizeZ = transform.localScale.z;
        level = FindObjectOfType<Level>();
        paddle = FindObjectOfType<Paddle>();       
        paddleToBallVector = 
            transform.position - paddle.transform.position;
        myAudioSource = GetComponent<AudioSource>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
        fortuneTimeLength = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (level.IsLevelWorking())
        {
            if (!hasStarted)
            {
                LockBallToPaddle();
                LauchOnMouseClick();
            }
            else if (ifFortuneStart && Time.time - fortuneStartingTime >= fortuneTimeLength)
            {
                transform.localScale = new Vector3
                    (ballSizeX, ballSizeY, ballSizeZ);
            }
        }
        //Debug.Log(myRigidbody2D.velocity.magnitude);
    }

    private void LauchOnMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hasStarted = true;
            myRigidbody2D.velocity = new Vector2(xPush, yPush);
        }
    }
    private void LockBallToPaddle()
    {
        Vector2 paddlePos = new Vector2(
        paddle.transform.position.x, 
            paddle.transform.position.y);
        transform.position = paddlePos + paddleToBallVector;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 velocityTweak = new Vector2
            (Random.Range(-randomFactor, randomFactor),
            Random.Range(-randomFactor, randomFactor));
        if (hasStarted)
        {
            myAudioSource.PlayOneShot(ballSounds);
            if (level.IsLevelWorking())
            {
                myRigidbody2D.velocity += velocityTweak;
            }
        }
    }

    public void StopMoving()
    {
        myRigidbody2D.velocity = new Vector2(0f, 0f);
    }

    public void ChangeBallScale(float ballSizeScale)
    {
        fortuneStartingTime = Time.time;
        transform.localScale = new Vector3
            (ballSizeX * ballSizeScale, ballSizeY * ballSizeScale, ballSizeZ);
        if(ballSizeScale > 1)
        {
            level.playSoundEffect("good");
        }
        else if (ballSizeScale < 1)
        {
            level.playSoundEffect("bad");
        }
    }

    public void MultiBall()
    {
        float deltaVelocity = 0.705f;
        level.playSoundEffect("good");
        CreatABallWithDeltaVelocity(+deltaVelocity, +deltaVelocity);
        CreatABallWithDeltaVelocity(+deltaVelocity, -deltaVelocity);
    }

    private void CreatABallWithDeltaVelocity(float deltaVelocityX, float deltaVelocityY)
    {
        GameObject cloneBall = Instantiate
            (ballSprite, transform.position, transform.rotation);
        cloneBall.name = "Ball";
        cloneBall.GetComponent<Rigidbody2D>().velocity = new Vector2
            (myRigidbody2D.velocity.magnitude * deltaVelocityX
            , myRigidbody2D.velocity.magnitude * deltaVelocityY);
        cloneBall.GetComponent<Ball>().SetBallStart();
        level.AddBallNumber();
        
    }

    public void SetBallStart()
    {
        hasStarted = true;
    }

    private float AbsOfVector2(Vector2 vt2)
    {
        return Mathf.Sqrt(
            Mathf.Pow(vt2.x, 2) + Mathf.Pow(vt2.y, 2));
    }
}
