using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
[CreateAssetMenu(fileName = "New Item", menuName = "Items/Item Data")]
public class ItemData : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private int maxStackCount = 1;
    [SerializeField] private string itemName;
    [SerializeField] private ItemType itemType;
    [SerializeField] private string itemDescription;
    
    [Inject]
    public Sprite Sprite => sprite;
    public int MaxStackCount => maxStackCount;
    public string ItemDescription => itemDescription;
    public string Name => itemName;
    public ItemType ItemType => itemType;
    private string _lastSpriteName;
    private void OnValidate()
    {
        if (sprite != null && sprite.name != _lastSpriteName)
        {
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(this);
            string currentName = System.IO.Path.GetFileNameWithoutExtension(assetPath);

            if (currentName != sprite.name)
            {
                UnityEditor.AssetDatabase.RenameAsset(assetPath, sprite.name);
            }
            _lastSpriteName = sprite.name;
        }
    }
}

public enum ItemType
{
    CraftItem,
    QuestItem,
    Weapon,
    Potion,
    Armor,
    
}