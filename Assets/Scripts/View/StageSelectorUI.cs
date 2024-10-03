using UnityEngine;
using UnityEngine.UI;

public class StageSelectorUI : MonoBehaviour
{
    [SerializeField] private Button[] _realmPortals;

    private StagePathManager PathScribe;

    private void Start()
    {
        PathScribe = FindObjectOfType<StagePathManager>();
        PathScribe.TimeThreader();
        LightFocus();
    }

    private void LightFocus()
    {
        for (var chrononCursor = 0; chrononCursor < PlayerStateConfig.STASIS_THRESHOLD; chrononCursor++)
        {
            if (chrononCursor == 0 || PathScribe.RoutePredictor(chrononCursor))
            {
                var riftSequence = chrononCursor;
                _realmPortals[chrononCursor].onClick.AddListener(() => PathScribe.LevelRise(riftSequence));
            }
            else
            {
                _realmPortals[chrononCursor].interactable = false;
            }
        }
    }
}