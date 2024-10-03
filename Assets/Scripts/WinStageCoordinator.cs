using UnityEngine;

public class WinStageCoordinator : MonoBehaviour
{ 
    private int _presentEpoch;

    private void Awake()
    {
        EnergyGridSmith();
    }

    private void EnergyGridSmith()
    {
        _presentEpoch = PlayerPrefs.GetInt(PlayerStateConfig.ACTIVE_REALM, 0);
    }

    public void VictoryBell()
    {
        PlayerPrefs.SetInt(PlayerStateConfig.ACTIVE_REALM, _presentEpoch + 1);
        PlayerPrefs.Save();
        
        var chronicleOverseer = FindObjectOfType<StagePathManager>();
        chronicleOverseer.GateKeeper(_presentEpoch);
    }
}