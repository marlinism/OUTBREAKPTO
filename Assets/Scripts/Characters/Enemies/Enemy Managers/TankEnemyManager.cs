using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankEnemyManager : Enemy
{
    public Animator animator;
    public Hitbox hitbox;

    public float attackSpeed;
    public float attackDuration;

    public bool attacking = false;
    public string vertical = " ";
    public string horizontal = " ";

    public float attackCoolDown;

    private Vector2 attackDirection;

    private float nextAction = 0f;

    private float attackDistance = 1.5f;
    private float distance = 0f;
    private bool directionLock = false;

    //private bool isAlive = true;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (Player == null)
        {
            return;
        }

        distance = DistanceFromPlayer();
        updateDirection();

        //update sprite 
        Vector3 characterScale = transform.localScale;
        if (horizontal.Equals("right") && !directionLock)
        {
            if(vertical.Equals("down")){
                characterScale.x = -(Mathf.Abs(transform.localScale.x));
            }else{
                characterScale.x = Mathf.Abs(transform.localScale.x);
            }
        }
        if (horizontal.Equals("left") && !directionLock)
        {
            if(vertical.Equals("down")){
                characterScale.x = Mathf.Abs(transform.localScale.x);
            }else{
                characterScale.x = -(Mathf.Abs(transform.localScale.x));
            }
        }
        transform.localScale = characterScale;

        //Check attack state
        if (attacking == false)
        {
            //update movement
            if (distance > attackDistance)
            {
                //transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
                MoveTowardsPoint(Player.transform.position);
                animator.SetBool("idle", false);
            }
            else
            {
                StopMovement();
                animator.SetBool("idle", true);
                

                //if cooldown time if over then attack
                if (Time.time > nextAction)
                {
                    attackDirection = Player.transform.position - transform.position;
                    animator.SetBool("idle", false);
                    animator.SetBool("attacking", true);
                    attacking = true;
                    //hitbox.SetActive(true);
                    nextAction = Time.time + attackDuration;
                    rb.velocity = new Vector2(0, 0);
                    //rb.velocity = attackDirection * attackSpeed;
                    directionLock = true;
                }
            }
        }
        else
        {
            //stop attacking if attack duration is over
            if (Time.time > nextAction)
            {
                attacking = false;
                hitbox.Disable();
                directionLock = false;
                animator.SetBool("attacking", false);
                
                nextAction = Time.time + attackCoolDown;
                animator.SetBool("idle", true);
            }else{
                if(Time.time > (nextAction - 0.20)){
                    hitbox.Enable();
                }
            }
        }

    }

    // Update the direction string to indicate whatch direction we are primarily moving
    void updateDirection()
    {
        Vector2 currDirection = DirectionTowardsPlayer();
        float x = currDirection.x;
        float y = currDirection.y;

        if(x <= 0){
            horizontal = "right";
        }else{
            horizontal = "left";
        }

        if(y <= 0){
            animator.SetInteger("vertical", 1);
            vertical = "down";

        }else{
            animator.SetInteger("vertical", 2);
            vertical = "up";

        }

        /*
        if (Mathf.Abs(x) >= Mathf.Abs(y))
        {
            animator.SetInteger("direction", 1);
            if (x > 0)
            {
                direction = "right";
            }
            else
            {
                direction = "left";
            }

        }
        else
        {
            if (y > 0)
            {
                animator.SetInteger("direction", 3);
                direction = "up";
            }
            else
            {
                animator.SetInteger("direction", 2);
                direction = "down";
            }
        }
        */

    }

    // Damagable method implementations
    public override void Kill()
    {
        // Stub, add death animation
        Destroy(gameObject);
    }
}
