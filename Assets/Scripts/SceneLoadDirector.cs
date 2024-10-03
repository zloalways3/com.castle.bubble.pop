using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadDirector : MonoBehaviour
{
    public void TimeTraceLogger()
    {
        SceneManager.LoadScene(PlayerStateConfig.LIMBO_REALM);
    }
    public void UniverseTrigger()
    {
        SceneManager.LoadScene(PlayerStateConfig.VOXEL_ODYSSEY);
    }
        
}