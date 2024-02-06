using Fusion;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance;

    [SerializeField] List<ReactionData> reactions = new List<ReactionData>();

    private void Awake()
    {
        Instance = this;
    }

    public Reaction GetReaction(string trigger, string target)
    {
        var reactionData = reactions.SingleOrDefault(x => x.Trigger == trigger && x.Target == target);
        return reactionData != null ? reactionData.Reaction : null;

    }
}

[System.Serializable]
public class ReactionData
{
    public string Trigger;
    public string Target;

    public Reaction Reaction;
}

