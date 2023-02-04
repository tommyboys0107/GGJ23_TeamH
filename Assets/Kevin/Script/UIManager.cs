using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text startText, endText;

    public void ShowStartText()
    {
        startText.gameObject.SetActive(true);

        startText.color= Color.white;


        StartCoroutine(WaveTextFadeOut(3f));
    }

    IEnumerator WaveTextFadeOut(float duration)
    {
        Color alpha = startText.color;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            startText.color = Color.Lerp(alpha, new Color(1, 1, 1, 0), elapsedTime / duration);

            yield return null;
        }
    }

    public void ShowEndText()
    {
        endText.gameObject.SetActive(true);
    }


}
