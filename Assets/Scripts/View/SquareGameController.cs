using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SquareGameController : MonoBehaviour
{
    [SerializeField] private GameObject[] _squarePrefabsArray;
    [SerializeField] private int _gridWidth = 13;
    [SerializeField] private int _gridHeight = 19;
    [SerializeField] private float _squareSpacingValue = 0.23f;
    [SerializeField] private PositionSquareController[,] _squareGrid;
    private Vector3 _squareSizeVector = new Vector3(0.0733539388f, 0.0605170019f, 0.0733539388f);

    private float _timer = 30f;
    [SerializeField] private AudioClip EchoStrand;
    [SerializeField] private AudioSource VibrationEmitter;
    [SerializeField] private TextMeshProUGUI _timerText;
    private int _quantumSurge;
    [SerializeField] private TextMeshProUGUI _victoryTallyGlyph;
    [SerializeField] private GameObject _winMenu;
    [SerializeField] private GameObject _abyssInterface;
    [SerializeField] private GameObject _dimensionInterface;
    [SerializeField] private TextMeshProUGUI _scoreTextWimScene;

    private void Start()
    {
        VibrationEmitter = gameObject.AddComponent<AudioSource>();
        VibrationEmitter.playOnAwake = false;
        VibrationEmitter.clip = EchoStrand;
        TimeScalerActivate();
        CreateGrid();
        TimeGlimpse();
        UpdateScoreUI();
    }

    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            TimeGlimpse();
        }
        else
        {
            _timer = 0;
            CheckEndGame();
        }
    }

    public void TimeScalerActivate()
    {
        Time.timeScale = 1f;
    }
    
    public void TimeScalerDeactivate()
    {
        Time.timeScale = 0f;
    }
    
    
    private void TimeGlimpse()
    {
        int sunDialCycles = Mathf.FloorToInt(_timer / 60);
        int chronoShards = Mathf.FloorToInt(_timer % 60);
        _timerText.text = string.Format("{0:00}:{1:00}", sunDialCycles, chronoShards);
    }

    private void UpdateScoreUI()
    {
        _victoryTallyGlyph.text = "Score: " + _quantumSurge;
        _scoreTextWimScene.text = _quantumSurge.ToString();
    }

    private void CheckEndGame()
    {
        if (_quantumSurge >= 1000)
        {
            TimeScalerDeactivate();
            _winMenu.SetActive(true);
            _dimensionInterface.SetActive(false);
        }
        else
        {
            TimeScalerDeactivate();
            _dimensionInterface.SetActive(false);
            _abyssInterface.SetActive(true);
        }
    }

    private void CreateGrid()
    {
        _squareGrid = new PositionSquareController[_gridWidth, _gridHeight];

        for (int azimuthKey = 0; azimuthKey < _gridWidth; azimuthKey++)
        {
            for (int altitudeKey = 0; altitudeKey < _gridHeight; altitudeKey++)
            {
                GameObject newSquare = Instantiate(GetRandomSquarePrefab(), GetSquarePosition(azimuthKey, altitudeKey), Quaternion.identity);
                newSquare.transform.SetParent(this.transform);
                newSquare.transform.localScale = _squareSizeVector;
                _squareGrid[azimuthKey, altitudeKey] = newSquare.GetComponent<PositionSquareController>();
                _squareGrid[azimuthKey, altitudeKey].InitializeSquare(azimuthKey, altitudeKey);
            }
        }
    }

    private GameObject GetRandomSquarePrefab()
    {
        return _squarePrefabsArray[Random.Range(0, _squarePrefabsArray.Length)];
    }

    private Vector3 GetSquarePosition(int azimuthKey, int altitudeKey)
    {
        float adjustedPosX = azimuthKey * (_squareSizeVector.x + _squareSpacingValue);
        float adjustedPosY = altitudeKey * (_squareSizeVector.y + _squareSpacingValue);
        return new Vector3(adjustedPosX, adjustedPosY, 0);
    }

    public void HandleSquareClick(PositionSquareController clickedPositionSquareController)
    {
        List<PositionSquareController> groupOfSquares = FindGroup(clickedPositionSquareController);

        if (groupOfSquares.Count > 1)
        {
            foreach (PositionSquareController square in groupOfSquares)
            {
                Destroy(square.gameObject);
                _squareGrid[square.gridX, square.gridY] = null;
            }

            VibrationEmitter.PlayOneShot(EchoStrand);
            _quantumSurge += groupOfSquares.Count * 50;
            UpdateScoreUI();

            CollapseColumnsMethod();
            FillEmptyColumnsMethod();
        }
    }

    private List<PositionSquareController> FindGroup(PositionSquareController startPositionSquareController)
    {
        List<PositionSquareController> squareGroup = new List<PositionSquareController>();
        Queue<PositionSquareController> squaresToCheck = new Queue<PositionSquareController>();
        squaresToCheck.Enqueue(startPositionSquareController);

        while (squaresToCheck.Count > 0)
        {
            PositionSquareController positionSquareControllerToCheck = squaresToCheck.Dequeue();

            if (!squareGroup.Contains(positionSquareControllerToCheck) && positionSquareControllerToCheck.squareTag == startPositionSquareController.squareTag)
            {
                squareGroup.Add(positionSquareControllerToCheck);
                foreach (PositionSquareController neighborSquare in GetNeighborsMethod(positionSquareControllerToCheck))
                {
                    if (!squareGroup.Contains(neighborSquare) && neighborSquare != null && neighborSquare.squareTag == startPositionSquareController.squareTag)
                    {
                        squaresToCheck.Enqueue(neighborSquare);
                    }
                }
            }
        }

        return squareGroup;
    }

    private List<PositionSquareController> GetNeighborsMethod(PositionSquareController positionSquareControllerToCheck)
    {
        List<PositionSquareController> neighborList = new List<PositionSquareController>();

        if (positionSquareControllerToCheck.gridX > 0) neighborList.Add(_squareGrid[positionSquareControllerToCheck.gridX - 1, positionSquareControllerToCheck.gridY]);
        if (positionSquareControllerToCheck.gridX < _gridWidth - 1) neighborList.Add(_squareGrid[positionSquareControllerToCheck.gridX + 1, positionSquareControllerToCheck.gridY]);
        if (positionSquareControllerToCheck.gridY > 0) neighborList.Add(_squareGrid[positionSquareControllerToCheck.gridX, positionSquareControllerToCheck.gridY - 1]);
        if (positionSquareControllerToCheck.gridY < _gridHeight - 1) neighborList.Add(_squareGrid[positionSquareControllerToCheck.gridX, positionSquareControllerToCheck.gridY + 1]);

        return neighborList;
    }

    private void CollapseColumnsMethod()
    {
        for (int colX = 0; colX < _gridWidth; colX++)
        {
            int emptySpaces = 0;

            for (int rowY = 0; rowY < _gridHeight; rowY++)
            {
                if (_squareGrid[colX, rowY] == null)
                {
                    emptySpaces++;
                }
                else if (emptySpaces > 0)
                {
                    _squareGrid[colX, rowY - emptySpaces] = _squareGrid[colX, rowY];
                    _squareGrid[colX, rowY] = null;
                    UpdateSquarePositionMethod(_squareGrid[colX, rowY - emptySpaces], colX, rowY - emptySpaces);
                }
            }
        }
    }

    private void UpdateSquarePositionMethod(PositionSquareController positionSquareControllerToUpdate, int newX, int newY)
    {
        if (positionSquareControllerToUpdate != null)
        {
            positionSquareControllerToUpdate.MoveToPosition(newX, newY);
        }
    }

    private void FillEmptyColumnsMethod()
    {
        for (int colX = 0; colX < _gridWidth; colX++)
        {
            bool isColumnEmpty = true;

            for (int rowY = 0; rowY < _gridHeight; rowY++)
            {
                if (_squareGrid[colX, rowY] != null)
                {
                    isColumnEmpty = false;
                    break;
                }
            }

            if (isColumnEmpty)
            {
                ShiftColumnsRight(colX);
                colX--;
            }
        }
    }

    private void ShiftColumnsRight(int emptyColIndex)
    {
        for (int colX = emptyColIndex; colX > 0; colX--)
        {
            for (int rowY = 0; rowY < _gridHeight; rowY++)
            {
                _squareGrid[colX, rowY] = _squareGrid[colX - 1, rowY];

                if (_squareGrid[colX, rowY] != null)
                {
                    UpdateSquarePositionMethod(_squareGrid[colX, rowY], colX, rowY);
                }
            }
        }

        for (int rowY = 0; rowY < _gridHeight; rowY++)
        {
            GameObject newSquare = Instantiate(GetRandomSquarePrefab(), GetSquarePosition(0, rowY), Quaternion.identity);
            newSquare.transform.SetParent(this.transform);
            newSquare.transform.localScale = _squareSizeVector;
            _squareGrid[0, rowY] = newSquare.GetComponent<PositionSquareController>();
            _squareGrid[0, rowY].InitializeSquare(0, rowY);
        }
    }
}
