using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   private Rigidbody2D rb2d;

   [Header("Movimiento")]
   [SerializeField] private float velocidadDeMovimiento;
   private float movimientoHorizontal = 0f;
   [Range(0,0.3f)][SerializeField] private float suavizadoDeMovimiento;
   private Vector3 velocidad = Vector3.zero;
   private bool mirandoDerecha = true;

      [Header("Jump")]
    [SerializeField] private float JumpForce;
    [SerializeField]private LayerMask WhatIsGround;
    [SerializeField]private Transform GroundController;
    [SerializeField]private Vector3 BoxDimensions;
    [SerializeField]private bool inGround;
    private bool salto=false;
   
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
       movimientoHorizontal = Input.GetAxisRaw("Horizontal")*velocidadDeMovimiento;

        if(Input.GetButtonDown("Jump"))
        {
          salto=true;
        }
   }

    //Funcion que se repite consecutivamente sin importar los frames
   void FixedUpdate(){
       Mover(movimientoHorizontal * Time.fixedDeltaTime, salto);

        inGround = Physics2D.OverlapBox(GroundController.position, BoxDimensions, 0f, WhatIsGround);
      //Mover
      Mover(movimientoHorizontal * Time.fixedDeltaTime, salto);

      salto=false;
      if(inGround){botAnim.SetBool("salto",false);}
   }

    //Funcion de movimiento del personaje
   private void Mover(float movimiento, bool saltar){
       Vector3 velocidadObjetivo = new Vector3(movimiento, rb2d.velocity.y);
       rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity, velocidadObjetivo, ref velocidad, suavizadoDeMovimiento);

        //Aqui se activa la animación si el personaje se mueve
       if(movimientoHorizontal!=0){ botAnim.SetBool("walking",true); } 
       else { botAnim.SetBool("walking",false); }

       if(movimiento>0 && !mirandoDerecha){
           Girar();
       }else if(movimiento<0 && mirandoDerecha){
           Girar();
       }

      if(saltar&&inGround){rb2d.AddForce(new Vector2(0f, JumpForce/1.5f));} 

   }

    //Funcion para girar el personaje si no esta viendo donde camina
   private void Girar(){
       mirandoDerecha = !mirandoDerecha;
       Vector3 escala = transform.localScale;
       escala.x *= -1;
       transform.localScale = escala;
   }

   void OnDrawGizmos(){
       Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(GroundController.position, BoxDimensions);
   }
}
