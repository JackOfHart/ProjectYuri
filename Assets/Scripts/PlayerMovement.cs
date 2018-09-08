using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{

    public bool isGrounded;
    public int playerJumpPower = 1250;


    public float playerspeed = 0;
    public float maxSpeed = 10;
    public float accele = 0.1f;
    public float deccel = 0.1f;
    public float rotateSpeed = 10;

    //THESE ARE USED FOR UI
    public Text rotateScore;


    private float totalRotation = 0;
    private int rotateNumScore = 0;
    private int newScore = 0;
    private Vector3 PointDirection = new Vector3();
    private bool keyDown = false;

    private void Start()
    {
        rotateScore.text = "Score: 0";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Jump") && isGrounded == true)
        {
            Jump();
        }

        //Direction works kind of, the biggest issue is that when you change directions it doesn't take it into consideration in terms of speed
        //So the next step would be to make seperate acceleration diretions but we'll stop there for now. 
        //Oh, and the decceleration goes backwords eventually
        //Decided Direction

        if (Input.GetKey(KeyCode.W) && isGrounded == true)
        {
            PointDirection = new Vector3(1, 0);

            keyDown = true;
        }

        else if (Input.GetKey(KeyCode.S) && isGrounded == true)
        {
            PointDirection = new Vector3(-1, 0);
            keyDown = true;
        }

        else if (Input.GetKey(KeyCode.A) && isGrounded == true)
        {
            PointDirection = new Vector3(0, 0, 1);
            keyDown = true;
        }

        else if (Input.GetKey(KeyCode.D) && isGrounded == true)
        {
            PointDirection = new Vector3(0, 0, -1);
            keyDown = true;
        }




        if (Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.S) == false && Input.GetKey(KeyCode.D) == false)
        {
            playerspeed -= deccel * Time.deltaTime;
        }

        //Decceleration doesn't work 
        if (playerspeed < maxSpeed && keyDown == true)
        {
            playerspeed += accele * Time.deltaTime;

        }

        this.transform.position += PointDirection * playerspeed;

        //If the player is at max speed and the key is being pressed continue at max speed
        if (playerspeed >= maxSpeed)
        {
            playerspeed = maxSpeed;
        }


        //Fixes the issue where player has negative speed.
        if (playerspeed < 0)
        {
            playerspeed = 0;
        }

        //AIR ROTATION
        if (Input.GetMouseButton(0) && isGrounded == false)
        {
            
            //What is the current rotate score
            transform.Rotate(Vector3.down * rotateSpeed * (playerspeed * 10) * Time.deltaTime);
            rotateNumScore += 1;
            
        }

        if (Input.GetMouseButton(1) && isGrounded == false)
        {
            
            transform.Rotate(Vector3.up * rotateSpeed * (playerspeed * 10) * Time.deltaTime);

            rotateNumScore += 1;
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded == true)
        {
            this.transform.localScale = new Vector3(1, .75f, 1);
        }
        else
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }

        //Get the Screen positions of the object
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

        //Get the Screen position of the mouse
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        //Get the angle between the points
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

        if (isGrounded == true)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0f, -angle, 0f));
        }



    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    void Jump()
    {
        //Jumping Code
        GetComponent<Rigidbody>().AddForce(Vector3.up * playerJumpPower);

        //When player is jumping they can rotate but they should be moving position when they rotate.
        
        isGrounded = false;
        
    }


    private void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.tag == "ground")
        {
            isGrounded = true;
            UpdateScore();
        }

    }

    void UpdateScore()
    {
        

        newScore = rotateNumScore;

        rotateScore.text = ("Score: " + newScore);

    }

}
