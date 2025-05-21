using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OyuncuHareketi : MonoBehaviour
{
    Rigidbody2D rgb;
    Vector2 velocity;

    float speedAmount = 5f;

    void Start()
    {
        rgb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // X ve Y ekseninde giriş oku (yani yukarı, aşağı, sağ, sol)
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        velocity = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        // Fizik motoru üzerinden pozisyon güncellemesi
        rgb.MovePosition(rgb.position + velocity * speedAmount * Time.fixedDeltaTime);
    }
}
