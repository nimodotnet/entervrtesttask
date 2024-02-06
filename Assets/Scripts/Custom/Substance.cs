using UnityEngine;

public class Substance : MonoBehaviour
{
    public string Name => name;
    //public float OnSpawnMinDistance => OnSpawnMinDistance;

    //public SubstanceShape PhysicalShapePrefab => physicalShapePrefab;

    [SerializeField] new string name;
    //[SerializeField] float onSpawnMinDistance;

    //[SerializeField] SubstanceShape physicalShapePrefab;

    //ObjectPool<SubstanceShape> shapesPool;

    bool isTriggered;

    private void Awake()
    {
       // shapesPool = new ObjectPool<SubstanceShape>(physicalShapePrefab, 4);
    }

    //public SubstanceShape GetPhysicalShape()
    //{
    //    //return shapesPool.GetObject();
    //}

    public void Interact(string trigger)
    {
        if (isTriggered) return;

        isTriggered = true;

        var reaction = InteractionManager.Instance.GetReaction(trigger, name);
        reaction.Simulate(gameObject);
    }
}
