using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPanel : MonoBehaviour
{
    private DukeOfFlies boss;
    public Transform bossStartPos;
    private void OnEnable()
    {
        //boss = FindObjectOfType<DukeOfFlies>();
        //boss.transform.position = bossStartPos.position;
        //boss.transform.rotation = bossStartPos.rotation;
        //Time.timeScale = 0;
        //StartCoroutine(Wait());

        StartCoroutine(WaitForBoss());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    IEnumerator WaitForBoss()
    {
        yield return new WaitForSeconds(0.5f);

        boss = FindObjectOfType<DukeOfFlies>();
        boss.transform.position = bossStartPos.position;
        boss.transform.rotation = bossStartPos.rotation;
        Time.timeScale = 0;
        StartCoroutine(Wait());
    }
}
