using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   private Rigidbody2D rb2d;

   [Header("Movimiento")]
   [SerializeField] private float SpeedMovement;
   private float movimientoHorizontal = 0f;
   [Range(0,0.3f)][SerializeField] private float smoothMovement;
   private Vector3 speed = Vector3.zero;
   [System.NonSerialized]public bool lookRigth = true;

      [Header("Salto")]
    [SerializeField]private float JumpForce;
    [SerializeField]private LayerMask WhatIsGround;
    [SerializeField]private Transform GroundController;
    [SerializeField]private Vector3 BoxDimensions;
    [SerializeField]private bool inGround;
    private bool salto=false;

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

        if(Input.GetButtonDown("Jump"))
        {
          salto=true;
        }

        if(Health<=0){ GameOver(); }
   }

    //Funcion que se repite consecutivamente sin importar los frames
   void FixedUpdate(){
      inGround = Physics2D.OverlapBox(GroundController.position, BoxDimensions, 0f, WhatIsGround);
      //Mover
      Movement(movimientoHorizontal * Time.fixedDeltaTime, salto);

      if(inGround){botAnim.SetBool("salto",false);}
   }

    //Funcion de movimiento del personaje
   private void Movement(float movimiento, bool saltar){
       Vector3 velocidadObjetivo = new Vector3(movimiento, rb2d.velocity.y);
       rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity, velocidadObjetivo, ref speed, smoothMovement);

        //Aqui se activa la animación si el personaje se mueve
       if(movimientoHorizontal!=0){ botAnim.SetBool("walking",true); } 
       else { botAnim.SetBool("walking",false); }
         //Aqui se gira si el personaje no mira donde camina
       if(movimiento>0 && !lookRigth){
           Turn();
       }else if(movimiento<0 && lookRigth){
           Turn();
       }

      if(saltar&&inGround){salto=false; rb2d.AddForce(new Vector2(0f, JumpForce));} 

   }

    //Funcion para girar el personaje si no esta viendo donde camina
   private void Turn(){
       lookRigth = !lookRigth;
       Vector3 escala = transform.localScale;
       escala.x *= -1;
       transform.localScale = escala;
   }

   public void DamageReceived(double damage){
    Health -= damage;
    print("Tu vida es: "+Health);
   }

   private void GameOver(){
    print("Has Muerto");
   }

   void OnDrawGizmosSelected(){
       Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(GroundController.position, BoxDimensions);
   }
}
