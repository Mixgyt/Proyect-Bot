using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScript : MonoBehaviour
{
    [SerializeField] private Vector2 VelocidadMovimiento;
    private Vector2 offset;
    private Material material;
    private Rigidbody2D Prb2d;

    private void Awake(){
        material = GetComponent<SpriteRenderer>().material;
        Prb2d = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    void Update(){
        offset = (Prb2d.velocity.x * 0.1f) * VelocidadMovimiento*Time.deltaTime;
        material.mainTextureOffset += offset;
    }
}
