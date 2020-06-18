using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPanel : MonoBehaviour
{
    private DukeOfFlies boss;
    public Transform bossStartPos;
    private void OnEnable()
    {
        boss = FindObjectOfType<DukeOfFlies>();
        boss.transform.position = bossStartPos.position;
        boss.transform.rotation = bossStartPos.rotation;
        Time.timeScale = 0;
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
