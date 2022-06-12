using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGreenArea : MonoBehaviour {

    private float timer = 1.5f;
    private Transform wall;

    private void Awake() {
        wall = transform.Find("Wall");
    }

    private void Update() {
        timer -= Time.deltaTime;
        
        if (timer < 0f) {
            ChangeWall();
            timer = 2f;
        }
    }

    private void ChangeWall() {
        if (wall.tag == "GreenWall") {
            wall.tag = "RedWall";
            wall.GetComponent<SpriteRenderer>().color = new Color32(221, 16, 3, 255);
        }
        else {
            wall.tag = "GreenWall";
            wall.GetComponent<SpriteRenderer>().color = new Color32(61, 207, 86, 255);
        }
    }
}
