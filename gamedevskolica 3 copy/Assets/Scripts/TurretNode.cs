using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretNode : MonoBehaviour
{
    private Turret? spawnedTurret = null;
    public bool isOccupied { get => spawnedTurret != null; }
    public void SpawnTurret(Turret turretPrefab)
    {
        if (isOccupied) return;
        var spawnedTurret = Instantiate(turretPrefab, transform.position, Quaternion.identity, transform); //instantiate pravi nov objekat(koji objekat, pozicija, rotacija, koji je parent)
        this.spawnedTurret = spawnedTurret;
        GameManager.Instance.Coins -= spawnedTurret.stats.price;
        PlayerController.LeaveShop();
    }
}