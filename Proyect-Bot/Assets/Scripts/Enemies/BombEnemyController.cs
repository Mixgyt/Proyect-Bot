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
    private PlayerController PlayerScript;
    private bool NearPlayer;

    void Start()
    {
     rb2d = GetComponent<Rigidbody2D>();
     BombAnim = GetComponent<Animator>();
     Player = GameObject.FindGameObjectWithTag("Player");
     PlayerScript = Player.GetComponent<PlayerController>();
    }

    void FixedUpdate(){
     float distPlayer = Vector2.Distance(transform.position, Player.transform.position);
     NearPlayer = distPlayer <= range;
     bool Burst = distPlayer <= burstRange;
    
    if(Burst){
       StartCoroutine(BurstEnemy());
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

    private IEnumerator BurstEnemy(){
        yield return new WaitForSecondsRealtime(0.2f);
        float distPlayer = Vector2.Distance(transform.position, Player.transform.position);
        if(distPlayer<burstRange){ PlayerScript.DamageReceived(2); }
        Destroy(gameObject);
        yield return null;
    }

    void OnDrawGizmosSelected(){
        Gizmos.color=Color.yellow;
        Gizmos.DrawWireSphere(transform.position,range);
        Gizmos.color=Color.red;
        Gizmos.DrawWireSphere(transform.position,burstRange);
    }
}
