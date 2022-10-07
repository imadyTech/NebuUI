using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace imady.NebuUI
{
    /// <summary>
    /// UIView对象池子，基于ObjectPoolBase实现；
    /// ViewPool层实现UIView的Stack管理。
    /// </summary>
    public class NbuUIViewPool : NbuPoolBase
    {
        /// <summary>
        /// 用于实现navigation功能
        /// </summary>
        private Stack<GameObject> viewStack = new Stack<GameObject>();
        /// <summary>
        /// 返回最顶层的view
        /// </summary>
        private GameObject currentObject => viewStack?.Peek();
        public INbuUIView currentView { get => currentObject.GetComponent<INbuUIView>(); }

        private string defaultObjectPrefabPathPrefix;


        public NbuUIViewPool(string pathPrefix)
        {
            defaultObjectPrefabPathPrefix = pathPrefix;
        }

        public override void Dispose()
        {
            viewStack.Clear();
            base.Dispose();
        }


        #region PUBLIC METHODS
        NbuResourcePathAttribute attributeCache = null;
        /// <summary>
        /// 根据ViewType激活MdyView
        /// </summary>
        /// <param name="mdyViewType">MdyViewBase类型</param>
        public GameObject WakeView(Type mdyViewType, Transform parent, bool isNewInstance)
        {
            try
            {
                attributeCache = mdyViewType.GetCustomAttribute<NbuResourcePathAttribute>();
                var path = $"{defaultObjectPrefabPathPrefix}{attributeCache.PrefabSubPath}{mdyViewType.Name}";
                var objectResult = base.WakeMdyObject(path, parent, isNewInstance);
                if (!isNewInstance) viewStack.Push(objectResult);
                Debug.Log($"<<- ViewStack PUSH : {viewStack.Peek()} : Count: {viewStack.Count}");
                return objectResult;
            }
            catch (Exception e)
            {
                //throw new ArgumentException("The type of LeeView may not include an attribute of 'LeeViewResourcePath'. Please check again.");
                Debug.LogException(e);
                Debug.LogError($"[ERROR] The type of {mdyViewType.Name} did not include an attribute of 'LeeViewResourcePathAttribute'.");
                return null;
            }

        }

        /// <summary>
        /// 根据View的层级名称（完整的Resources下路径+名称）激活UIView实例
        /// </summary>
        /// <param name="leeViewType">leeView类型</param>
        public GameObject WakeView(string viewFullName, Transform parent, bool isNewInstance)
        {
            return base.WakeMdyObject($"{defaultObjectPrefabPathPrefix}{viewFullName}", parent, isNewInstance);
        }

        public void HibernateView()
        {
            if (currentObject != null)
            {
                currentObject.SetActive(false);
                Debug.Log($"->> ViewStack POP: {viewStack.Peek()} : Count: {viewStack.Count}");
                viewStack.Pop();

                //currentView.SetState(imadyApp.imadyObjectState.Cooling);
            }
        }
        #endregion
    }
}
