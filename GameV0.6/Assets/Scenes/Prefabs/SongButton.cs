using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongButton : MonoBehaviour
{
    public string songName;
    private GameObject diffscene;

    // Start is called before the first frame update
    void Start()
    {
        diffscene = GameObject.FindGameObjectWithTag("songName");
        TMPro.TextMeshProUGUI mText = gameObject.GetComponent<TMPro.TextMeshProUGUI>();

        songName = mText.text;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void saveSong()
    {
        diffscene.SendMessage("saveSong", songName);
    }
}
