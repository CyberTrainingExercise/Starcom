using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class WarningTextEffect : MonoBehaviour
{
    [SerializeField]
    private String[] textToDisplay;

    private TMP_Text text;
    private DateTime startTime;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        startTime = DateTime.Now;
    }
    private void Update()
    {
        int timeInPeriod = (DateTime.Now - startTime).Seconds % 10;
        text.text = textToDisplay[timeInPeriod];
    }
}
