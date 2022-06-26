using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricEnemyController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField]private float Speed;
    
    [Header("Ataque")]
    [SerializeField]private float AttackRange;
    [SerializeField]private LayerMask PlayerMask;
    private bool NearPlayer;
    private Animator EAnim;
    private GameObject Player;
    private HealthPlayer PlayerScript;
    void Start()
    {
        EAnim = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerScript = Player.GetComponent<HealthPlayer>();
    }

    void Update()
    {
        
    }

    void FixedUpdate(){
        NearPlayer = Physics2D.OverlapCircle(transform.position,AttackRange,PlayerMask);

        if(NearPlayer){
            Attack();
        }
    }

    private void Attack(){
        EAnim.SetBool("ataque",true);
        PlayerScript.DamageReceived(1);
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
