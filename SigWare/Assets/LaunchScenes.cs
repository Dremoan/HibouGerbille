using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GRP18
{
    public class LaunchScenes : MonoBehaviour

    {
        [SerializeField] private string[] nameScenes;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                LoadSceneGame(nameScenes[0]);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                LoadSceneGame(nameScenes[1]);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                LoadSceneGame(nameScenes[2]);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                LoadSceneGame(nameScenes[3]);
            }

        }

        private void LoadSceneGame(string name)
        {
            SceneManager.LoadScene(name);
        }
    }
}
