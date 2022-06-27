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
    private Rigidbody2D rb2d;
    private PlayerController PlayerC;

    void Start(){
      percentage = Health;
      PlayerAnim = GetComponent<Animator>();
      rb2d = GetComponent<Rigidbody2D>();
      PlayerC = GetComponent<PlayerController>();
    }

    void Update(){
        ImgHealth.fillAmount = Health/percentage;
        if(ImgHealth.fillAmount<=0.5f&&ImgHealth.fillAmount>0.3f){
            ImgHealth.color = Color.yellow;
        }else if(ImgHealth.fillAmount<=0.3f){
            ImgHealth.color = Color.red;
        }
        
        if(Health<=0){
            Destroy(gameObject);
        }
    }

    public void DamageReceived(float damage){
        if(!Protection) {
        Health -= damage;
        Protection = true;
        StartCoroutine(TimeProtection());
        print("vida: "+Health);
        }
    }

    IEnumerator TimeProtection(){
        PlayerAnim.SetBool("daño",true);
        Physics2D.IgnoreLayerCollision(3,7, true);

        yield return new WaitForSecondsRealtime(1f);
        
        Physics2D.IgnoreLayerCollision(3,7, false);
        PlayerAnim.SetBool("daño",false);
        Protection = false;
    }
}
