using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Flash: MonoBehaviour
{
    public float timeBetweenFlashes;
    public int numFlashes;
    public float timeDelay;

    void Start()
    {
        StartCoroutine(FlashObject());
    }

    // Update is called once per frame
    IEnumerator FlashObject()
    {
        Image image;
        SpriteRenderer sprite;
        TextMeshProUGUI text;
        yield return new WaitForSeconds(timeDelay);
        for (int i = 0; i < numFlashes; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                if (TryGetComponent<Image>(out image)) image.color = new Color(image.color.r, image.color.g, image.color.b, j*0.1f);
                if (TryGetComponent<SpriteRenderer>(out sprite)) sprite.color = new Color(image.color.r, image.color.g, image.color.b, j*0.1f);
                if (TryGetComponent<TextMeshProUGUI>(out text)) text.color = new Color(image.color.r, image.color.g, image.color.b, j*0.1f);
                foreach (Transform child in transform)
                {
                    if (child.TryGetComponent<Image>(out image)) image.color = new Color(image.color.r, image.color.g, image.color.b, j*0.1f);
                    if (child.TryGetComponent<SpriteRenderer>(out sprite)) sprite.color = new Color(image.color.r, image.color.g, image.color.b, j*0.1f);
                    if (child.TryGetComponent<TextMeshProUGUI>(out text)) text.color = new Color(image.color.r, image.color.g, image.color.b, j*0.1f);
                }
                yield return new WaitForSeconds(0.05f);
                
            }
            for (int j = 0; j < 10; j++)
            {
                if (TryGetComponent<Image>(out image)) image.color = new Color(image.color.r, image.color.g, image.color.b, 1 - j*0.1f);
                if (TryGetComponent<SpriteRenderer>(out sprite)) sprite.color = new Color(image.color.r, image.color.g, image.color.b, 1 - j*0.1f);
                if (TryGetComponent<TextMeshProUGUI>(out text)) text.color = new Color(image.color.r, image.color.g, image.color.b, 1 - j*0.1f);
                foreach (Transform child in transform)
                {
                    if (child.TryGetComponent<Image>(out image)) image.color = new Color(image.color.r, image.color.g, image.color.b, 1 - j*0.1f);
                    if (child.TryGetComponent<SpriteRenderer>(out sprite)) sprite.color = new Color(image.color.r, image.color.g, image.color.b, 1 - j*0.1f);
                    if (child.TryGetComponent<TextMeshProUGUI>(out text)) text.color = new Color(image.color.r, image.color.g, image.color.b, 1 - j*0.1f);
                }
                yield return new WaitForSeconds(0.05f);
            }
            yield return new WaitForSeconds(timeBetweenFlashes);
        }
    }
}