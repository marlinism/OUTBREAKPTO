using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossEnemyManager : Enemy
{    
    public GameObject projectile;
    public GameObject bomb;
    public GameObject projectileSpawnLocation;
    public BossTentacleAttackSpawner tSpawner;
    public Animator animator;
    public DoorController bossDoor;
    public float maxCooldown = 5f;
    public float minCooldown = 1f;

    private float tCooldown = 3f;
    private float attackNumber = 0;
    private float tAttackNumber = 0;
    private float cooldown;
    public AudioClip newTrack;
    private AudioManager theAM;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        theAM = FindObjectOfType<AudioManager>();
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
                GameObject instance = Instantiate(projectile);
                instance.transform.position = projectileSpawnLocation.transform.position;
            }
            animator.Play("Attack");
            cooldown = Random.Range(minCooldown, maxCooldown);
            
        }else{
            cooldown -= Time.deltaTime;
        }

        if(tCooldown <= 0){
            if(tAttackNumber >= 5){
                tSpawner.SpawnWave(Random.Range(50, 100));
                //tSpawner.SpawnWave();
                tAttackNumber = 0;
            }else{
                tAttackNumber++;
                tSpawner.Spawn(Random.Range(1, 4));
            }
            tCooldown = 3.5f;
        }else{
            tCooldown -= Time.deltaTime;
        }

        
    }

    public override void Kill()
    {
        // Stub, add death sequence/game win
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Destroy(gameObject);
    }

    public override bool Alert(bool createAlertSignal = false)
    {
        bool toReturn = base.Alert(createAlertSignal);

        if (alerted)
        {
            bossDoor.Close();
            bossDoor.Locked = true;

            GameSystem.Inst.CameraControl.SecondaryTarget = gameObject;
            GameSystem.Inst.CameraControl.ChangeCameraSizeScale(ZoomLevel.ZoomOut2);
            if(newTrack != null) {
                theAM.ChangeBGM(newTrack);
            }
        }

        return toReturn;
    }
}
