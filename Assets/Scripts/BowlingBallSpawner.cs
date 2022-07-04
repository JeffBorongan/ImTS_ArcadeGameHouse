using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BowlingBallSpawner : Spawner
{
    [SerializeField] private XRInteractionManager _xrInteractionManager;
    public override void Spawn()
    {
        base.Spawn();
        _spawnedObject.GetComponent<XRGrabInteractable>().interactionManager = _xrInteractionManager;
    }
}
