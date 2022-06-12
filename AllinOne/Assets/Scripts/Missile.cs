using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {

    private Transform missilePrefab;
    private Transform target;
    private Vector3 spawnPosition;
    private Transform parent;
    private float movementSpeed = 1f;
    private float rotationSpeed = 10f;
    private Vector3 targetPosition;

    private bool isQuitting = false;

    private void Awake() {
        spawnPosition = transform.position;
        missilePrefab = Resources.Load<Transform>("Prefabs/pfMissile");
        parent = transform.parent;
    }

    private void Start() {
        target = GameObject.FindGameObjectWithTag("MissileBall").transform;
        targetPosition = target.position;
        targetPosition.z = 0f;
    }

    private void Update() {
        targetPosition = target.position;
        targetPosition.z = 0f;

        var step = movementSpeed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        Vector3 lookDirection = targetPosition - transform.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        var desiredRotation = Quaternion.Euler(new Vector3(0, 0, angle));

        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotationSpeed);
    }

    private void OnEnable() {
        isQuitting = false;
        transform.position = spawnPosition;
        transform.rotation = Quaternion.identity;
        target = GameObject.FindGameObjectWithTag("MissileBall").transform;
        GetComponent<TrailRenderer>().Clear();
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
            Instantiate(missilePrefab, spawnPosition, Quaternion.identity, parent);
        }
    }
}
