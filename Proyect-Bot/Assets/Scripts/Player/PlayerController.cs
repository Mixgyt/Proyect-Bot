using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   private Rigidbody2D rb2d;
   [System.NonSerialized]public bool CanMove=true;

   [Header("Movimiento")]
   [SerializeField] private float SpeedMovement;
   private float movimientoHorizontal = 0f;
   [Range(0,0.3f)][SerializeField] private float smoothMovement;
   private Vector3 speed = Vector3.zero;
   [System.NonSerialized]public bool lookRigth = true;
   [SerializeField]private Vector2 HitForce;

      [Header("Salto")]
    [SerializeField]private float JumpForce;
    [SerializeField]private LayerMask WhatIsGround;
    [SerializeField]private Transform GroundController;
    [SerializeField]private Vector3 BoxDimensions;
    [SerializeField]private bool inGround;
    private bool jump=false;

      [Header("Vida")]
    [SerializeField]private double Health;
   
   //Variables de Animación
   private Animator botAnim;

    //Inicio del Script
   void Start(){
       rb2d = GetComponent<Rigidbody2D>();
       botAnim = GetComponent<Animator>();
   }

    //Funcion repetida cada frame
   void Update()
   {
       movimientoHorizontal = Input.GetAxisRaw("Horizontal")*SpeedMovement;

        if(Input.GetButtonDown("Jump")&&inGround&&CanMove)
        {
          jump=true;
        }
   }

    //Funcion que se repite consecutivamente sin importar los frames
   void FixedUpdate(){
      inGround = Physics2D.OverlapBox(GroundController.position, BoxDimensions, 0f, WhatIsGround);
      //Mover
      if(CanMove){
      Movement(movimientoHorizontal * Time.fixedDeltaTime, jump);
      }
      if(inGround){ botAnim.SetBool("salto",false); }
   }

    //Funcion de movimiento del personaje
   private void Movement(float move, bool saltar){
       Vector3 velocidadObjetivo = new Vector3(move, rb2d.velocity.y);
       rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity, velocidadObjetivo, ref speed, smoothMovement);

        //Aqui se activa la animación si el personaje se mueve
       if(movimientoHorizontal!=0){ botAnim.SetBool("caminando",true); } 
       else { botAnim.SetBool("caminando",false); }
         //Aqui se gira si el personaje no mira donde camina
       if(move>0 && !lookRigth){
           Turn();
       }else if(move<0 && lookRigth){
           Turn();
       }

      if(saltar&&inGround){
        inGround=false; 
        jump=false; 
        rb2d.AddForce(new Vector2(0f, JumpForce)); 
        botAnim.SetBool("salto",true);
        }
       
       botAnim.SetBool("enSuelo",inGround);
       botAnim.SetFloat("velocidadY",rb2d.velocity.y);
   }

    //Funcion para girar el personaje si no esta viendo donde camina
   private void Turn(){
       lookRigth = !lookRigth;
       Vector3 scale = transform.localScale;
       scale.x *= -1;
       transform.localScale = scale;
   }

   public void Hit(Vector2 pointHit){
      rb2d.velocity = new Vector2(-HitForce.x * pointHit.x, HitForce.y);
   }

   void OnDrawGizmosSelected(){
       Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(GroundController.position, BoxDimensions);
   }
}
