using UnityEngine;

public class PositionSquareController : MonoBehaviour
{
    public int gridX, gridY;
    public string squareTag;
    private float squareSpacing = 0.23f;
    
    public void InitializeSquare(int x, int y)
    {
        gridX = x;
        gridY = y;
        squareTag = gameObject.tag;
    }

    public void MoveToPosition(int x, int y)
    {
        gridX = x;
        gridY = y;
        
        Vector3 novaVector = new Vector3(x * (0.0733539388f + squareSpacing), 
            y * (0.0605170019f + squareSpacing), 0);
        transform.position = novaVector;
    }
    
    void OnMouseDown()
    {
        SquareGameController squareGameController = GetComponentInParent<SquareGameController>();
        if (squareGameController != null)
        {
            squareGameController.HandleSquareClick(this);
        }
    }
}