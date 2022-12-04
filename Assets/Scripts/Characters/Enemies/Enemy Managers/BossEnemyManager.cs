using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyManager : Enemy
{    
    public GameObject projectile;
    public GameObject bomb;
    public BossTentacleAttackSpawner tSpawner;
    public Animator animator;
    public float maxCooldown = 5f;
    public float minCooldown = 1f;

    private float tCooldown = 3f;
    private float attackNumber = 0;
    private float tAttackNumber = 0;
    private float cooldown;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();


    }

    // Update is called once per frame
    void Update()
    {
        if(!alerted){
            return;
        }

        GameObject player = PlayerSystem.Inst.GetPlayer();

        if(player == null){
            return;
        }

        //float cooldown = Random.Range(minCooldown, maxCooldown);
        if(cooldown <= 0){
            if(attackNumber >= 5){
                //Instantiate(bomb);
                attackNumber = 0;
            }else{
                attackNumber++;
                //Instantiate(projectile);
            }
            animator.Play("Attack");
            cooldown = Random.Range(minCooldown, maxCooldown);
            
        }else{
            cooldown -= Time.deltaTime;
        }

        if(tCooldown <= 0){
            if(tAttackNumber >= 5){
                tSpawner.SpawnWave(Random.Range(5, 20));
                tAttackNumber = 0;
            }else{
                tAttackNumber++;
                tSpawner.Spawn(Random.Range(1, 4));
            }
            tCooldown = 3f;
        }else{
            tCooldown -= Time.deltaTime;
        }

        
    }

    public override void Kill()
    {
        // Stub, add death sequence/game win
        Destroy(gameObject);
    }
}
