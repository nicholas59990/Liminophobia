using UnityEngine;


// Because they are data containers we need to replace as Scriptable
[CreateAssetMenu] // Allows you to create instances of this scriptable object by clicking "Create" in Unity
public class ItemSO : ScriptableObject
{
    public string itemName;

    // Which stat this item affects
    public StatToChange statToChange = StatToChange.none;

    // How much to change the stat
    public float amountToChangeStat;

    // How long the effect will last
    public float duration = 5f;

    // The 3D prefab
    public GameObject dropPrefab;

    // Untick for items that shouldn't be consumable (e.g., keys)
    public bool consumable = true;

    public void UseItem()
    {

        PlayerMovement player = GameObject.Find("Capsule").GetComponent<PlayerMovement>();

        // For if stamina is selected
        if (statToChange == StatToChange.stamina)
        {
            player.ChangeStamina(amountToChangeStat, duration);
        }

        // For if speed is selected
        if (statToChange == StatToChange.speed)
        {
           player.ChangeSpeed(amountToChangeStat, duration);
        }

        // For if stamina and speed selected
        if (statToChange == StatToChange.both)
        {
           player.ChangeSpeed(amountToChangeStat, duration);
           player.ChangeStamina(amountToChangeStat, duration);
        }
    }

    public enum StatToChange
    {
        none, // No changes
        stamina, // More/less stamina for a limited time
        speed, // More/less speed at the same cost of stamina if walking normally
        both // More/less stamina and speed for a limited time
    };
}
