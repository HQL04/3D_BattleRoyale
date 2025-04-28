using UnityEngine;

public class SparkleEffect : MonoBehaviour
{
    public Material material;          // Material cần làm lấp lánh
    public Color sparkleColor = Color.white;  // Màu lấp lánh
    public float sparkleSpeed = 2f;     // Tốc độ lấp lánh

    private void Update()
    {
        if (material != null)
        {
            float emission = Mathf.PingPong(Time.time * sparkleSpeed, 1.0f); // Sáng tối qua lại
            Color finalColor = sparkleColor * Mathf.LinearToGammaSpace(emission);
            material.SetColor("_EmissionColor", finalColor);
        }
    }
}
