using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ScreenFade : MonoBehaviour
{
    public TeleportationProvider teleportationProvider;
    public bool fadeOnStart = true;
    public float fadeDuration;
    public Color fadeColor;
    private Renderer rend;
    
    void Start()
    {
        rend = GetComponent<Renderer>();
        if (fadeOnStart)
        {
            FadeIn();
        }
    }
    
    public void FadeIn()
    {
        Fade(1, 0);
    }

    public void FadeOut()
    {
        Fade(0,1);
    }
    
    public void Fade(float alphaIn, float alphaOut)
    {
        StartCoroutine(FadeRoutine(alphaIn, alphaOut));
    }

    public void teleportFade()
    {
        fadeDuration = teleportationProvider.delayTime;
        Fade(0,1);
        fadeDuration = 1f;
        FadeIn();
    }

    public IEnumerator FadeRoutine(float alphaIn, float alphaOut)
    {
        
        float timer = 0;
        while (timer <= fadeDuration)
        {
            Color newColor = fadeColor;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);
            
            rend.material.SetColor("_BaseColor", newColor);

            timer += Time.deltaTime;
            yield return null;
        }
        
        Color newColor2 = fadeColor;
        newColor2.a = alphaOut;
            
        rend.material.SetColor("_BaseColor", newColor2);
    }
}
