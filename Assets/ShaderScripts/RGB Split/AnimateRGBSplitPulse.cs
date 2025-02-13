using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AnimateRGBSplitPulse : MonoBehaviour
{
    public float minFrequency = 2.0f;
    public float maxFrequency = 10.0f;
    public float period = 5.0f;

    void Update()
    {
        var volume = VolumeManager.instance.stack.GetComponent<RGBSplitVolume>();
        if (volume != null)
        {
  
            float t = (Mathf.Sin(Time.time * (2 * Mathf.PI / period)) + 1.0f) * 0.5f;
            volume.pulseFrequency.value = Mathf.Lerp(minFrequency, maxFrequency, t);
        }
    }
}
