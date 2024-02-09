using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class AudioGroup
{
    public string Name;
    public List<AudioSource> List;
    [Range(0, 1)]
    public float Volume;
    private Slider _slider;

    public AudioGroup(List<AudioSource> list, float volume)
    {
        List = list;
        Volume = volume;
    }

    public AudioGroup(string name, List<AudioSource> list, float volume)
    {
        Name = name;
        List = list;
        Volume = volume;
    }

    public AudioGroup()
    {
        Name = "";
        List = new List<AudioSource>();
        Volume = 1f;
    }

    public AudioGroup(string name, float volume)
    {
        Name = name;
        List = new List<AudioSource>();
        Volume = volume;
    }
}
