using UnityEngine;
public class TagPlayerAction : GoapActionBase
{
    void Awake()
    {
        actionName = "Tag Player";

        cost = 1f;

        preMask = GoapBits.Mask(GoapFact.HasWeapon, GoapFact.AtPlayer);
        addMask = GoapBits.Mask(GoapFact.PlayerTagged);

        delMask = GoapBits.Mask(GoapFact.AtPlayer);
    }

    public override GoapStatus Tick(GoapContext ctx)
    {
        Debug.Log("GOAP: Tagged intruder!");
        return GoapStatus.Success;
    }
}
