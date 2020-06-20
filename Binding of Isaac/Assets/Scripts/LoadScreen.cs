using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadScreen : MonoBehaviour
{
    public GameObject loadScreen;
    public bool loadScreenActive = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TurnOffLoadScreen());
    }

    IEnumerator TurnOffLoadScreen()
    {
        yield return new WaitForSeconds(2f);
        loadScreenActive = false;
        loadScreen.SetActive(false);
    }
}
