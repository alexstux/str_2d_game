using System;
using System.Collections.Generic;

using UnityEngine;

public class BoxSpawner : MonoBehaviour {
    public GameObject box;
    private const float L1 = 3f;
    private const float L2 = 3f;
    private float realityL1 = L1;
    private float realityL2 = L2;
    private float oldScalePlayerX;
    private float oldScalePlayerY;
    private float oldScaleBoxX;
    private float oldScaleBoxY;
    private Pair<float, float> realityScale;
    private const int numberOfPairBoxes = 20;
    private float visibleLength;
    private float realityVisibleLength;
    private int countOfVisibleBoxUp = 0;
    private int countOfVisibleBoxDown = numberOfPairBoxes - 1;
    private Rigidbody2D player;
    private List<Pair<GameObject, GameObject>> boxes;
    
    void Start() {
        player = GetComponent<Rigidbody2D>();
        boxes = new List<Pair<GameObject, GameObject>>();
        realityScale = new Pair<float, float>();
        for (int i = (int)(numberOfPairBoxes / -2f);
            i < numberOfPairBoxes / 2f;
            i++) {
            boxes.Add(new Pair<GameObject, GameObject>(
                Instantiate(
                    box,
                    Vector2.zero
                    + Vector2.up * L2 * i
                    + Vector2.right * L1,
                    Quaternion.identity
                ),
                Instantiate(
                    box,
                    Vector2.zero
                    + Vector2.up * L2 * i
                    + Vector2.left * L1,
                    Quaternion.identity
                )
            ));
            oldScalePlayerX = transform.localScale.x;
            oldScalePlayerY = transform.localScale.y;
            oldScaleBoxX = box.transform.localScale.x;
            oldScaleBoxY = box.transform.localScale.y;
            realityVisibleLength = visibleLength =
                Math.Abs(boxes[0].first.transform.position.y
                - player.position.y);
        }
    }

    void Update() {
        SetScale();
        if (PlayerProperties.v != Vector2.zero) {
            if (PlayerProperties.v.Cos(Vector2.up) > 0) {
                if (Math.Abs(boxes[countOfVisibleBoxUp].first.transform.position.y
                    - player.position.y) > realityVisibleLength) {
                    boxes[countOfVisibleBoxUp].first.transform.position = new Vector3(
                        realityL1,
                        boxes[countOfVisibleBoxDown].first.transform.position.y + realityL2,
                        boxes[countOfVisibleBoxUp].first.transform.position.z
                    );
                    boxes[countOfVisibleBoxUp].second.transform.position = new Vector3(
                        -realityL1,
                        boxes[countOfVisibleBoxDown].second.transform.position.y + realityL2,
                        boxes[countOfVisibleBoxUp].second.transform.position.z
                    );

                    countOfVisibleBoxUp = (countOfVisibleBoxUp + 1) % numberOfPairBoxes;
                    countOfVisibleBoxDown = (countOfVisibleBoxDown + 1) % numberOfPairBoxes;
                }
            } else {
                if (Math.Abs(boxes[countOfVisibleBoxDown].first.transform.position.y
                    - player.position.y) > realityVisibleLength) {
                    boxes[countOfVisibleBoxDown].first.transform.position = new Vector3(
                        realityL1,
                        boxes[countOfVisibleBoxUp].first.transform.position.y - realityL2,
                        boxes[countOfVisibleBoxDown].first.transform.position.z
                    );
                    boxes[countOfVisibleBoxDown].second.transform.position = new Vector3(
                        -realityL1,
                        boxes[countOfVisibleBoxUp].second.transform.position.y - realityL2,
                        boxes[countOfVisibleBoxDown].second.transform.position.z
                    );

                    if (countOfVisibleBoxDown == 0) {
                        countOfVisibleBoxDown = numberOfPairBoxes - 1;
                        countOfVisibleBoxUp = 0;
                    } else if (countOfVisibleBoxUp == 0) {
                        countOfVisibleBoxUp = numberOfPairBoxes - 1;
                        --countOfVisibleBoxDown;
                    } else {
                        --countOfVisibleBoxDown;
                        --countOfVisibleBoxUp;
                    }
                }
            }
        }
    }

    void SetScale() {
        // Scale space
        realityScale = Formulas.Scale(new Pair<float, float>(L1, L2));
        realityL1 = realityScale.first;
        realityL2 = realityScale.second;
        realityScale = Formulas.Scale(new Pair<float, float>(0f, visibleLength));
        realityVisibleLength = realityScale.second;

        // Scale all boxes
        realityScale = 
            Formulas.Scale(new Pair<float, float>(
                oldScaleBoxX,
                oldScaleBoxY
            ));
        foreach (var box in boxes) {
            box.first.transform.localScale = box.second.transform.localScale =
                new Vector3(
                    realityScale.first,
                    realityScale.second,
                    box.first.transform.localScale.z
                );
        }

        // Scale player
        realityScale =
            Formulas.Scale(new Pair<float, float>(
                oldScalePlayerX,
                oldScalePlayerY
            ));
        transform.localScale =
                new Vector3(
                    realityScale.first,
                    realityScale.second,
                    transform.localScale.z
                );
    }
}
