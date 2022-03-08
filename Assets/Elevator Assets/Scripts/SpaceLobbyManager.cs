using UnityEngine;

public class SpaceLobbyManager : MonoBehaviour
{
    [SerializeField] private GameObject spaceshipHologram = null;
    [SerializeField] private GameObject game1Trophy = null;
    [SerializeField] private GameObject game2Trophy = null;
    [SerializeField] private GameObject game3Trophy = null;

    private void Start()
    {
        if (TrophyManager.Instance.IsGame1Accomplished)
        {
            game1Trophy.SetActive(true);
        }

        if (TrophyManager.Instance.IsGame2Accomplished)
        {
            game2Trophy.SetActive(true);
        }

        if (TrophyManager.Instance.IsGame3Accomplished)
        {
            game3Trophy.SetActive(true);
        }
    }

    private void Update()
    {
        spaceshipHologram.transform.Rotate(0, 0, 10 * Time.deltaTime);
    }

    public void OpenElevatorDoor()
    {
        ElevatorManager.Instance.OpenElevatorDoor();
    }

    public void GetElevatorPart(MeshRenderer meshRenderer)
    {
        ElevatorManager.Instance.GetElevatorPart(meshRenderer);
    }

    public void EnableEmissive(int number)
    {
        ElevatorManager.Instance.EnableEmissive(number);
    }
}