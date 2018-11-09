using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerbilBehaviour : MonoBehaviour {

    public AnimationCurve curveSpeed;
    [SerializeField] private float returnMoveDuration;

	void Update ()
    {
		if(Input.GetMouseButtonDown(0))
        {
            transform.Translate(10, 0, 0);
        }
	}
}
