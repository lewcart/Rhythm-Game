using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class Reset : MonoBehaviour
{
    //reloads Game scene
    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }
}