using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snow : MonoBehaviour
{
    private AreaEffector2D[] _winds;
    private LayerMask _windLayer;
    // Start is called before the first frame update
    private void Awake() 
    {
        Wind[] _windsObject = FindObjectsOfType<Wind>();
        _winds = new AreaEffector2D[_windsObject.Length];
        for (int i = 0; i < _winds.Length; i++)
        {
            _winds[i] = _windsObject[i].GetComponent<AreaEffector2D>();
        }

        _windLayer = _winds[0].colliderMask;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            foreach (var wind in _winds)
            {
                wind.colliderMask = 0;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            foreach (var wind in _winds)
            {
                wind.colliderMask = _windLayer;
            }
        }
    }
}
