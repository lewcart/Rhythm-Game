using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Difficulty : MonoBehaviour
{
    

    void Start () {
         
     }

    public void PlayEasy()
    {
        SceneManager.LoadScene("GameEasy");
    }

    public void PlayNormal()
    {
        SceneManager.LoadScene("GameNormal");
    }

    public void PlayHard()
    {
        SceneManager.LoadScene("GameHard");
    }


}
