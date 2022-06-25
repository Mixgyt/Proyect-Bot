using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPlayer : MonoBehaviour
{
    public float Health;
    private float percentage;
    public Image ImgHealth;

    void Start(){
      percentage = Health;
    }

    void Update(){
        ImgHealth.fillAmount = Health/percentage;
        if(Health<=0){
            Destroy(gameObject);
        }
    }

    public void DamageReceived(float damage){
        Health -= damage;
        print("vida"+Health);
    }
}
