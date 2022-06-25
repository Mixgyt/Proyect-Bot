using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEnemyController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField]private float speed;
     [Range(0,0.3f)][SerializeField]private float smoothMovement;
    private Rigidbody2D rb2d;
    private bool lookToRigth=true;
    private Vector3 velocity = Vector3.zero;
    private Animator BombAnim;
    
    [Header("Detección")]
    [SerializeField]private float range;
    [SerializeField]private float burstRange;
    [SerializeField]private LayerMask playerMask;
    private GameObject Player;
    private HealthPlayer PlayerScript;
    private bool NearPlayer;
    private bool burst=true;

    void Start()
    {
     rb2d = GetComponent<Rigidbody2D>();
     BombAnim = GetComponent<Animator>();
     Player = GameObject.FindGameObjectWithTag("Player");
     PlayerScript = Player.GetComponent<HealthPlayer>();
    }

    void FixedUpdate(){
     float distPlayer = Vector2.Distance(transform.position, Player.transform.position);
     NearPlayer = distPlayer <= range;
     bool Burst = distPlayer <= burstRange;
    
    if(Burst&&burst){
       BurstAnim();
       speed*=0;
     }
     if(NearPlayer){
      FollowPlayer();
     }
    }

    private void FollowPlayer(){
        if(transform.position.x < Player.transform.position.x){
            Movement(speed*Time.fixedDeltaTime);
        }else if(transform.position.x > Player.transform.position.x){
            Movement(-speed*Time.fixedDeltaTime);
        }
    }

    private void Movement(float move){
        Vector3 ObjectiveVelocity = new Vector3(move, rb2d.velocity.y);
        rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity,ObjectiveVelocity, ref velocity, smoothMovement);

        if(move!=0){ BombAnim.SetBool("walking",true); }
        else{ BombAnim.SetBool("walking",false); }

        if(move<0 && lookToRigth){
         Turn();
        }else if(move>0 && !lookToRigth){
         Turn();
        }
    }

    private void Turn(){
        lookToRigth=!lookToRigth;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    
    private void BurstAnim(){
        BombAnim.SetBool("explosion",true);
        burst=false;
    }
    private void BurstEnemy(){
         float distPlayer = Vector2.Distance(transform.position, Player.transform.position);
        if(distPlayer<burstRange){ PlayerScript.DamageReceived(2); }
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected(){
        Gizmos.color=Color.yellow;
        Gizmos.DrawWireSphere(transform.position,range);
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(transform.position,burstRange);
    }
}
