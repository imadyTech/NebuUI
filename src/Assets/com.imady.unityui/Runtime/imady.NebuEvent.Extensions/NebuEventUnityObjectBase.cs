using imady.NebuEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;


namespace imady.NebuUI
{
    /// <summary>
    /// The base NebuEventObject which is capable to communicate with registered providers/observers.
    /// Since unity gameObject must inheritated from MonoBehaviour, NebuEventUnityObjectBase is implemented via an adaptor pattern (to NebuEventInterfaceObjectBase). 
    /// </summary>
    public abstract partial class NebuEventUnityObjectBase : MonoBehaviour,  INebuEventObjectBase
    {
        public static string SubscribeLog = string.Empty;

        public static string MessageLog = string.Empty;

        private NebuEventInterfaceObjectBase baseEventObject;

        protected NebuEventManager eventsystemCache;

        protected virtual void Awake()
        {
            baseEventObject = new NebuEventInterfaceObjectBase(this.GetType());
        }


        #region imady.NebuyEvent System METHODS IMPLEMENTATION
        //--------------------------------

        public virtual INebuEventObjectBase AddEventManager(NebuEventManager eventSystem)
        {
            eventSystem.Register(this);
            eventsystemCache = eventSystem;
            return this;
        }

        /// <summary>
        /// Unsubscriber实际上还没实现
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected class Unsubscriber<T> : IDisposable where T : INebuEventObjectBase
        {
            private List<T> _observers;
            private T _observer;

            public Unsubscriber(List<T> observers, T observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
        #endregion


        #region Wrapper of baseEventObject
        public bool isProvider => baseEventObject.isProvider;

        public bool isObserver => baseEventObject.isObserver;

        public IEnumerable<Type> providerInterfaces => baseEventObject.providerInterfaces;

        public IEnumerable<Type> observerInterfaces => baseEventObject.observerInterfaces;

        public bool isObservingMessage(Type interfaceType) => baseEventObject.isObservingMessage(interfaceType);

        public void NotifyObservers(NebuMessageBase message) => baseEventObject.NotifyObservers(message);

        public void Subscribe(INebuEventObjectBase observer) => baseEventObject.Subscribe(observer);
        #endregion
    }
}
