using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class TypingText : MonoBehaviour
{
    [SerializeField]
    private String textToDisplay;

    private TMP_Text text;

    private void Start()
    {
        text = GetComponent<TMP_Text>();
        StartCoroutine("PrintText");
    }
    private void Update()
    {
        if (Input.GetKey("space"))
        {
            StopCoroutine("PrintText");
            StartCoroutine("PrintText");
        }
    }
    private IEnumerator PrintText()
    {
        int i;
        for (i = 0; i < textToDisplay.Length + 1; i++)
        {
            text.text = textToDisplay.Substring(0, i);
            if (i % 2 == 0)
            {
                text.text = text.text + "|";
            }
            yield return new WaitForSeconds(0.2f);
        }
        while (true)
        {
            if (i++ % 2 == 0)
            {
                text.text = textToDisplay + "|";
            } else {
                text.text = textToDisplay;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
}
