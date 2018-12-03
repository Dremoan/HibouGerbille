using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Events : MonoBehaviour {


    [SerializeField] private UnityEvent eventInvoke;

    public void InvokeEvent()
    {
        eventInvoke.Invoke();
    }
}
