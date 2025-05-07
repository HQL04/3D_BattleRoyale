using UnityEngine;

public class SparkleLight : MonoBehaviour
{
    private Light pointLight;
    public float maxIntensity = 5f;  // Cường độ tối đa của ánh sáng
    public float sparkleSpeed = 3f;  // Tốc độ nhấp nháy

    void Start()
    {
        // Lấy component Light
        pointLight = GetComponent<Light>();
    }

    void Update()
    {
        // Thay đổi cường độ ánh sáng theo hàm sin(time) để tạo hiệu ứng nhấp nháy
        pointLight.intensity = Mathf.Abs(Mathf.Sin(Time.time * sparkleSpeed)) * maxIntensity;
    }
}
