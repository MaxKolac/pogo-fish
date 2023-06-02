using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBarsManager : MonoBehaviour
{
    [SerializeField] private GameObject durationBarPrefab;
    [SerializeField] private int barCount;
    [Header("Debugging")]
    private Dictionary<int, GameObject> durationBars;
    private Dictionary<int, UpgradeDurationBar> durationBarScripts;
    private Dictionary<int, bool> isBarReserved;
    private List<Vector2> durationBarPositions;

    private void Start()
    {
        durationBars = new Dictionary<int, GameObject>();
        durationBarScripts = new Dictionary<int, UpgradeDurationBar>();
        isBarReserved = new Dictionary<int, bool>();
        durationBarPositions = new List<Vector2>();
        for (int i = 0; i < barCount; i++)
        {
            GameObject newBar = Instantiate(durationBarPrefab);
            UpgradeDurationBar newBarScript = newBar.GetComponent<UpgradeDurationBar>();
            newBar.transform.parent = transform;
            newBar.SetActive(false);
            durationBars.Add(i, newBar);
            durationBarScripts.Add(i, newBarScript);
            isBarReserved.Add(i, false);
            durationBarPositions.Add(new Vector2(-2, 15.25f - i));
            durationBars[i].transform.position = durationBarPositions[i];
        }
    }

    private void OnEnable()
    {
        Actions.OnTimedUpgradeExpires += RearrangeBars;
    }

    private void OnDisable()
    {
        Actions.OnTimedUpgradeExpires -= RearrangeBars;
        for (int i = 0; i < barCount; i++)
        {
            durationBars[i].SetActive(false);
            isBarReserved[i] = false;
        }
    }

    /// <summary>
    /// Reserves one of the DurationBars and returns its ID so that the caller can reference their DurationBar.
    /// </summary>
    public int ReserveBar()
    {
        int id = -1;
        for (int i = 0; i < barCount; i++)
        {
            if (!isBarReserved[i])
            {
                id = i;
                isBarReserved[id] = true;
                durationBars[id].SetActive(true);
                return id;
            }
        }
        Debug.LogError("All UpgradeDurationBars are reserved! Time to panic!!!");
        return id;
    }

    /// <summary>
    /// Removes reservation on the DurationBar.
    /// </summary>
    /// <param name="id">The ID of the DurationBar.</param>
    public void ReleaseBar(int id)
    {
        isBarReserved[id] = false;
        durationBars[id].SetActive(false);
    }

    /// <summary>
    /// Arranges active UpgradeBars visually in a column. Call this when one of the UpgradeBars expires and leaves a gap afterwards.
    /// </summary>
    public void RearrangeBars()
    {
        int j = 0;
        for (int i = 0; i < barCount; i++)
        {
            if (isBarReserved[i])
            {
                durationBars[i].transform.position = durationBarPositions[j];
                j++;
            }
        }
    }

    public GameObject GetBarGameObject(int id) => durationBars[id];
    public UpgradeDurationBar GetBarScript(int id) => durationBarScripts[id];
    //public UpgradeDurationBar GetBarScript(int id) => durationBars[id].GetComponent<UpgradeDurationBar>();
}
