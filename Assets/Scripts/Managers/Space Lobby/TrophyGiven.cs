using UnityEngine;

public class TrophyGiven : MonoBehaviour
{
    #region Trophy Rotation

    private void Update()
    {
        transform.Rotate(0, 10 * Time.deltaTime, 0);
    }

    #endregion

    #region Trophy Detection

    private void OnTriggerEnter(Collider other)
    {
        if (SpaceLobbyManager.Instance.TrophyType == TrophyType.Game1)
        {
            if (other.CompareTag("Game1TrophyHolder"))
            {
                SpaceLobbyManager.Instance.TrophyType = TrophyType.None;
                SpaceLobbyManager.Instance.Game1TrophyHologram.SetActive(false);
                SpaceLobbyManager.Instance.Game1TrophyGiven.SetActive(false);
                SpaceLobbyManager.Instance.Game1TrophyDisplay.SetActive(true);
                SpaceLobbyManager.Instance.Game1TrophyLight.SetActive(true);
                gameObject.SetActive(false);
            }
        }

        if (SpaceLobbyManager.Instance.TrophyType == TrophyType.Game2)
        {
            if (other.CompareTag("Game2TrophyHolder"))
            {
                SpaceLobbyManager.Instance.TrophyType = TrophyType.None;
                SpaceLobbyManager.Instance.Game2TrophyHologram.SetActive(false);
                SpaceLobbyManager.Instance.Game2TrophyGiven.SetActive(false);
                SpaceLobbyManager.Instance.Game2TrophyDisplay.SetActive(true);
                SpaceLobbyManager.Instance.Game2TrophyLight.SetActive(true);
                gameObject.SetActive(false);
            }
        }

        if (SpaceLobbyManager.Instance.TrophyType == TrophyType.Game3)
        {
            if (other.CompareTag("Game3TrophyHolder"))
            {
                SpaceLobbyManager.Instance.TrophyType = TrophyType.None;
                SpaceLobbyManager.Instance.Game3TrophyHologram.SetActive(false);
                SpaceLobbyManager.Instance.Game3TrophyGiven.SetActive(false);
                SpaceLobbyManager.Instance.Game3TrophyDisplay.SetActive(true);
                SpaceLobbyManager.Instance.Game3TrophyLight.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }

    #endregion
}