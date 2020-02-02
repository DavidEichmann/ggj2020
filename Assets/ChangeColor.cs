using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    public Image fill;
    public Color maxHealthColor = Color.green;
    public Color minHealthColor = Color.red;

    private Slider _slider;
    private Director _director;

    private void Start()
    {
        _slider = GetComponent<Slider>();
        _director = FindObjectOfType<Director>();
    }

    private void OnGUI()
    {
        var health = _director.Health;
        _slider.value = health;
        fill.color = Color.Lerp(minHealthColor, maxHealthColor, health);
    }
}
