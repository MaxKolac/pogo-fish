using UnityEngine;

public class DurationBarDictionaryEntry
{
    public GameObject barGameObject;
    public UpgradeDurationBar barScript;
    public bool isReserved = false;

    public DurationBarDictionaryEntry(GameObject durationBarObject, UpgradeDurationBar durationBarScript)
    {
        barGameObject = durationBarObject;
        barScript = durationBarScript;
    }

    public void SetPosition(Vector2 pos) => barGameObject.transform.position = pos;
    public void Reserve()
    {
        isReserved = true;
        barGameObject.SetActive(true);
    }
    public void Release()
    {
        isReserved = false;
        barGameObject.SetActive(false);
    }
}

