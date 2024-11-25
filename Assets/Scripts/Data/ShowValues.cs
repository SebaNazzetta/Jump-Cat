using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowValues : MonoBehaviour
{
    public TMP_Text jumpText;
    void Update()
    {
        float jump = FindObjectOfType<PlayerController>().GetLastJumpForce();
        jumpText.text = "Last jump: " + jump;
    }
}
