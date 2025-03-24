using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] private float _windForce = 60;
    private AreaEffector2D _areaEffector2D;
    private SpriteRenderer _spriteRenderer;
    private WaitForSeconds _changeTime = new WaitForSeconds(5f);
    private WaitForSeconds _waitTime = new WaitForSeconds(1f);

    private void OnEnable()
    {
        _areaEffector2D = GetComponent<AreaEffector2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _areaEffector2D.forceMagnitude = _windForce;

        StartCoroutine(ChangeWind());
    }

    private IEnumerator ChangeWind()
    {
        float currentForce = _areaEffector2D.forceMagnitude;

        yield return _changeTime;

        _areaEffector2D.forceMagnitude = 0;

        yield return _waitTime;

        _areaEffector2D.forceMagnitude = currentForce;
        _areaEffector2D.forceMagnitude *= -1;

        StartCoroutine(ChangeWind());
    }

    [SerializeField] private float smoothingTime = 1f;  // Tiempo en segundos para suavizar la transición a 0
    private float currentSpeed = 0f;

    private void Update()
    {
        // Reducir la velocidad a 0 cuando la fuerza es 0
        if (_areaEffector2D.forceMagnitude == 0)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, Time.deltaTime / smoothingTime);  // Suavizar hacia 0 en 1 segundo
        }
        else
        {
            // Establecer velocidad normal cuando hay viento
            currentSpeed = (_areaEffector2D.forceMagnitude > 0) ? -0.2f : 0.2f;
        }

        // Cambiar el tamaño del sprite de forma suave con currentSpeed
        _spriteRenderer.size = new Vector2(_spriteRenderer.size.x + currentSpeed, _spriteRenderer.size.y);
    }
}
