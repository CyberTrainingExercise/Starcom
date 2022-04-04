using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField]
    private int minutes = 30;

    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 360 * Time.deltaTime / (minutes * 60)));
    }
}
