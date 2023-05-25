public class PickableObjectPooler : GenericPooler<PickableObjectType>
{
    protected override void Awake()
    {
        base.Awake();
        Actions.OnPickableObjectDespawn += DespawnObject;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        Actions.OnPickableObjectDespawn -= DespawnObject;
    }
}
