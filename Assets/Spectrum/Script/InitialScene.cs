using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialScene : MonoBehaviour
{
    void Start()
    {
        if (Display.displays.Length > 1)
            Display.displays[1].Activate();

        SceneManager.LoadScene(1);
    }
}
