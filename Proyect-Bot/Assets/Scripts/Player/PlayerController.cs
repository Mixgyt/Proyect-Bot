using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   private Rigidbody2D rb2d;

   [Header("Movimiento")]
   private float movimientoHorizontal = 0f;
   [SerializeField] private float velocidadDeMovimiento;
   [Range(0,0.3f)][SerializeField] private float suavizadoDeMovimiento;
   private Vector3 velocidad = Vector3.zero;
   private bool mirandoDerecha = true;

    //Inicio del Script
   void Start(){
       rb2d = GetComponent<Rigidbody2D>();
   }

    //Funcion repetida cada frame
   void Update(){
       movimientoHorizontal = Input.GetAxisRaw("Horizontal")*velocidadDeMovimiento;
   }

    //Funcion que se repite consecutivamente sin importar los frames
   void FixedUpdate(){
       Mover(movimientoHorizontal * Time.fixedDeltaTime);
   }

    //Funcion de movimiento del personaje
   private void Mover(float movimiento){
       Vector3 velocidadObjetivo = new Vector3(movimiento, rb2d.velocity.y);
       rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity, velocidadObjetivo, ref velocidad, suavizadoDeMovimiento);

       if(movimiento>0 && !mirandoDerecha){
           Girar();
       }else if(movimiento<0 && mirandoDerecha){
           Girar();
       }
   }

    //Funcion para girar el personaje si no esta viendo donde camina
   private void Girar(){
       mirandoDerecha = !mirandoDerecha;
       Vector3 escala = transform.localScale;
       escala.x *= -1;
       transform.localScale = escala;
   }
}
