using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{

    public float xOffset, yOffset, moveSpeed;

    ParticleSystem trail;
    Vector2 left, center, right;
    Vector2 startPos, endPos;//touch start and end points
    bool isTouchPossible = false;//indicates that a touch event has started
    Vector2 targetPos, deltaPos;
    int displacement;
    Animator playerTiltAnim;
    void Awake()
    {
        left = new Vector2(-xOffset, yOffset);
        right = new Vector2(xOffset, yOffset);
        center = new Vector2(0, yOffset);
        transform.position = center;
        targetPos = center;
        displacement = 0;
        playerTiltAnim = this.gameObject.GetComponent<Animator>();
    }
    void Update()
    {
        detectSwipes();//check for inputs
        setTargetPos();//respond to inputs
    }
    void detectSwipes()
    {//check for inputs
        if (Input.GetMouseButtonDown(0))
        {
            startPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                                        Input.mousePosition.y, 10f));
            endPos = startPos;
            isTouchPossible = true;
        }
        if (Input.GetMouseButtonUp(0) && isTouchPossible)
        {
            endPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                                        Input.mousePosition.y, 10f));
        }

        if (endPos.x - startPos.x > 1 && isTouchPossible)
        {//right swipe
            displacement = 1;
            if (displacement > 2) displacement = 2;
            isTouchPossible = false;
            playerTiltAnim.SetTrigger("turnRight");
        }
        else if (endPos.x - startPos.x < -1 && isTouchPossible)
        {//left swipe
            displacement = -1;
            if (displacement < -2) displacement = -2;
            isTouchPossible = false;
            playerTiltAnim.SetTrigger("turnLeft");
        }
    }
    void setTargetPos()
    {//respond to inputs
        if (targetPos == left && displacement > 0)
            targetPos.x += displacement * xOffset;
        else if (targetPos == center && displacement > 0)
            targetPos = right;
        else if (targetPos == center && displacement < 0)
            targetPos = left;
        else if (targetPos == right && displacement < 0)
            targetPos.x += displacement * xOffset;
        displacement = 0;

    }

    void FixedUpdate()
    {
        deltaPos = targetPos - (Vector2)transform.position;
        transform.position += (Vector3)deltaPos * moveSpeed * Time.deltaTime;
    }
    public bool didSwipeOccur()
    {//used in tutorial scene
        if (targetPos != center)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}