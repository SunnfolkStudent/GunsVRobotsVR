using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ScreenFade : MonoBehaviour
{
    public bool fadeOnTeleport;
    public TeleportationProvider teleportationProvider;
    public HandController handController;
    public bool fadeOnStart = true;
    public float fadeDuration;
    public Color fadeColor;
    private Renderer _rend;

    [SerializeField]
    private bool _dark;

    void Start()
    {
        _rend = GetComponent<Renderer>();
        if (fadeOnStart)
        {
            FadeIn();
        }
    }

    public void FadeIn()
    {
        Debug.Log("FadingIn");
        Fade(1, 0);
    }

    public void FadeOut()
    {
        Debug.Log("FadingOut");
        Fade(0, 1);
    }

    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    public void TeleportFadeFunction()
    {
        StartCoroutine(TeleportFade());
    }

    public IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        float timer = 0;
        while (timer <= fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);

            _rend.material.SetColor("_BaseColor", newColor);

            timer += Time.deltaTime;
            yield return null;
        }

        Color newColor2 = fadeColor;
        newColor2.a = alphaOut;

        _rend.material.SetColor("_BaseColor", newColor2);
        _dark = alphaIn < alphaOut;
    }

    IEnumerator TeleportFade()
    {
        fadeDuration = teleportationProvider.delayTime;
        handController.currentTeleportHand.SetActive(false);
        if (fadeOnTeleport)
        {
            FadeOut();
            while (!_dark)
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.0625f);
            FadeIn();
            while (_dark)
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.0625f);
        }
        else
        {
            yield return new WaitForSeconds(0.1875f);
        }
        handController.currentTeleportHand.SetActive(true);
    }
}

