using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GRP18
{

    public class StoryboardManager : MonoBehaviour
    {

        [SerializeField] private GerbilBehaviour fiScript;
        [SerializeField] private AudioSource cameraSource;

        public void FiEnters()
        {
            StartCoroutine(fiScript.FiComingIn());
        }

        public void LaunchAmbient()
        {
            StartCoroutine(BlendAmbienceCoroutine());
        }

        IEnumerator BlendAmbienceCoroutine()
        {
            cameraSource.Play();
            for (int i = 0; i < 100; i++)
            {
                this.GetComponent<AudioSource>().volume -= 0.005f;
                cameraSource.volume += 0.005f;
                yield return null;
            }
        }

    }

}
