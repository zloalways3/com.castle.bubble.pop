using UnityEngine;
using UnityEngine.SceneManagement;

public class StagePathManager : MonoBehaviour
{
    public void GateKeeper(int levelIndex)
    {
        PlayerPrefs.SetInt(PlayerStateConfig.ADVANCEMENT_LADDER + levelIndex, 1);
        PlayerPrefs.Save();
    }

    public void LevelRise(int levelIndex)
    {
        PlayerPrefs.SetInt(PlayerStateConfig.ACTIVE_REALM, levelIndex + 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(PlayerStateConfig.VOXEL_ODYSSEY);
    }

    public void TimeThreader()
    {
        for (var glyphStream = 0; glyphStream < PlayerStateConfig.STASIS_THRESHOLD; glyphStream++)
        {
            if (!PlayerPrefs.HasKey(PlayerStateConfig.ADVANCEMENT_LADDER + glyphStream))
                PlayerPrefs.SetInt(PlayerStateConfig.ADVANCEMENT_LADDER + glyphStream, glyphStream == 0 ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    public bool RoutePredictor(int levelIndex)
    {
        return PlayerPrefs.GetInt(PlayerStateConfig.ADVANCEMENT_LADDER + levelIndex, 0) == 1;
    }
}