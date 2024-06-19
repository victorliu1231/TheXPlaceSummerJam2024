using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Fade: MonoBehaviour
{
    public float timeBetweenFadeInterps;
    public enum FadeType {In, Out};
    public FadeType fadeType;
    public float timeDelay;
    public Color startColor;
    public Color endColor;

    void Start()
    {
        StartCoroutine(StartFade());
    }

    // Update is called once per frame
    IEnumerator StartFade()
    {
        TextMeshProUGUI text;
        Image image;
        SpriteRenderer sprite;
        yield return new WaitForSeconds(timeDelay);
        for (int i = 0; i < 10; i++){
            if (fadeType == FadeType.In){
                if (TryGetComponent<TextMeshProUGUI>(out text)) text.color = new Color(startColor.r, startColor.g, startColor.b, i / 10f);
                if (TryGetComponent<Image>(out image)) image.color = new Color(startColor.r, startColor.g, startColor.b, i / 10f);
                if (TryGetComponent<SpriteRenderer>(out sprite)) sprite.color = new Color(startColor.r, startColor.g, startColor.b, i / 10f);
            } else {
                if (TryGetComponent<TextMeshProUGUI>(out text)) text.color = new Color(endColor.r, endColor.g, endColor.b, 1 - (i+1) / 10f);
                if (TryGetComponent<Image>(out image)) image.color = new Color(endColor.r, endColor.g, endColor.b, 1 - (i+1) / 10f);
                if (TryGetComponent<SpriteRenderer>(out sprite)) sprite.color = new Color(endColor.r, endColor.g, endColor.b, 1 - (i+1) / 10f);
            }
            yield return new WaitForSeconds(timeBetweenFadeInterps);
        }
    }
}