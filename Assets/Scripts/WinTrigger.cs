using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //ACA VA LA SECUENCIA QUE SURJA POR GANAR
            Debug.Log("WINNNN");
        }
    }
}
