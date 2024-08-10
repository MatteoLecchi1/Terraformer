using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryBuilding
{
    public Transform cargoSpawnPositionTransform;
    public GameObject cargoPrefab;
    [SerializeField]
    private Collider deliveryArea;

    public GameObject SpawnCargo()
    {
        GameObject cargo = GameObject.Instantiate(cargoPrefab,cargoSpawnPositionTransform.position,cargoSpawnPositionTransform.rotation);
        return cargo;
    }
    public DeliveryBuilding()
    {

    }
}
