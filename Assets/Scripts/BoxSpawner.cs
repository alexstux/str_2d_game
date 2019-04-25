using System;
using System.Collections.Generic;

using UnityEngine;

public class BoxSpawner : MonoBehaviour {
    public GameObject box;
    private const float L = 3f;
    private const int numberOfPairBoxes = 100;
    private float visibleLength;
    private int countOfVisibleBoxUp = 0;
    private int countOfVisibleBoxDown = numberOfPairBoxes - 1;
    private Rigidbody2D player;
    private List<Pair<GameObject, GameObject>> boxes;
    
    void Start() {
        player = GetComponent<Rigidbody2D>();
        boxes = new List<Pair<GameObject, GameObject>>();
        for (int i = (int)(numberOfPairBoxes / -2f);
            i < numberOfPairBoxes / 2f;
            i++) {
            boxes.Add(new Pair<GameObject, GameObject>(
                Instantiate(
                    box,
                    Vector2.zero
                    + Vector2.up * L * i
                    + Vector2.right * L,
                    Quaternion.identity
                ),
                Instantiate(
                    box,
                    Vector2.zero
                    + Vector2.up * L * i
                    + Vector2.left * L,
                    Quaternion.identity
                )
            ));
            visibleLength =
                Math.Abs(boxes[0].first.transform.position.y
                - player.position.y);
        }
    }

    void Update() {
        if (PlayerProperties.v != Vector2.zero) {
            if (PlayerProperties.v.Cos(Vector2.up) > 0) {
                if (Math.Abs(boxes[countOfVisibleBoxUp].first.transform.position.y
                    - player.position.y) > visibleLength) {
                    boxes[countOfVisibleBoxUp].first.transform.position = new Vector3(
                        boxes[countOfVisibleBoxUp].first.transform.position.x,
                        boxes[countOfVisibleBoxDown].first.transform.position.y + L,
                        boxes[countOfVisibleBoxUp].first.transform.position.z
                    );
                    boxes[countOfVisibleBoxUp].second.transform.position = new Vector3(
                        boxes[countOfVisibleBoxUp].second.transform.position.x,
                        boxes[countOfVisibleBoxDown].second.transform.position.y + L,
                        boxes[countOfVisibleBoxUp].second.transform.position.z
                    );

                    countOfVisibleBoxUp = (countOfVisibleBoxUp + 1) % numberOfPairBoxes;
                    countOfVisibleBoxDown = (countOfVisibleBoxDown + 1) % numberOfPairBoxes;
                }
            } else {
                if (Math.Abs(boxes[countOfVisibleBoxDown].first.transform.position.y
                    - player.position.y) > visibleLength) {
                    boxes[countOfVisibleBoxDown].first.transform.position = new Vector3(
                        boxes[countOfVisibleBoxDown].first.transform.position.x,
                        boxes[countOfVisibleBoxUp].first.transform.position.y - L,
                        boxes[countOfVisibleBoxDown].first.transform.position.z
                    );
                    boxes[countOfVisibleBoxDown].second.transform.position = new Vector3(
                        boxes[countOfVisibleBoxDown].second.transform.position.x,
                        boxes[countOfVisibleBoxUp].second.transform.position.y - L,
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
}
