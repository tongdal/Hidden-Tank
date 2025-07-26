using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    public GameObject[] titles;

    Text text;
    private void Awake()
    {
        text = GetComponentInChildren<Text>(true);
    }
    public void MissOn()
    {
        titles[0].SetActive(true);
    }
    public void MissOff()
    {
        titles[0].SetActive(false);
    }
    public void HitOn()
    {
        titles[1].SetActive(true);
    }
    public void HitOff()
    {
        titles[1].SetActive(false);
    }
}

