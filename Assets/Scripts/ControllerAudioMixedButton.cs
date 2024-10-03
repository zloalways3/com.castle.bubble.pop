using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ControllerAudioMixedButton : MonoBehaviour
{
    [SerializeField] private AudioMixer _resonanceCore;
    [SerializeField] private Image waveToggleGlyph;
    [SerializeField] private Image melodyToggleGlyph;
    [SerializeField] private Sprite pulseOnSigil;
    [SerializeField] private Sprite pulseOffSigil;
    [SerializeField] private Button saveButton;

    private bool _echoPulseActive;
    private bool _harmonyThread;

    void Start()
    {
        _echoPulseActive = PlayerPrefs.GetInt("sonicEchoToggled", 1) == 1;
        _harmonyThread = PlayerPrefs.GetInt("_melodicFlowActive", 1) == 1;
        
        SoundWarpGrid();
        TuneShifter();
        
        saveButton.onClick.AddListener(PauseMark);
    }

    public void SonicFluxToggle()
    {
        _echoPulseActive = !_echoPulseActive;
        SoundWarpGrid();
    }

    public void ResonanceFlux()
    {
        _harmonyThread = !_harmonyThread;
        TuneShifter();
    }

    private void SoundWarpGrid()
    {
        _resonanceCore.SetFloat(PlayerStateConfig.SONIC_BEACON, _echoPulseActive ? 0f : -80f);
        waveToggleGlyph.sprite = _echoPulseActive ? pulseOnSigil : pulseOffSigil;
    }

    private void TuneShifter()
    {
        _resonanceCore.SetFloat(PlayerStateConfig.ECHO_SIGNATURE, _harmonyThread ? 0f : -80f);
        melodyToggleGlyph.sprite = _harmonyThread ? pulseOnSigil : pulseOffSigil;
    }
    
    public void PauseMark()
    {
        PlayerPrefs.SetInt("sonicEchoToggled", _echoPulseActive ? 1 : 0);
        PlayerPrefs.SetInt("_melodicFlowActive", _harmonyThread ? 1 : 0);
        PlayerPrefs.Save();
    }
}