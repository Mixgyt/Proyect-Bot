using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricEnemyController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField]private float Speed;
    private Rigidbody2D rb2d;
    [SerializeField]private float DetectedDistance;
    [SerializeField]private Transform ControllerObj;
    [SerializeField]private bool MoveToRigth;
    private bool WaitTime=false;
    private float HorizontalMovement;

    
    [Header("Ataque")]
    [SerializeField]private float AttackRange;
    [SerializeField]private LayerMask PlayerMask;
    private bool NearPlayer;
    private Animator EAnim;
    private GameObject Player;
    private HealthPlayer PlayerScript;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        EAnim = GetComponent<Animator>();
        PlayerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthPlayer>();
        Player = GameObject.FindGameObjectWithTag("Player");

        HorizontalMovement = Speed;

        if(!MoveToRigth){
            StartCoroutine(Turn());
        }
    }

    void FixedUpdate(){
        NearPlayer = Physics2D.OverlapCircle(transform.position,AttackRange,PlayerMask);
        RaycastHit2D InfoGround = Physics2D.Raycast(ControllerObj.position, Vector2.down, DetectedDistance);

        rb2d.velocity = new Vector2(HorizontalMovement, rb2d.velocity.y);

        if(NearPlayer){
            Attack();
        }else{
            EAnim.SetBool("ataque",false);
        }

        if(!WaitTime){
            EAnim.SetBool("caminar",true);
        }

        if(!InfoGround){
         StartCoroutine(Turn());
        }
    }

    private void Attack(){
        StartCoroutine(WaitT());
        EAnim.SetBool("ataque",true);
        PlayerScript.DamageReceived(1);
    }

    private IEnumerator WaitT(){
        WaitTime=true;
        EAnim.SetBool("caminar",false);
        HorizontalMovement = 0f;

      yield return new WaitForSecondsRealtime(1f);

        HorizontalMovement = Speed;
        EAnim.SetBool("caminar",true);
        WaitTime=false;
    }

    private IEnumerator Turn(){
        WaitTime = true;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
        MoveToRigth=!MoveToRigth;
        HorizontalMovement=0f;
        EAnim.SetBool("caminar",false);
        yield return new WaitForSecondsRealtime(2f);
        WaitTime = false;
        Speed *= -1;
        HorizontalMovement = Speed;
        EAnim.SetBool("caminar",true);
        yield return null;
    }

    private void OnCollisionEnter2D(Collision2D collider){
        if(collider.gameObject.CompareTag("Player")){
            Player.GetComponent<PlayerController>().Hit(collider.GetContact(0).normal);
        }
    }

    void OnDrawGizmosSelected(){
        //Gizmos del Rango de ataque
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
        //Gizmos del Raycast
        Gizmos.color = Color.red;
        Gizmos.DrawLine(ControllerObj.position, ControllerObj.position + Vector3.down * DetectedDistance);
    }
}
