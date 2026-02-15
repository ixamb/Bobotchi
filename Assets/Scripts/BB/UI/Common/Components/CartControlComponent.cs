using System;
using BB.Entities;
using Core.Runtime.Extensions;
using Core.Runtime.Services.Views;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BB.UI.Common.Components
{
    public sealed class CartControlComponent : ViewComponent<CartControlDto>
    {
        [SerializeField] private TMP_Text quantity;
        [SerializeField] private Button incrementButton;
        [SerializeField] private Button decrementButton;
        [Space] [SerializeField] private Button validateButton;
        [SerializeField] private TMP_Text textValidateButton;

        private CartControlDto _cartControlDto;

        public event Action<PurchasableEntity, uint> OnAddToCartClick;

        public override void Initialize(CartControlDto cartControlDto)
        {
            _cartControlDto = cartControlDto;

            incrementButton.onClick.ReplaceListeners(OnIncrementButton);
            decrementButton.onClick.ReplaceListeners(OnDecrementButton);

            incrementButton.interactable = _cartControlDto.IsValid() && _cartControlDto.IsIncrementable();
            decrementButton.interactable = _cartControlDto.IsValid() && _cartControlDto.IsDecrementable();
            validateButton.interactable = _cartControlDto.IsValid();

            validateButton.onClick.ReplaceListeners(() =>
                OnAddToCartClick?.Invoke(_cartControlDto.Entity, _cartControlDto.Quantity));

            UpdateTextFields();
        }

        private void OnIncrementButton()
        {
            IncrementSelectedQuantity();
            SetInteractableButtons();
        }

        private void OnDecrementButton()
        {
            DecrementSelectedQuantity();
            SetInteractableButtons();
        }

        private void IncrementSelectedQuantity()
        {
            _cartControlDto.Increment();
            UpdateTextFields();
        }

        private void DecrementSelectedQuantity()
        {
            _cartControlDto.Decrement();
            UpdateTextFields();
        }

        private void SetInteractableButtons()
        {
            incrementButton.interactable = _cartControlDto.IsIncrementable();
            decrementButton.interactable = _cartControlDto.IsDecrementable();
        }

        private void UpdateTextFields()
        {
            quantity.text = _cartControlDto.Quantity.ToString();
            textValidateButton.text = $"Ajouter pour {_cartControlDto.Entity.Price * _cartControlDto.Quantity}€";
        }
    }

    public sealed class CartControlDto : ComponentDto
    {
        public PurchasableEntity Entity { get; }
        public uint Quantity { get; private set; }

        private float _totalCart;
        private readonly float _balance;

        public CartControlDto(PurchasableEntity entity, uint quantity, float totalCart, float balance)
        {
            Quantity = quantity;
            Entity = entity;
            _totalCart = totalCart;
            _balance = balance;
        }

        public void Increment()
        {
            Quantity++;
            _totalCart += Entity.Price;
        }

        public void Decrement()
        {
            Quantity--;
            _totalCart -= Entity.Price;
        }
        
        public bool IsValid() => _totalCart <= _balance;

        public bool IsIncrementable()
        {
            if (Entity.SinglePurchase && Quantity >= 1)
                return false;
            
            return _totalCart + Entity.Price <= _balance;
        }

        public bool IsDecrementable(bool fromOne = false) => Quantity > (fromOne ? 1 : 0);
    }
}