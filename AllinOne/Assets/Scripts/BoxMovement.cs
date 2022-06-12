using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxMovement : MonoBehaviour {

    private Transform boxPrefab;
    private Vector3 spawnPosition;
    private Transform parent;
    private float movementSpeed;
    private Rigidbody2D rb;

    private bool isQuitting = false;

    private void Awake() {
        boxPrefab = Resources.Load<Transform>("Prefabs/pfBox");
        rb = GetComponent<Rigidbody2D>();
        spawnPosition = transform.position;
        movementSpeed = 3f;
        rb.velocity = Vector2.left * movementSpeed;
        parent = transform.parent;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.tag == "End") {
            transform.position = spawnPosition;
            rb.velocity = Vector2.left * movementSpeed;
        }
    }

    private void OnEnable() {
        isQuitting = false;
        transform.position = spawnPosition;
        rb.velocity = Vector2.left * movementSpeed;
    }

    private void OnDisable() {
        if (!transform.gameObject.activeInHierarchy) {
            isQuitting = true;
        }
    }

    private void OnApplicationQuit() {
        isQuitting = true;
    }

    private void OnDestroy() {
        if (!isQuitting) {
            Instantiate(boxPrefab, spawnPosition, Quaternion.identity, parent);
        }
    }

}
