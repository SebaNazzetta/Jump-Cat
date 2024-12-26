using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] private float _windForce = 60;
    private AreaEffector2D _areaEffector2D;
    private WaitForSeconds _changeTime = new WaitForSeconds(5f);
    private WaitForSeconds _waitTime = new WaitForSeconds(1f);

    private void OnEnable()
    {
        _areaEffector2D = GetComponent<AreaEffector2D>();
        _areaEffector2D.forceMagnitude = _windForce;

        StartCoroutine(ChangeWind());
    }

    private IEnumerator ChangeWind()
    {
        while (true)
        {
            float currentForce = _areaEffector2D.forceMagnitude;

            yield return _changeTime;

            _areaEffector2D.forceMagnitude = 0;

            yield return _waitTime;

            _areaEffector2D.forceMagnitude = currentForce;
            _areaEffector2D.forceMagnitude *= -1;

            StartCoroutine(ChangeWind());
        }
    }

}
