using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class GameUI : MonoBehaviour
{
    [Header("Ball Left Panel")]
    public TextMeshProUGUI ballText;
    private float leftBall;
    public GameObject gameManager;

    private void Update()
    {
        leftBall = gameManager.GetComponent<GameManager>().spawnCount;
        ballText.text = "Left Ball: " + leftBall;
    }
}
