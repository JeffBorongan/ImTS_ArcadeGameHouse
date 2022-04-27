using UnityEngine;

public class Spaceship : MonoBehaviour
{
    #region Move Update

    private void Update()
    {
        transform.Rotate(TileGameManager.Instance.SessionData.spaceshipSpeed * TileGameManager.Instance.SessionData.speedFactor * Time.deltaTime * Vector3.up);
    }

    #endregion

    #region Spaceship Count

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Head"))
        {
            TileGameManager.Instance.SpaceshipCount++;
        }
    }

    #endregion
}