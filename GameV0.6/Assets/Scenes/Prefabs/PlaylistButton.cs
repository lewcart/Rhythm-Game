using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaylistButton : MonoBehaviour
{
    public string[] playlist;
    private GameObject diffscene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void savePlaylist()
    {
        diffscene = GameObject.FindGameObjectWithTag("songName");
        //string[] playlist;
        diffscene.SendMessage("savePlaylist", playlist);
    }
}
