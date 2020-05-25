using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float lifetime;

    private void OnEnable()
    {
        StartCoroutine(Delay());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Delay()
    {
        yield return new WaitForSecondsRealtime(lifetime);
        this.gameObject.SetActive(false);
    }
}
