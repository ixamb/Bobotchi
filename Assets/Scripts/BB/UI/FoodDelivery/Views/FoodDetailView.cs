using BB.Actions;
using BB.Data;
using BB.Entities;
using BB.Services.Modules.GameData;
using BB.UI.Common;
using BB.UI.Common.Components;

namespace BB.UI.FoodDelivery.Views
{
    public sealed class FoodDetailView : PurchaseDetailView
    {
        public override void Initialize(PurchasableEntity purchasableEntity)
        {
            if (purchasableEntity is not Food)
                return;
            base.Initialize(purchasableEntity);
        }

        protected override void InitializePurchasableCharacteristics(PurchasableEntity purchasableEntity)
        {
            if (purchasableEntity is not Food food)
                return;
            
            food.EffectActions.ForEach(effect =>
            {
                var characteristicEffect = Instantiate(characteristicPrefab, characteristicsParent);
                characteristicEffect.Initialize(
                    new CharacteristicComponentDto
                    {
                        Sprite = GameDataService.Instance.GetSprite(StatIconEntryKey(effect.AffectedState)),
                        Description = $"{(effect.Affection >= 0 ? "+" : string.Empty)}{effect.Affection} : {effect.AffectedState.ToTranslatedString()}",
                    });
                
                CharacteristicComponents.Add(characteristicEffect);
            });
            return;

            string StatIconEntryKey(CharacterStateStat state)
            {
                return state switch
                {
                    CharacterStateStat.Energy => "state-energy-icon",
                    CharacterStateStat.Hunger => "state-hunger-icon",
                    CharacterStateStat.Esteem => "state-esteem-icon",
                    _ => string.Empty
                };
            }
        }
    }
}