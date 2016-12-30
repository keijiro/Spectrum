using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Chimera
{
    public class SceneSwitcher : MonoBehaviour
    {
        [SerializeField] UnityEngine.UI.Text _startText;
        [SerializeField] GameObject _sceneMenu;

        [Space]
        [SerializeField] ScreenBlit[] _screens;
        [SerializeField] float _fadeSpeed = 1;

        IEnumerator Start()
        {
            _sceneMenu.SetActive(false);

            yield return null;

            while (!Input.GetMouseButton(0) && Input.touches.Length == 0)
                yield return null;

            _startText.enabled = false;

            var alpha = 1.0f;
            while (alpha > 0)
            {
                alpha = Mathf.Clamp01(alpha - Time.deltaTime * _fadeSpeed);
                foreach (var blit in _screens)
                    blit.overlayColor = new Color(1, 1, 1, alpha);
                yield return null;
            }
        }

        IEnumerator FadeOutAndChangeScene(int index)
        {
            var alpha = 0.0f;
            while (alpha < 1)
            {
                alpha = Mathf.Clamp01(alpha + Time.deltaTime * _fadeSpeed);
                foreach (var blit in _screens)
                    blit.overlayColor = new Color(1, 1, 1, alpha);
                yield return null;
            }

            SceneManager.LoadScene(index);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
                _sceneMenu.SetActive(true);
            else if (Input.GetKeyUp(KeyCode.Tab))
                _sceneMenu.SetActive(false);
        }

        public void ChangeScene(int index)
        {
            StartCoroutine(FadeOutAndChangeScene(index));
        }
    }
}
