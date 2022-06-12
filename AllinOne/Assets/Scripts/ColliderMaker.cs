using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ColliderMaker : MonoBehaviour {

    private EdgeCollider2D edgeCollider;
    private SpriteRenderer spriteRenderer;

    private Vector2[] points;
    private Vector2 pointA;
    private Vector2 pointB;
    private Vector2 pointC;
    private Vector2 pointD;
    private Vector2 pointE;

    private Vector2 spriteOffsets;

    private float tempWidthValue;
    private float tempHeightValue;

    private void Awake() {
        edgeCollider = GetComponent<EdgeCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        points = new Vector2[5];

        tempWidthValue = spriteRenderer.size.x;
        tempHeightValue = spriteRenderer.size.y;

    }

    private void Update() {
        if (spriteRenderer.size.x != tempWidthValue || spriteRenderer.size.y != tempHeightValue) {
            SetEdgeColliderPoints();
        }
    }

    private void SetEdgeColliderPoints() {
        spriteOffsets = new Vector2(0.1f, 0.1f);
        pointA = new Vector2(-(spriteRenderer.size.x / 2 - spriteOffsets.x),    spriteRenderer.size.y / 2 - spriteOffsets.y);
        pointB = new Vector2(-(spriteRenderer.size.x / 2 - spriteOffsets.x),    -(spriteRenderer.size.y / 2 - spriteOffsets.y));
        pointC = new Vector2(spriteRenderer.size.x / 2 - spriteOffsets.x,       -(spriteRenderer.size.y / 2 - spriteOffsets.y));
        pointD = new Vector2(spriteRenderer.size.x / 2 - spriteOffsets.x,       spriteRenderer.size.y / 2 - spriteOffsets.y);
        pointE = new Vector2(-(spriteRenderer.size.x / 2 - spriteOffsets.x),    spriteRenderer.size.y / 2 - spriteOffsets.y);

        points.SetValue(pointA, 0);
        points.SetValue(pointB, 1);
        points.SetValue(pointC, 2);
        points.SetValue(pointD, 3);
        points.SetValue(pointE, 4);

        edgeCollider.points = points;

        tempWidthValue = spriteRenderer.size.x;
        tempHeightValue = spriteRenderer.size.y;
    }
}
