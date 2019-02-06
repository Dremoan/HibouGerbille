using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GRP18_TheGerbilAndTheOwl
{
    public class Part2OwlEyes : MonoBehaviour
    {
        [SerializeField] private Animator owlEyes;

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<GerbilBehaviour>() != null)
            {
                owlEyes.SetBool("Part1", false);
                this.gameObject.SetActive(false);
            }
        }
    }
}
