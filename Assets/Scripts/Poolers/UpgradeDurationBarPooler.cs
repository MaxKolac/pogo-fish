using System.Collections.Generic;
using UnityEngine;

//Leave this alone until more upgrades that need a bar are added. Cant test that rn.
public class UpgradeDurationBarPooler : MonoBehaviour
{
    [SerializeField] private GameObject barPrefab;
    [SerializeField] private int barsAvailableForUse;
    private Queue<GameObject> activeBars = new Queue<GameObject>();
    private Queue<GameObject> inactiveBars = new Queue<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < barsAvailableForUse; i++)
        {
            inactiveBars.Enqueue(Instantiate(barPrefab));
        }
    }

    public void ActivateBar(out GameObject barGameObj, out UpgradeDurationBar barScript)
    {
        if (inactiveBars.Count == 0)
            InstantiateAdditionalBar();
        barGameObj = inactiveBars.Dequeue();
        barScript = barGameObj.GetComponent<UpgradeDurationBar>();
        activeBars.Enqueue(barGameObj);
    }

    private void InstantiateAdditionalBar()
    {
        Debug.LogWarning($"UpgradeDurationBarPooler ran out of Bars! Instantiating additional Bar, but just FYI, the game should probably NOT need this many bars.");
        GameObject newBar = Instantiate(barPrefab);
        inactiveBars.Enqueue(newBar);
    }
}
