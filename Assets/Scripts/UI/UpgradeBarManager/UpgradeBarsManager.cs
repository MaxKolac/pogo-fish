using System;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeBarsManager : MonoBehaviour
{
    [SerializeField] private GameObject durationBarPrefab;
    [SerializeField] private int barCount;
    private readonly Dictionary<int, DurationBarDictionaryEntry> durationBarDictionary = new();
    private readonly List<Vector2> durationBarPositions = new();

    private void Start()
    {
        for (int i = 0; i < barCount; i++)
        {
            GameObject newBar = Instantiate(durationBarPrefab);
            UpgradeDurationBar newBarScript = newBar.GetComponent<UpgradeDurationBar>();
            newBar.transform.parent = transform;
            durationBarDictionary.Add(i, new DurationBarDictionaryEntry(newBar, newBarScript));
            durationBarPositions.Add(new Vector2(-2, 15.25f - i));
            durationBarDictionary[i].SetPosition(durationBarPositions[i]);
        }
    }

    private void OnEnable()
    {
        Actions.OnTimedUpgradeExpires += RearrangeBars;
    }

    private void OnDisable()
    {
        Actions.OnTimedUpgradeExpires -= RearrangeBars;
        foreach (DurationBarDictionaryEntry entry in durationBarDictionary.Values)
            entry.Release();
    }

    /// <summary>
    /// Reserves one of the DurationBars and returns its ID so that the caller can reference their DurationBar.
    /// </summary>
    public int ReserveBar()
    {
        int id = -1;
        for (int i = 0; i < barCount; i++)
        {
            DurationBarDictionaryEntry entry = durationBarDictionary[i];
            if (!entry.isReserved)
            {
                id = i;
                entry.Reserve();
                return id;
            }
        }
        Debug.LogError("All UpgradeDurationBars are reserved! Time to panic!!! Increase their count and restart the game.");
        return id;
    }

    /// <summary>
    /// Removes reservation on the DurationBar.
    /// </summary>
    /// <param name="id">The ID of the DurationBar.</param>
    public void ReleaseBar(int id) => durationBarDictionary[id].Release();

    /// <summary>
    /// Arranges active UpgradeBars visually in a column. Call this when one of the UpgradeBars expires and leaves a gap afterwards.
    /// </summary>
    public void RearrangeBars()
    {
        int i = 0;
        foreach (DurationBarDictionaryEntry entry in durationBarDictionary.Values)
        {
            if (entry.isReserved)
            {
                entry.SetPosition(durationBarPositions[i]);
                i++;
            }
        }
    }

    public DurationBarDictionaryEntry GetBarDictionaryEntry(int id) => durationBarDictionary[id];
    public GameObject GetBarGameObject(int id) => durationBarDictionary[id].barGameObject;
    public UpgradeDurationBar GetBarScript(int id) => durationBarDictionary[id].barScript;
}
