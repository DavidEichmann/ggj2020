using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    public Image fill;
    public Color maxHealthColor = Color.green;
    public Color minHealthColor = Color.red;
    public Renderer LightRenderer;

    private Transform _hpObj;

    private Director _director;

    private void Start()
    {
        _hpObj = GetComponent<Transform>();
        _director = FindObjectOfType<Director>();
    }

    private void Update()
    {
        if (_hpObj != null)
        {
            _hpObj.localScale = new Vector3(_director.Health, _hpObj.localScale.y, _hpObj.localScale.z);
            LightRenderer.material.SetColor("_EmissionColor", Color.Lerp(minHealthColor, maxHealthColor, _director.Health));

        }
    }

}
