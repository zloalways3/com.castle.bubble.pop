using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneShiftController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _ascensionWhisper;
    private float _etherPulseChrono;
    private int _beaconQuota; 
    private const float _beaconInterval = 0.5f;

    private void Start()
    {
        StartCoroutine(UniverseSeed(PlayerStateConfig.PRIME_REALM));
    }

    private IEnumerator UniverseSeed(string nameScene)
    {
        AsyncOperation phaseSyncCycle = SceneManager.LoadSceneAsync(nameScene);

        phaseSyncCycle.allowSceneActivation = false;

        while (!phaseSyncCycle.isDone)
        {
            SymbolPulseHandler();
                
            if (phaseSyncCycle.progress >= 0.9f)
            {
                phaseSyncCycle.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    private void SymbolPulseHandler()
    {
        _etherPulseChrono += Time.deltaTime;

        if (_etherPulseChrono >= _beaconInterval)
        {
            _beaconQuota = (_beaconQuota + 1) % 4;
            string etherTokens = new string('.', _beaconQuota);
            _ascensionWhisper.text = "Loading" + etherTokens;
            _etherPulseChrono = 0f;
        }
    }
}