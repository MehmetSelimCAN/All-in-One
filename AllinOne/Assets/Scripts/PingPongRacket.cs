using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongRacket : MonoBehaviour {

    private Rigidbody2D rb;
    private float xInput;
    private float movementSpeed;
    private Vector3 spawnPosition;

    private Vector3 originalCameraPosition;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        movementSpeed = 2f;
        spawnPosition = transform.position;

        originalCameraPosition = Camera.main.transform.position;
    }

    private void Update() {
        xInput = Input.GetAxisRaw("Horizontal");
        Move();
    }

    private void Move() {
        rb.velocity = new Vector2(xInput * movementSpeed, 0f);
    }

    private void OnEnable() {
        transform.position = spawnPosition;
        Camera.main.transform.position = originalCameraPosition;
    }
}
