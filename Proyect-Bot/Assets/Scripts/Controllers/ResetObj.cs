using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObj : MonoBehaviour
{
    [SerializeField]private GameObject TPoint;

    private void OnTriggerEnter2D(Collider2D collision){
        bool player = collision.CompareTag("Player");
        if(player){collision.transform.position = TPoint.transform.position; }
    }
}
