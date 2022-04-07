using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleHTTP;
using TMPro;

public class PayloadManager : MonoBehaviour
{
    [SerializeField]
    private int apiUpdateDelay = 5;
    private DateTime lastApiUpdate;

    [SerializeField]
    private TMP_Text[] satText;
    [SerializeField]
    private Animator[] satAnimators;
    [SerializeField]
    private SpriteRenderer[] satSprites;
    [SerializeField]
    private TMP_Text connectionText;
    [SerializeField]
    private TMP_Text timeUntilText;
    [SerializeField]
    private GameObject warningText;

    private StatusData data;

    private DateTime startTime;

    private void Start()
    {
        lastApiUpdate = DateTime.Now;
        startTime = DateTime.Now;
    }
    private void Update()
    {
        if (DateTime.Now > lastApiUpdate.AddSeconds(apiUpdateDelay))
        {
            lastApiUpdate = DateTime.Now;
            StartCoroutine(ApiUpdate());
        }
    }
    private void UpdateConnectionStatus()
    {
        if (data == null) return;
        int timeInPeriod = (DateTime.Now - startTime).Minutes % 30;
        if (timeInPeriod < 10)
        {
            // sat0
            connectionText.text = "DLAB Connection Status: " + data.status0;
            warningText.SetActive(data.IsOnline(0));
            if (data.IsOnline(0))
            {
                // looking for offline
                if (!data.IsOnline(1))
                {
                    timeUntilText.text = "Time Until Offline: " + (10 - timeInPeriod);
                } else if (!data.IsOnline(2))
                {
                    timeUntilText.text = "Time Until Offline: " + (20 - timeInPeriod);
                } else {
                    timeUntilText.text = "Time Until Offline: Infinite";
                }
            } else {
                // looking for online
                if (data.IsOnline(1))
                {
                    timeUntilText.text = "Time Until Online: " + (10 - timeInPeriod);
                } else if (data.IsOnline(2))
                {
                    timeUntilText.text = "Time Until Online: " + (20 - timeInPeriod);
                } else {
                    timeUntilText.text = "Time Until Online: Infinite";
                }
            }
        } else if (timeInPeriod < 20)
        {
            // sat1
            connectionText.text = "DLAB Connection Status: " + data.status1;
            warningText.SetActive(data.IsOnline(1));
            if (data.IsOnline(1))
            {
                // looking for offline
                if (!data.IsOnline(2))
                {
                    timeUntilText.text = "Time Until Offline: " + (20 - timeInPeriod);
                } else if (!data.IsOnline(0))
                {
                    timeUntilText.text = "Time Until Offline: " + (30 - timeInPeriod);
                } else {
                    timeUntilText.text = "Time Until Offline: Infinite";
                }
            } else {
                // looking for online
                if (data.IsOnline(2))
                {
                    timeUntilText.text = "Time Until Online: " + (20 - timeInPeriod);
                } else if (data.IsOnline(0))
                {
                    timeUntilText.text = "Time Until Online: " + (30 - timeInPeriod);
                } else {
                    timeUntilText.text = "Time Until Online: Infinite";
                }
            }
        } else {
            // sat2
            connectionText.text = "DLAB Connection Status: " + data.status2;
            warningText.SetActive(data.IsOnline(2));
            if (data.IsOnline(2))
            {
                // looking for offline
                if (!data.IsOnline(0))
                {
                    timeUntilText.text = "Time Until Offline: " + (30 - timeInPeriod);
                } else if (!data.IsOnline(1))
                {
                    timeUntilText.text = "Time Until Offline: " + (40 - timeInPeriod);
                } else {
                    timeUntilText.text = "Time Until Offline: Infinite";
                }
            } else {
                // looking for online
                if (data.IsOnline(0))
                {
                    timeUntilText.text = "Time Until Online: " + (30 - timeInPeriod);
                } else if (data.IsOnline(1))
                {
                    timeUntilText.text = "Time Until Online: " + (40 - timeInPeriod);
                } else {
                    timeUntilText.text = "Time Until Online: Infinite";
                }
            }
        }
        // find what satellite is in view
        // if yes, find time till out of view
        // if no, find time till back in view
    }
    private IEnumerator ApiUpdate()
    {
        // Create the request object
        Request request = new Request("http://localhost:5001/status");

        // Instantiate the client
        Client http = new Client();
        // Send the request
        yield return http.Send(request);

        // Use the response if the request was successful, otherwise print an error
        if (http.IsSuccessful())
        {
            Response resp = http.Response();
            // StatusData dataOut = new StatusData("ok", "ok", "ok");
            // string json = JsonUtility.ToJson(dataOut);
            // print("Json data: " + json);
            data = JsonUtility.FromJson<StatusData>(resp.Body());
            print("Json data: " + data);
            satText[0].text = "Sat0: " + data.status0;
            satText[1].text = "Sat1: " + data.status1;
            satText[2].text = "Sat2: " + data.status2;
            if (data.status0 == "offline")
            {
                satText[0].color = Color.red;
                satAnimators[0].SetBool("IsOffline", true);
                satSprites[0].color = Color.red;
            } else {
                satText[0].color = Color.white;
                satAnimators[0].SetBool("IsOffline", false);
                satSprites[0].color = Color.white;
            }
            if (data.status1 == "offline")
            {
                satText[1].color = Color.red;
                satAnimators[1].SetBool("IsOffline", true);
                satSprites[1].color = Color.red;
            } else {
                satText[1].color = Color.white;
                satAnimators[1].SetBool("IsOffline", false);
                satSprites[1].color = Color.white;
            }
            if (data.status2 == "offline")
            {
                satText[2].color = Color.red;
                satAnimators[2].SetBool("IsOffline", true);
                satSprites[2].color = Color.red;
            } else {
                satText[2].color = Color.white;
                satAnimators[2].SetBool("IsOffline", false);
                satSprites[2].color = Color.white;
            }
            UpdateConnectionStatus();
            Debug.Log("status: " + resp.Status().ToString() + "\nbody: " + resp.Body());
        } else {
            Debug.Log("error: " + http.Error());
        }
    }
    private void SetStatus(int sat, bool status)
    {
        satText[sat].color = status ? Color.white : Color.red;
        satAnimators[sat].SetBool("IsOffline", status);
        satSprites[sat].color = status ? Color.white : Color.red;
    }
}
[Serializable]
public class StatusData
{
    // in this format for REST request serialization
    public String status0;
    public String status1;
    public String status2;

    public StatusData(String status0, String status1, String status2)
    {
        this.status0 = status0;
        this.status1 = status1;
        this.status2 = status2;
    }
    public bool IsOnline(int sat)
    {
        if (sat == 0)
        {
            return status0 == "ok";
        } else if (sat == 1)
        {
            return status1 == "ok";
        } else {
            return status2 == "ok";
        }
    }
}