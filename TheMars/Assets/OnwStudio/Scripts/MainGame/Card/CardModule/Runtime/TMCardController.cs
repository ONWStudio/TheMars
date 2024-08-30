using Onw.Attribute;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;
namespace TMCard.Runtime
{
    // .. Controller
    [DisallowMultipleComponent]
    public sealed class TMCardController : MonoBehaviour
    {
        [FormerlySerializedAs("_cardController")]
        [Header("Model")]
        [SerializeField, InitializeRequireComponent]
        private TMCardModel _cardModel;

        [Header("View")]
        [SerializeField, InitializeRequireComponent]
        private TMCardViewer _cardViewer;

        private TMCardData _cardData;

        private void Start()
        {
            if (_cardModel.CardData)
            {
                SetCardData(_cardModel.CardData);
            }
            else
            {
                this
                    .ObserveEveryValueChanged(controller => _cardModel.CardData)
                    .Take(1)
                    .Subscribe(SetCardData)
                    .AddTo(this);
            }
            
            _cardModel.OnDragBeginCard.AddListener(() => _cardViewer.SetView(false));
            _cardModel.OnDragEndCard.AddListener(() => _cardViewer.SetView(true));
        }

        public void SetCardData(TMCardData cardData)
        {
            _cardData = cardData;
            _cardViewer.SetUI(cardData);
        }
    }
}
