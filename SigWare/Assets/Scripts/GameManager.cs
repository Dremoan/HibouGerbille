using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GRP18
{

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private string sceneName;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource musicEndingSource;



        private void Update()
        {
            ReloadScene();
        }

        private void ReloadScene()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(sceneName);
            }
        }

        public void Victoire()
        {
            #if SIGWARE
            LevelManager.Instance.Win()
            #endif
        }

        public void Defaite()
        {
            #if SIGWARE
            LevelManager.Instance.Lose()
            #endif
        }

        IEnumerator BlendMusicCoroutine()
        {
            musicEndingSource.Play();
            for (int i = 0; i < 200; i++)
            {
                musicSource.volume -= 0.005f;
                musicEndingSource.volume += 0.005f;
                yield return null;
            }
        }

        public void BlendMusics()
        {
            StartCoroutine(BlendMusicCoroutine());
        }
    }
}
