using UnityEngine;

namespace Chimera
{
    //
    // Parameter controller for stripe effects
    //
    [ExecuteInEditMode]
    public class StripeController : MonoBehaviour
    {
        #region Public properties

        /// Horizontal density (1 / slice width)
        public float horizontalDensity {
            get { return _horizontalDensity; }
            set { _horizontalDensity = value; }
        }

        [Space, SerializeField]
        float _horizontalDensity = 2;

        /// Vertical density (1 / slice height)
        public float verticalDensity {
            get { return _verticalDensity; }
            set { _verticalDensity = value; }
        }

        [SerializeField]
        float _verticalDensity = 32;

        /// Scroll speed
        public float scrollSpeed {
            get { return _scrollSpeed; }
            set { _scrollSpeed = value; }
        }

        [SerializeField]
        float _scrollSpeed = 7;

        /// Random color
        public float randomColor {
            get { return _randomColor; }
            set { _randomColor = value; }
        }

        [Space, SerializeField, Range(0, 1)]
        float _randomColor = 1;

        /// See-through rate
        public float seethrough {
            get { return _seethrough; }
            set { _seethrough = value; }
        }

        [SerializeField, Range(0, 1)]
        float _seethrough = 0;

        /// Cutoff level
        public float cutoff {
            get { return _cutoff; }
            set { _cutoff = value; }
        }

        [SerializeField, Range(0, 1)]
        float _cutoff = 0;

        /// Probability of highlighten slices
        public float highlightRate {
            get { return _highlightRate; }
            set { _highlightRate = value; }
        }

        [Space, SerializeField, Range(0, 1)]
        float _highlightRate = 0.2f;

        /// Highlight color
        public Color highlightColor {
            get { return _highlightColor; }
            set { _highlightColor = value; }
        }

        [SerializeField, ColorUsage(false, true, 0, 8, 0.128f, 3)]
        Color _highlightColor = Color.white;

        /// Highlight color randomness
        public float highlightRandomColor {
            get { return _highlightRandomColor; }
            set { _highlightRandomColor = value; }
        }

        [SerializeField, Range(0, 1)]
        float _highlightRandomColor = 0;

        #endregion

        #region MonoBehaviour functions

        int _idParams;
        int _idModifiers;
        int _idHighlight;
        int _idHighlightColor;

        float _time = 40;

        void OnEnable()
        {
            _idParams = Shader.PropertyToID("_Chimera_StripeParams");
            _idModifiers = Shader.PropertyToID("_Chimera_StripeModifiers");
            _idHighlight = Shader.PropertyToID("_Chimera_StripeHighlight");
            _idHighlightColor = Shader.PropertyToID("_Chimera_StripeHighlightColor");
        }

        void LateUpdate()
        {
            _time += Time.deltaTime * _scrollSpeed;

            var highlightIntensity = _highlightColor.grayscale;

            Shader.SetGlobalVector(_idParams, new Vector3(_time, _horizontalDensity, _verticalDensity));
            Shader.SetGlobalVector(_idModifiers, new Vector3(_randomColor, _seethrough, _cutoff));
            Shader.SetGlobalVector(_idHighlight, new Vector3(_highlightRate, highlightIntensity, _highlightRandomColor));
            Shader.SetGlobalColor(_idHighlightColor, _highlightColor / highlightIntensity);
        }

        #endregion
    }
}
