using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTeleport : MonoBehaviour {

    private Transform wall;
    private SpriteRenderer wallSpriteRenderer;

    private void Awake() {
        wall = transform.parent.Find("Wall");
        wallSpriteRenderer = wall.GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.tag == "GreenWall") {
            ContactPoint2D contact = collision.contacts[0];
            Vector3 contactPosition = contact.point;

            if (Mathf.Abs((wall.transform.position.x - contactPosition.x) / wallSpriteRenderer.size.x) >= Mathf.Abs((wall.transform.position.y - contactPosition.y) / wallSpriteRenderer.size.y)) {
                if (contactPosition.x > wall.transform.position.x) {
                    transform.position = new Vector2(contactPosition.x - (2 * wallSpriteRenderer.size.x - 0.5f) + 0.2f, contactPosition.y);
                    //teleport right to left
                }
                else {
                    transform.position = new Vector2(contactPosition.x + (2 * wallSpriteRenderer.size.x - 0.5f) - 0.2f, contactPosition.y);
                    //teleport right to left
                }
            }
            else {
                if (contactPosition.y > wall.transform.position.y) {
                    transform.position = new Vector2(contactPosition.x, contactPosition.y - (2 * wallSpriteRenderer.size.y - 0.5f) + 0.2f);
                    //teleport down to up
                }
                else {
                    transform.position = new Vector2(contactPosition.x, contactPosition.y + (2 * wallSpriteRenderer.size.y - 0.5f) - 0.2f);
                    //teleport up to down
                }
            }
        }
    }
}
