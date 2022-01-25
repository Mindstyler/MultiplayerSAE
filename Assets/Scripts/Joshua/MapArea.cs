using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using Photon.Pun;

public class MapArea : MonoBehaviour
{
    public enum State
    {
        Neutral,
        Captured,
    }

    private List<PointAreaCollider> pointAreaColliderList;
    private State state;
    private float captureProgress;
    private float pointProgress;
    private int player;

    private void Awake()
    {
        pointAreaColliderList = new List<PointAreaCollider>();

        foreach (Transform child in transform)
        {
            PointAreaCollider pointAreaCollider = child.GetComponent<PointAreaCollider>();
            if (pointAreaCollider != null)
            {
                pointAreaColliderList.Add(pointAreaCollider);
            }
        }

        //foreach (PhotonView playerId in )
        //{

        //}

        state = State.Neutral;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Neutral:
                List<PlayerPointAreas> playerPointAreasInsideList = new List<PlayerPointAreas>();

                foreach (PointAreaCollider pointAreaCollider in pointAreaColliderList)
                {
                    foreach (PlayerPointAreas playerPointAreas in pointAreaCollider.GetPlayerPointAreasList())
                    {
                        if (!playerPointAreasInsideList.Contains(playerPointAreas))
                        {
                            playerPointAreasInsideList.Add(playerPointAreas);
                        }
                        for (int i = 0; i < playerPointAreasInsideList.Count; i++)
                        {
                            Debug.Log(playerPointAreasInsideList.Contains(playerPointAreas));
                        }
                    }
                }



                float captureProgressSpeed = 0.5f;
                if (playerPointAreasInsideList.Count == 1)
                {

                    captureProgress += captureProgressSpeed * Time.deltaTime;
                }


                Debug.Log("playerCountInsideArea: " + playerPointAreasInsideList.Count + "; progress: " + captureProgress);

                if (captureProgress >= 1f)
                {
                    state = State.Captured;
                    Debug.Log("Captured!");
                }
                break;
            case State.Captured:

                List<PlayerPointAreas> playerPointsInsideList = new List<PlayerPointAreas>();

                foreach (PointAreaCollider pointAreaCollider in pointAreaColliderList)
                {
                    foreach (PlayerPointAreas playerPointAreas in pointAreaCollider.GetPlayerPointAreasList())
                    {
                        if (!playerPointsInsideList.Contains(playerPointAreas))
                        {
                            playerPointsInsideList.Add(playerPointAreas);
                        }
                    }
                }

                float pointProgressSpeed = 1f;
                if (playerPointsInsideList.Count == 1)
                {
                    captureProgress += pointProgressSpeed * Time.deltaTime;
                    //pointProgress += playerPointsInsideList.Count * pointProgressSpeed * Time.deltaTime;
                    Debug.Log("playerCountInsideArea: " + playerPointsInsideList.Count + "; progress: " + pointProgress);
                    if (pointProgress >= 25f)
                    {
                        SceneManager.LoadScene(3);
                    }
                }


                break;
        }
    }
}
