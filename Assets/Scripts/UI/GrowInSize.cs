using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowInSize: MonoBehaviour
{
    public Vector3 startScale;
    public Vector3 endScale;
    public float duration;
    public float timeDelay;
    private float timeElapsed = 0;

    void Start()
    {
        transform.localScale = startScale;
        StartCoroutine(Scale());
    }

    IEnumerator Scale()
    {
        yield return new WaitForSeconds(timeDelay);
        while (true)
        {
            if (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                transform.localScale = Vector3.Lerp(startScale, endScale, timeElapsed / duration);
            }
            else
            {
                yield break;
            }
        }
    }
}