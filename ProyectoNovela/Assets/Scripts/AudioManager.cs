using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameObject Song1, Song2;
    void Start()
    {
        Song1.SetActive(true);
        Song2.SetActive(false);
    }

    public void SwitchSong()
    {
        if (Song1 == true)
        {
            Song1.SetActive(false);
            Song2.SetActive(true);
        }
        if (Song1 == false)
        {
            Song1.SetActive(true);
            Song2.SetActive(false);
        }
    }
}