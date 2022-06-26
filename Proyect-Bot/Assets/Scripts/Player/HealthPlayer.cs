using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPlayer : MonoBehaviour
{
    public float Health;
    private float percentage;
    private bool Protection=false;
    public Image ImgHealth;
    private Animator PlayerAnim;

    void Start(){
      percentage = Health;
      PlayerAnim = GetComponent<Animator>();
    }

    void Update(){
        ImgHealth.fillAmount = Health/percentage;
        if(Health<=0){
            Destroy(gameObject);
        }
    }

    public void DamageReceived(float damage){
        if(!Protection) {
        Health -= damage;
        PlayerAnim.SetBool("daño",true);
        Protection = true;
        print("vida: "+Health);
        }
    }

    public void ProtectionStop(){
        PlayerAnim.SetBool("daño",false);
        Protection = false;
    }
}
