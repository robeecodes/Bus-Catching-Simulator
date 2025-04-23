using UnityEngine;

public class GradualFogification : MonoBehaviour
{
    public static void IncreaseDensity(float timeOfDay, float startTime, float endTime, float minDensity,
        float maxDensity, Material skyboxMat)
    {
        bool wrapsAroundMidnight = endTime < startTime;

        float normalizedTime;

        if (wrapsAroundMidnight)
        {
            if (timeOfDay >= startTime || timeOfDay <= endTime)
            {
                if (timeOfDay >= startTime)
                {
                    normalizedTime = (timeOfDay - startTime) / ((24f - startTime) + endTime);
                }
                else
                {
                    normalizedTime = ((24f - startTime) + timeOfDay) / ((24f - startTime) + endTime);
                }
            }
            else
            {
                normalizedTime = 0f;
            }
        }
        else
        {
            if (timeOfDay >= startTime && timeOfDay <= endTime)
            {
                normalizedTime = (timeOfDay - startTime) / (endTime - startTime);
            }
            else
            {
                normalizedTime = 0f;
            }
        }

        float fogDensity = Mathf.Lerp(minDensity, maxDensity, normalizedTime);

        RenderSettings.fogDensity = fogDensity;
        skyboxMat.SetColor("_FogCol", RenderSettings.fogColor);
        skyboxMat.SetColor("_Tint", RenderSettings.fogColor);
        skyboxMat.SetFloat("_FogIntens", fogDensity + 0.2f);
    }
}