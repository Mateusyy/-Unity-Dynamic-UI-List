using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pool", menuName = "Pooler")]
public class ListItemsPoolSO : ScriptableObject
{
    [SerializeField] private ListItem prefab;
    private List<ListItem> _poolObjects = new List<ListItem>();

    private void OnEnable()
    {
        _poolObjects.Clear();
    }

    private void OnDisable()
    {
        _poolObjects.Clear();
    }

    public ListItem Spawn(Transform parent = null)
    {
        ListItem item;
        if (_poolObjects.Any(p => !p.gameObject.activeSelf))
        {
            item = _poolObjects.First(p => !p.gameObject.activeSelf);
        }
        else
        {
            item = parent == null ? Instantiate(prefab) : Instantiate(prefab, parent);
            _poolObjects.Add(item);
        }

        item.gameObject.SetActive(true);
        return item;
    }

    public void Despawn()
    {
        for (int i = 0; i < _poolObjects.Count; i++)
        {
            _poolObjects[i].gameObject.SetActive(false);
        }
    }
}
