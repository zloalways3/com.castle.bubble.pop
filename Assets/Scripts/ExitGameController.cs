using UnityEngine;

public class ExitGameController : MonoBehaviour
{ 
    public void JourneyShade()
    {
#if UNITY_EDITOR
        
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}