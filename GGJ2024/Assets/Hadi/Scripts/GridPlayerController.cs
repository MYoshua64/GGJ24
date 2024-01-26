using UnityEngine;

public class GridPlayerController : MonoBehaviour, IGridController
{
  [SerializeField]
  private GridMover monsterCharacter;
  [SerializeField]
  private GridMover weakCharacter;

  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.W))
      Move(Vector2.up);
    if (Input.GetKeyDown(KeyCode.A))
      Move(Vector2.left);
    if (Input.GetKeyDown(KeyCode.S))
      Move(Vector2.down);
    if (Input.GetKeyDown(KeyCode.D))
      Move(Vector2.right);
  }

  public void Move(Vector2 input)
  {
    if (!monsterCharacter.CanMove || !weakCharacter.CanMove) return;
    monsterCharacter.Move(input);
    weakCharacter.Move(input);
  }
}
