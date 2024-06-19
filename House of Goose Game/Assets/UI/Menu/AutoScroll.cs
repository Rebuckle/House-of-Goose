using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{
    public RectTransform contentPanelTransform;

    float speed = 50.0f;

    [SerializeField]
    bool isLooping = false;

    void Start()
    {
        StartCoroutine(AutoScrollText());
    }

    IEnumerator AutoScrollText()
    {
        while (contentPanelTransform.localPosition.y < 5000)
        {
            contentPanelTransform.Translate(Vector3.up * speed * Time.deltaTime);
            if (contentPanelTransform.localPosition.y > 0)
            {
                if (isLooping)
                {
                    contentPanelTransform.localPosition = Vector3.up * contentPanelTransform.localPosition.y;
                }
                else
                {
                    break;
                }
            }
            yield return null;
        }
    }
}
