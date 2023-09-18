using System.Collections.Generic;
using UnityEngine;

namespace Shop
{
    public class ShopPanel : MonoBehaviour
    {
        [SerializeField] private Transform itemsParent;
        [SerializeField] private ShopItemViewFactory shopItemViewFactory;
        
        private List<ShopItemView> _shopItems = new();

        public void Show(IEnumerable<ShopItem> items)
        {
            Clear();
            
            foreach (var item in items)
            { 
                var spawnedItem = shopItemViewFactory.Get(item, itemsParent);
                spawnedItem.OnClick += OnItemViewClick;
                
                spawnedItem.Unselect();
                spawnedItem.UnHighlight();
                
                _shopItems.Add(spawnedItem);
            }
        }

        private void OnItemViewClick(ShopItemView obj)
        {
            
        }

        private void Clear()
        {
            foreach (var item in _shopItems)
            {
                item.OnClick -= OnItemViewClick;
                Destroy(item.gameObject);
            }
            
            _shopItems.Clear();
        }
    }
}