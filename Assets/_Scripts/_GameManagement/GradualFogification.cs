using UnityEngine;

public class GradualFogification : MonoBehaviour
{
    private static readonly int FogCol = Shader.PropertyToID("_FogCol");
    private static readonly int Tint = Shader.PropertyToID("_Tint");
    private static readonly int FogIntens = Shader.PropertyToID("_FogIntens");

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
        skyboxMat.SetColor(FogCol, RenderSettings.fogColor);
        skyboxMat.SetColor(Tint, RenderSettings.fogColor);
        skyboxMat.SetFloat(FogIntens, fogDensity + 0.2f);
    }
}