using UnityEngine;
using UnityEngine.UI;

public class FullScreenToggle : MonoBehaviour
{
    Toggle toggle;

    void Awake()
    {
        toggle = GetComponent<Toggle>();

        // Initialize toggle state WITHOUT triggering OnValueChanged
        toggle.SetIsOnWithoutNotify(Screen.fullScreen);

        // Add listener manually
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    void OnToggleChanged(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        Debug.Log("Fullscreen is now: " + isFullscreen);
    }
}
