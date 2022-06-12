using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstructionArea : MonoBehaviour {

    private SpriteRenderer timerSpriteRenderer;
    private Vector2 tempVector;
    private float makeInstructionTimer;
    private float giveInstructionTimer;
    private float minimumMovementTime;
    private bool instructionCompleted;

    private Transform ball;
    private Transform instruction;

    private InstructionType instructionType;

    public enum InstructionType {
        Up,
        RightUp,
        Right,
        RightDown,
        Down,
        LeftDown,
        Left,
        LeftUp
    }

    private void Awake() {
        giveInstructionTimer = 0f;
        makeInstructionTimer = 0f;
        minimumMovementTime = 0.35f;

        timerSpriteRenderer = transform.Find("Timer/Time").GetComponent<SpriteRenderer>();
        tempVector = timerSpriteRenderer.size;

        ball = transform.GetComponentInChildren<BallMovement>().transform;
        instruction = transform.Find("Canvas/Instruction").transform;
    }

    private void Update() {
        giveInstructionTimer -= Time.deltaTime;

        if (giveInstructionTimer < 0 && instructionCompleted) {
            GiveInstruction();
            giveInstructionTimer = 4f;
        }

        if (!instructionCompleted) {
            switch (instructionType) {
                case InstructionType.Left:
                                            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                                                minimumMovementTime -= Time.deltaTime;
                                            }
                                            break;

                case InstructionType.Right:
                                            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                                                minimumMovementTime -= Time.deltaTime;
                                            }
                                            break;

                case InstructionType.Up:
                                            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
                                                minimumMovementTime -= Time.deltaTime;
                                            }
                                            break;

                case InstructionType.Down:
                                            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
                                                minimumMovementTime -= Time.deltaTime;
                                            }
                                            break;

                case InstructionType.RightUp:
                                            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightArrow))) {
                                                minimumMovementTime -= Time.deltaTime;
                                            }
                                            break;

                case InstructionType.RightDown:
                                            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightArrow))) {
                                                minimumMovementTime -= Time.deltaTime;
                                            }
                                            break;

                case InstructionType.LeftUp:
                                            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow))) {
                                                minimumMovementTime -= Time.deltaTime;
                                            }
                                            break;

                case InstructionType.LeftDown:
                                            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.LeftArrow))) {
                                                minimumMovementTime -= Time.deltaTime;
                                            }
                                            break;
            }

            instruction.Find("InstructionArrow").GetComponent<Image>().fillAmount = (0.35f - minimumMovementTime) / 0.35f;
        }

        if (minimumMovementTime < 0) {
            InstructionCompleted();
        }


        makeInstructionTimer += Time.deltaTime;
        tempVector = new Vector2(makeInstructionTimer, timerSpriteRenderer.size.y);
        if (makeInstructionTimer > 4f) {
            makeInstructionTimer = 0f;
            tempVector = new Vector2(makeInstructionTimer, timerSpriteRenderer.size.y);
            
            //if didnt complete instruction --> death
            if (!instructionCompleted) {
                ball = transform.GetComponentInChildren<BallMovement>().transform;
                Destroy(ball.gameObject);
                instructionCompleted = true;
                giveInstructionTimer = 0f;
                makeInstructionTimer = 0f;
                minimumMovementTime = 0.35f;
            }
        }

        timerSpriteRenderer.size = tempVector;
    }

    private void GiveInstruction() {
        int randomNumber = Random.Range(0, 7);
        instructionCompleted = false;
        instructionType = (InstructionType)randomNumber;
        instruction.gameObject.SetActive(true);
        instruction.transform.rotation = Quaternion.AngleAxis(-45 * randomNumber, Vector3.forward);
        giveInstructionTimer = 4f;
        makeInstructionTimer = 0f;
    }

    private void InstructionCompleted() {
        minimumMovementTime = 0.35f;
        instructionCompleted = true;
        instruction.gameObject.SetActive(false);
    }

    private void OnEnable() {
        makeInstructionTimer = 0f;
        InstructionCompleted();
        GiveInstruction();
    }
}
