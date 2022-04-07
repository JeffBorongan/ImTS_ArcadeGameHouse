using UnityEngine;

public class Trophy : MonoBehaviour
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
        if (SpaceLobbyManager.Instance.GameNumber == GameNumber.Game1)
        {
            if (other.CompareTag("Game1TrophyHolder"))
            {
                VoiceOverManager.Instance.LastButtonSelected = LastButtonSelected.Any;
                VoiceOverManager.Instance.InvokeLastButtonSelected();
                TrophyManager.Instance.AddGameTrophyPresented((int)GameNumber.Game1);
                SpaceLobbyManager.Instance.GameNumber = GameNumber.None;
                SpaceLobbyManager.Instance.Game1TrophyHologram.SetActive(false);
                SpaceLobbyManager.Instance.Game1Trophy.SetActive(false);
                gameObject.SetActive(false);
            }
        }

        if (SpaceLobbyManager.Instance.GameNumber == GameNumber.Game2)
        {
            if (other.CompareTag("Game2TrophyHolder"))
            {
                VoiceOverManager.Instance.LastButtonSelected = LastButtonSelected.Any;
                VoiceOverManager.Instance.InvokeLastButtonSelected();
                TrophyManager.Instance.AddGameTrophyPresented((int)GameNumber.Game2);
                SpaceLobbyManager.Instance.GameNumber = GameNumber.None;
                SpaceLobbyManager.Instance.Game2TrophyHologram.SetActive(false);
                SpaceLobbyManager.Instance.Game2Trophy.SetActive(false);
                gameObject.SetActive(false);
            }
        }

        if (SpaceLobbyManager.Instance.GameNumber == GameNumber.Game3)
        {
            if (other.CompareTag("Game3TrophyHolder"))
            {
                VoiceOverManager.Instance.LastButtonSelected = LastButtonSelected.Any;
                VoiceOverManager.Instance.InvokeLastButtonSelected();
                TrophyManager.Instance.AddGameTrophyPresented((int)GameNumber.Game3);
                SpaceLobbyManager.Instance.GameNumber = GameNumber.None;
                SpaceLobbyManager.Instance.Game3TrophyHologram.SetActive(false);
                SpaceLobbyManager.Instance.Game3Trophy.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }

    #endregion
}