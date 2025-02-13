using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AreaNameTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _areaName;

    private void Start()
    {
        TMP_Text textMesh = _areaName.GetComponent<TextMeshPro>();
        textMesh.color = new Color(1, 1, 1, 0);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        GetComponent<BoxCollider2D>().enabled = false;
        if (other.CompareTag("Player"))
        {
            StartCoroutine(FadeInText());
        }
    }

    IEnumerator FadeInText()
    {
        TMP_Text textMesh = _areaName.GetComponent<TextMeshPro>();
        textMesh.color = new Color(1, 1, 1, 0);
        var t = 0f;
        var speed = 1.5f;
        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            textMesh.color = new Color(1, 1, 1, t);
            yield return null;
        }
        yield return new WaitForSeconds(5f);
        StartCoroutine(FadeOutText());
    }

    IEnumerator FadeOutText()
    {
        TMP_Text textMesh = _areaName.GetComponent<TextMeshPro>();
        textMesh.color = new Color(1, 1, 1, 1);
        var t = 1f;
        var speed = 1.5f;
        while (t > 0f)
        {
            t -= Time.deltaTime * speed;
            textMesh.color = new Color(1, 1, 1, t);
            yield return null;
        }
    }
}
