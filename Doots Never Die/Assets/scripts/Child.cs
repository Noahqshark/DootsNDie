using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Child : MonoBehaviour
{
    // Base Variables
    private Rigidbody2D rigidBody;
    private BoxCollider2D collider;
    private Vector3 currentPosition;
    private Vector3 previousPosition;
    private Vector3 moveAmount;
    private bool scared;
    private int scareDuration;
    private int timer;

    public Player player;
    public Aim aim;
    public Spawner spawner;
    public int hp;
    public int speed;

    // Start is called before the first frame update
    // Setting our base variables to their proper values
    //-------------------------------------------------------------------------------------------------------------
    void Start()
    {
        scared = false;
        timer = 0;
        hp = 40;
        rigidBody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        moveAmount = Vector3.zero;
    }

    // Update is called once per frame
    //-------------------------------------------------------------------------------------------------------------
    void Update()
    {

        //I will need to know how projectiles are done to make this work
        if (aim.projectileList.Count > 0)
        {
            for (int i = 0; i < aim.projectileList.Count; i++)
            {
                CheckForDamage(aim.projectileList[i].GetComponent<BoxCollider2D>());
            }
        }

        //if the child is too scared, they leave the game
        if(hp <= 0)
        {
            player.timer += 5.0f;
            spawner.children.Remove(gameObject);
            Destroy(gameObject);
            return;
        }

        //if the child has been scared for 5 seconds, it is no longer scared. A projectile resets this counter
        //also, the counter will reset on its own in 5 seconds so that the int doesn't increase
        //to infinity and cause memory issues
        if(scareDuration > 300)
        {
            scared = false;
            scareDuration = 0;
        }

        //moving directly away from the player if scared
        if(scared == true)
        {
            moveAmount = new Vector3(-player.transform.position.x + currentPosition.x, 
                                     -player.transform.position.y + currentPosition.y, 0);
        }
        
        //moving towards the player if they get close
        else if(Vector3.Distance(player.transform.position, currentPosition) < 2)
        {
            moveAmount = new Vector3(player.transform.position.x - currentPosition.x, 
                                     player.transform.position.y - currentPosition.y, 0);
        }

        //randomized wandering otherwise
        else if (timer == 0)
        {
            moveAmount = new Vector3(Random.Range(-1.0f, 2.0f), Random.Range(-1.0f, 2.0f), 0);
        }

        //updating timers
        timer++;
        scareDuration++;

        //if two seconds have passed - change directions
        if(timer > 120)
        {
            timer = 0;
        }

        //where the object was last frame
        previousPosition = currentPosition;

        //actually moving the object
        moveAmount = moveAmount.normalized * Time.deltaTime * speed;
        rigidBody.MovePosition(currentPosition + moveAmount);

        //updating where the object is located
        currentPosition = transform.position;

    }

    // Checks if bodies are colliding and removes if true
    //-------------------------------------------------------------------------------------------------------------
    void CheckForDamage(Collider2D attack)
    {
        if (collider.IsTouching(attack))
        {
            aim.projectileList.Remove(attack.gameObject);
            Destroy(attack.gameObject);
            hp -= 10;
            scared = true;
            scareDuration = 0;
        }
    }

    /// <summary>
    /// Checks for the player and takes HP off them
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collider.IsTouching(player.GetComponent<BoxCollider2D>()))
        {
            player.timer -= 5.0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Hitbox")
        {
            hp -= 10;
            scared = true;
            scareDuration = 0;
            //Debug.Log("Hit!"); //Tests if collison works
        }
    }
}
