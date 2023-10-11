using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shop
{
    public class ShopPanel : MonoBehaviour
    {
        [SerializeField] private Transform itemsParent;
        [SerializeField] private ShopItemViewFactory shopItemViewFactory;

        private OpenSkinsChecker _openSkinsChecker;
        private SelectedSkinChecker _selectedSkinChecker;
        
        private readonly List<ShopItemView> _shopItems = new();

        public event Action<ShopItemView> ItemViewClicked;

        public void Initialize(OpenSkinsChecker openSkinsChecker, SelectedSkinChecker selectedSkinChecker)
        {
            _openSkinsChecker = openSkinsChecker;
            _selectedSkinChecker = selectedSkinChecker;
        }

        public void Show(IEnumerable<ShopItem> items)
        {
            Clear();
            
            foreach (var item in items)
            { 
                var spawnedItem = shopItemViewFactory.Get(item, itemsParent);
                spawnedItem.Initialize(item);
                spawnedItem.OnClick += OnItemViewClick;
                
                spawnedItem.Unselect();
                spawnedItem.UnHighlight();
                
                _openSkinsChecker.Visit(spawnedItem.Item);

                if (_openSkinsChecker.IsOpened)
                {
                    _selectedSkinChecker.Visit(spawnedItem.Item);

                    if (_selectedSkinChecker.IsSelected)
                    {
                        spawnedItem.Select();
                        spawnedItem.Highlight();
                        ItemViewClicked?.Invoke(spawnedItem);
                    }
                    
                    spawnedItem.Unlock();
                }
                else
                {
                    spawnedItem.Lock();
                }
                
                _shopItems.Add(spawnedItem);
            }
        }

        private void OnItemViewClick(ShopItemView itemView)
        {
            Highlight(itemView);
            ItemViewClicked?.Invoke(itemView);
        }

        private void Highlight(ShopItemView shopItemView)
        {
            foreach (var item in _shopItems)
            {
                item.UnHighlight();
            }

            shopItemView.Highlight();
        }

        public void Select(ShopItemView itemView)
        {
            foreach (var item in _shopItems)
            {
                item.Unselect();
            }
            
            itemView.Select();
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