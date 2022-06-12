using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallJump : MonoBehaviour {

    private Transform jumperPlayerPrefab;
    private Vector3 spawnPosition;
    private Transform parent;
    private Rigidbody2D rb;

    private Vector3 originalCameraPosition;

    private Transform areaDeathEffect;

    private bool isQuitting = false;

    private void Awake() {
        jumperPlayerPrefab = Resources.Load<Transform>("Prefabs/pfJumperBall");
        rb = GetComponent<Rigidbody2D>();
        spawnPosition = transform.position;
        parent = transform.parent;

        areaDeathEffect = transform.parent.Find("AreaDeathEffect");

        originalCameraPosition = Camera.main.transform.position;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }
    }

    private void Jump() {
        rb.velocity = new Vector2(0f, 2.5f);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.tag == "RedWall") {
            Destroy(gameObject);
        }

        else if (collision.collider.tag == "Box") {
            Destroy(collision.collider.gameObject);
            Destroy(gameObject);
        }
    }

    private IEnumerator Shake() {
        float elapsed = 0.0f;

        while (elapsed < 0.15f) {
            float x = Random.Range(-1f, 1f) * 0.3f;
            float y = Random.Range(-1f, 1f) * 0.3f;

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
        areaDeathEffect.GetComponent<Animator>().Play("AreaDeathIdle");
        GetComponent<Animator>().Play("PlayerDeathIdle");
        Camera.main.transform.position = originalCameraPosition;
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
            Transform newBall = Instantiate(jumperPlayerPrefab, spawnPosition, Quaternion.identity, parent);
            newBall.GetComponent<Animator>().Play("PlayerDeathEffect", -1, 0);
            areaDeathEffect.GetComponent<Animator>().Play("AreaDeathEffect", -1, 0);
            newBall.GetComponent<BallJump>().CameraShake();

            if (SceneManager.GetActiveScene().name == "GameScene") {
                GameManager.LoseLife();
            }
        }
    }
}
