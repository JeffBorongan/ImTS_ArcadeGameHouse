using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialOnCollide : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material _material;
    private Material _oldMaterial;

    private void OnCollisionEnter(Collision collision)
    {
        _oldMaterial = _meshRenderer.material;
        _meshRenderer.material = _material;
    }

    private void OnCollisionExit(Collision collision)
    {
        _meshRenderer.material = _oldMaterial;
    }
}
