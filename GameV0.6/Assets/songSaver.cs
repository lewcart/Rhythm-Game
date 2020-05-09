using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class songSaver : MonoBehaviour
{
    public string songName;
    public string[] songList;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void saveSong(string song)
    {
        songList = new string[1]{ song };
        //songName = song;
    }

    public void savePlaylist(string[] playlist)
    {
        Debug.Log("Yeet");
        songList = playlist;
        
    }
}
