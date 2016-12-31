using UnityEngine;

namespace Spectrum
{
    public class LightColorChanger : MonoBehaviour
    {
        [SerializeField] Light[] _targets;
        [SerializeField, Range(0, 1)] float _saturation = 0.3f;
        [SerializeField] float _speed = 0.5f;

        void Update()
        {
            var time = Time.time * _speed;
            var slide = 1.0f / _targets.Length;

            for (var i = 0; i < _targets.Length; i++)
            {
                var hue = (i * slide + time) % 1.0f;
                _targets[i].color = Color.HSVToRGB(hue, _saturation, 1);
            }
        }
    }
}
