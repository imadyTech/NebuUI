using System;
using UnityEngine;
using System.Reflection;

namespace imady.NebuUI
{
    /// <summary>
    /// APP里涉及的模拟对象池子，基于ObjectPoolBase实现；
    /// </summary>
    public class NbuObjectPool : NbuPoolBase
    {
        private string defaultObjectPrefabPathPrefix;
        public NbuObjectPool(string pathPrefix)
        {
            defaultObjectPrefabPathPrefix = pathPrefix;
        }

        public new void Dispose()
        {
            base.Dispose();
        }


        #region PUBLIC METHODS
        NbuResourcePathAttribute attributeCache = null;
        /// <summary>
        /// 根据object type激活实例（默认会产生对象的克隆体）
        /// </summary>
        /// <param name="nebuObjectType">被激活资源的类型，指定了NbuResourcePathAttribute。</param>
        public GameObject WakeObject(Type nebuObjectType, Transform parent)
        {
            try
            {
                attributeCache = nebuObjectType.GetCustomAttribute<NbuResourcePathAttribute>();
                if (attributeCache == null) 
                    throw new ArgumentException("The object parameter did not includes 'NbuResourcePathAttribute'.");
                var path = $"{defaultObjectPrefabPathPrefix}{attributeCache.PrefabSubPath}{nebuObjectType.Name}";
                //注意：所有对象可能会有克隆体 - 与ViewPool方式不同。
                var objectResult = base.WakeMdyObject(path, parent, true);
                return objectResult;
            }
            catch (Exception e)
            {
                Debug.LogError($"[GAMEOBJECT INSTANTIATE ERROR] '{nebuObjectType.Name}' : {e}");
                return null;
            }

        }

        /// <summary>
        /// 根据对象的层级名称（完整的Resources下路径+名称）激活实例
        /// </summary>
        /// <param name="objectFullName">被激活资源的名称（不包含路径prefix）</param>
        public GameObject WakeObject(string objectFullName, Transform parent)
        {
            return base.WakeMdyObject(defaultObjectPrefabPathPrefix + objectFullName, parent, true);
        }

        public void HibernateObject(NebuEventUnityObjectBase Object)
        {
            base.HibernateObject(Object.gameObject);
        }
        #endregion
    }
}
