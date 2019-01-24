using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HibouGerbille
{
    public class Launch : MonoBehaviour
    {

        public Rigidbody bodyNoisette;
        public float forceEject;

        void Start()
        {
            bodyNoisette.AddForce(Vector3.forward * forceEject, ForceMode.Impulse);
        }

    }
}
