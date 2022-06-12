using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PingPongBall : MonoBehaviour {

    private Transform pingPongBallPrefab;
    private Vector3 spawnPosition;
    private Transform parent;
    private Rigidbody2D rb;
    private float difference;
    private float movementSpeed;
    private Vector2 force;

    private Transform areaDeathEffect;

    private Vector3 originalCameraPosition;

    private bool isQuitting = false;

    private void Awake() {
        pingPongBallPrefab = Resources.Load<Transform>("Prefabs/pfPingPongBall");
        spawnPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        movementSpeed = 75;
        parent = transform.parent;

        areaDeathEffect = transform.parent.Find("AreaDeathEffect");

        originalCameraPosition = Camera.main.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.tag == "PingPongRacket") {
            rb.velocity = Vector2.zero;
            difference = collision.collider.transform.position.x - transform.position.x;
            force = new Vector2(-difference, 2);
            force.Normalize();
            rb.AddForce(force * movementSpeed);
        }

        else if (collision.collider.tag == "RedWall") {
            Destroy(gameObject);
        }
    }

    private IEnumerator Shake() {
        originalCameraPosition = Camera.main.transform.position;

        float elapsed = 0.0f;

        while (elapsed < 0.15f) {
            float x = Random.Range(-1f, 1f) * 0.4f;
            float y = Random.Range(-1f, 1f) * 0.4f;

            Camera.main.transform.position = new Vector3(x, y, originalCameraPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        Camera.main.transform.position = originalCameraPosition;
    }

    public void CameraShake() {
        StartCoroutine(Shake());
    }

    private void OnEnable() {
        isQuitting = false;
        transform.position = spawnPosition;
        areaDeathEffect.GetComponent<Animator>().Play("AreaDeathIdle", -1, 0);
        GetComponent<Animator>().Play("PlayerDeathIdle", -1, 0);
        Camera.main.transform.position = originalCameraPosition;

        force = new Vector2(-4, 4);
        force.Normalize();
        rb.AddForce(force * movementSpeed);
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
            Transform newBall = Instantiate(pingPongBallPrefab, spawnPosition, Quaternion.identity, parent);
            newBall.GetComponent<Animator>().Play("PlayerDeathEffect", -1, 0);
            areaDeathEffect.GetComponent<Animator>().Play("AreaDeathEffect", -1, 0);
            newBall.GetComponent<PingPongBall>().CameraShake();

            if (SceneManager.GetActiveScene().name == "GameScene") {
                GameManager.LoseLife();
            }
        }
    }

}
