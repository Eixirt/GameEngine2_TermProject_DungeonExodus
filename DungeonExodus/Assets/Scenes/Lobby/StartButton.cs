using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartButton : MonoBehaviour
{
    [SerializeField] private int SceneIndex;

    public void OnButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneIndex);
    }
}
