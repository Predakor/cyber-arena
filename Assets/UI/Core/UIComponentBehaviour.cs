using Assets.Scripts.Utils;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Core
{
    public abstract class UIComponentBehaviour : MonoBehaviour
    {
        [SerializeField] protected UIDocument document;

        private CancellationTokenSource _uiCts;
        protected CancellationToken UiCancellationToken => _uiCts?.Token ?? CancellationToken.None;

        protected virtual void Awake()
        {
            gameObject.EnsureComponent(out document);
        }

        private void OnEnable()
        {
            _uiCts = CancellationTokenSource.CreateLinkedTokenSource(destroyCancellationToken, UiCancellationToken);
            UIBinder.Bind(this, document.rootVisualElement);
            OnUIEnabled();
        }

        private void OnDisable()
        {
            _uiCts.Cancel();
            _uiCts.Dispose();
            _uiCts = null;

            UIBinder.Unbind(this);
            OnUIDisabled();
        }

        protected virtual void OnUIEnabled() { }
        protected virtual void OnUIDisabled() { }
    }

}
