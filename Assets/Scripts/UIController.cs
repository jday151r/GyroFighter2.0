using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Image barrelRollReadyCircle;
    public PlayerController player;

    void Start()
    {

    }

    void Update()
    {
        barrelRollReadyCircle.fillAmount = 1 - (player.barrelLerp - player.barrelLimit) / (1 - player.barrelLimit);
        barrelRollReadyCircle.color = new Color(1, 1, 1, (player.barrelLerp < player.barrelLimit) ? 1 : 0.5f);
    }
}