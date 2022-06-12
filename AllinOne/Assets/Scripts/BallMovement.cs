using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallMovement : MonoBehaviour {

    private Transform movingBallPrefab;
    private Vector3 spawnPosition;
    private Transform parent;
    private Rigidbody2D rb;

    private float xInput;
    private float yInput;
    private Vector2 movementVector;
    private float movementSpeed = 2.5f;

    private Vector3 originalCameraPosition;

    public MovingBallType movingBallType;

    public enum MovingBallType {
        RunningAwayFromMissile,
        Free
    }

    private Transform areaDeathEffect;

    private bool isQuitting = false;

    private void Awake() {
        movingBallPrefab = Resources.Load<Transform>("Prefabs/pfMovingBall");
        spawnPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        parent = transform.parent;

        areaDeathEffect = transform.parent.Find("AreaDeathEffect");

        originalCameraPosition = Camera.main.transform.position;
    }

    private void Update() {
        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");
        Move();
    }

    private void Move() {
        movementVector = new Vector2(xInput, yInput);
        rb.velocity = movementVector * movementSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.tag == "RedWall") {
            Destroy(gameObject);
        }

        else if (collision.collider.tag == "Missile") {
            Destroy(collision.collider.gameObject);
            Destroy(gameObject);
        }
    }

    private IEnumerator Shake() {
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
            Transform newBall = Instantiate(movingBallPrefab, spawnPosition, Quaternion.identity, parent);
            newBall.GetComponent<Animator>().Play("PlayerDeathEffect", -1, 0);
            areaDeathEffect.GetComponent<Animator>().Play("AreaDeathEffect", -1, 0);
            newBall.GetComponent<BallMovement>().CameraShake();

            if (SceneManager.GetActiveScene().name == "GameScene") {
                GameManager.LoseLife();
            }

            if (movingBallType == MovingBallType.RunningAwayFromMissile) {
                newBall.tag = "MissileBall";
                newBall.GetComponent<BallMovement>().movingBallType = MovingBallType.RunningAwayFromMissile;
            }
        }
    }
}
