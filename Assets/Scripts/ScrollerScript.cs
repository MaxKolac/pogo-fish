using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollerScript : MonoBehaviour
{
    public GameObject player;
    public float height = 0f;

    /// <summary>Once the Player reached above this Y coordinate, height will be incremented.</summary>
    private float heightBarrier;
    private Vector3 oldPosition = Vector3.zero;

    void Awake()
    {
        heightBarrier = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 4f, 0)).y;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y < heightBarrier) return;
        height += Mathf.Max(0, player.transform.position.y - oldPosition.y);
        oldPosition = player.transform.position;
    }
}
