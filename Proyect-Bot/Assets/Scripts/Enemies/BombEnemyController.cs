using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEnemyController : MonoBehaviour
{
    [Header("Detección")]
    [SerializeField]private float range;
    [SerializeField]private LayerMask playerMask;
    private GameObject Player;
    private bool NearPlayer;

    void Start()
    {
     Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update(){
     NearPlayer = Physics2D.OverlapCircle(transform.position,range,playerMask);
     if(NearPlayer){
      FollowPlayer();
     }
    }

    private void FollowPlayer(){
        if(transform.position.x < Player.transform.position.x){

        }
    }

    void OnDrawGizmosSelected(){
        Gizmos.color=Color.yellow;
        Gizmos.DrawWireSphere(transform.position,range);
    }
}
