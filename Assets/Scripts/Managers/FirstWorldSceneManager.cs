using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FirstWorldSceneManager : MonoBehaviour
{
    [SerializeField] private string _thisWorldArtSceneName;
    [SerializeField] private Image _blackLoadPanel;

    private void Awake()
    {
        if (_blackLoadPanel != null)
        {
            _blackLoadPanel.gameObject.SetActive(true);
            StartCoroutine(FadeOut(_blackLoadPanel));
        }
    }
    void Start()
    {
        if(_thisWorldArtSceneName != "")
        {
            SceneManager.LoadScene(_thisWorldArtSceneName, LoadSceneMode.Additive);
        }
    }

    private IEnumerator FadeOut(Image spriteRenderer)
    {
        spriteRenderer.color = new Color(0, 0, 0, 1);
        var t = 1f;
        var speed = 1.5f;
        yield return new WaitForSeconds(0.5f);
        while (t > 0f)
        {
            t -= Time.deltaTime * speed;
            spriteRenderer.color = new Color(0, 0, 0, t);
            yield return null;
        }

        spriteRenderer.color = new Color(0, 0, 0, 0);
    }

}
