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
                if (TryGetComponent<TextMeshProUGUI>(out text)) text.color = new Color(1f, 1f, 1f, i / 10f);
                if (TryGetComponent<Image>(out image)) image.color = new Color(1f, 1f, 1f, i / 10f);
                if (TryGetComponent<SpriteRenderer>(out sprite)) sprite.color = new Color(1f, 1f, 1f, i / 10f);
            } else {
                if (TryGetComponent<TextMeshProUGUI>(out text)) text.color = new Color(1f, 1f, 1f, 1 - i / 10f);
                if (TryGetComponent<Image>(out image)) image.color = new Color(1f, 1f, 1f, 1 - i / 10f);
                if (TryGetComponent<SpriteRenderer>(out sprite)) sprite.color = new Color(1f, 1f, 1f, 1 - i / 10f);
            }
            yield return new WaitForSeconds(timeBetweenFadeInterps);
        }
    }
}