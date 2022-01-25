using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAreaCollider : MonoBehaviour
{
    private List<PlayerPointAreas> playerPointAreasList = new List<PlayerPointAreas>();

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<PlayerPointAreas>(out PlayerPointAreas playerPointAreas))
        {
            playerPointAreasList.Add(playerPointAreas);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.TryGetComponent<PlayerPointAreas>(out PlayerPointAreas playerPointAreas))
        {
            playerPointAreasList.Remove(playerPointAreas);
        }
    }

    public List<PlayerPointAreas> GetPlayerPointAreasList()
    {
        return playerPointAreasList;
    }
}
