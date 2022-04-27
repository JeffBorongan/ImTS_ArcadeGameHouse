using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Tile : MonoBehaviour
{
    #region Parameters

    [SerializeField] private MeshRenderer tileMeshRenderer = null;
    private TileColor tileColor = TileColor.None;

    #endregion

    #region Encapsulations

    public MeshRenderer TileMeshRenderer { get => tileMeshRenderer;}
    public TileColor TileColor { get => tileColor; set => tileColor = value; }

    #endregion

    #region Function with Delay

    private IEnumerator FunctionWithDelay(float waitTime, UnityAction function)
    {
        yield return new WaitForSeconds(waitTime);
        function.Invoke();
    }

    #endregion

    #region Milestone Color Change

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Head"))
        {
            if (TileColor == TileColor.Green)
            {
                TileGameManager.Instance.VFXGreen.SetActive(true);
                StartCoroutine(FunctionWithDelay(1f, () => TileGameManager.Instance.VFXGreen.SetActive(false)));
            }

            if (TileColor == TileColor.Orange)
            {
                TileGameManager.Instance.VFXOrange.SetActive(true);
                StartCoroutine(FunctionWithDelay(1f, () => TileGameManager.Instance.VFXOrange.SetActive(false)));
            }

            if (TileColor == TileColor.Pink)
            {
                TileGameManager.Instance.VFXPink.SetActive(true);
                StartCoroutine(FunctionWithDelay(1f, () => TileGameManager.Instance.VFXPink.SetActive(false)));
            }

            TileGameManager.Instance.FloorMeshRenderer.material.SetTexture("_BaseMap", TileGameManager.Instance.FloorBaseMaps[(int)TileColor + 1]);
            TileGameManager.Instance.FloorMeshRenderer.material.SetTexture("_BumpMap", TileGameManager.Instance.FloorNormalMaps[(int)TileColor + 1]);
            TileGameManager.Instance.FloorMeshRenderer.material.SetTexture("_EmissionMap", TileGameManager.Instance.FloorEmissionMaps[(int)TileColor + 1]);
            TileGameManager.Instance.AddTilePassed();
        }
    }

    #endregion
}