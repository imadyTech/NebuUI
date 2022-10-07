using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace imady.NebuUI
{
    /// <summary>
    /// 可以实现对象的SetActive、Instantiate、Destroy等方法。
    /// </summary>
    public class NbuPoolBase 
    {
        /// <summary>
        /// 缓存所有对象
        /// </summary>
        private List<GameObject> objectPool;

        private NbuResourceLoader loader;


        #region CONSTRUCTOR & DESTRUCTOR
        public NbuPoolBase()
        {
            objectPool = new List<GameObject>();
            loader = new NbuResourceLoader();
        }


        public virtual void Dispose()
        {
            foreach (GameObject obj in objectPool)
            {
                UnityEngine.Object.Destroy(obj);
            }
            objectPool.Clear();
        }
        #endregion

        //todo: 目前的objectpool不实现销毁功能

        /// <summary>
        /// 根据定义好的prefab名称（可能包含‘/’字符）查找对象（如果pool中已经存在，则只是取出而不重新实例化）
        /// </summary>
        /// <param name="objectPrefabPath">要求传入完整的prefab路径（例如“/Prefabs/UITemplate/xxxx”）</param>
        /// <returns></returns>
        protected virtual GameObject WakeMdyObject(string objectPrefabPath, Transform parentTransform, bool isNewInstance)
        {
            if (string.IsNullOrEmpty(objectPrefabPath))
                return null;
            var gameobject = objectPool
                .Where(o => o.name.Equals(objectPrefabPath))
                .FirstOrDefault();
            if (isNewInstance || gameobject == null)//cache missing, instantiate a new object
            {
                gameobject = CreateObject(objectPrefabPath, parentTransform);
                if (gameobject == null)
                {
                    Debug.LogWarning($"MdyObject对象 '{objectPrefabPath}' 加载失败！");
                    return null;
                }
            }

            gameobject.SetActive(true);
            return gameobject;
        }

        /// <summary>
        /// 生成指定类型的MdyObject对象实例（可能生成多重实例）
        /// </summary>
        /// <typeparam name="SourceType"></typeparam>
        /// <param name="objectPrefabPath">完整的prefab资源路径</param>
        protected GameObject CreateObject(string objectPrefabPath, Transform parentTransform)
        {
            if (string.IsNullOrEmpty(objectPrefabPath)) return null;

            try
            {
                var gameobject = loader.LoadPrefab(objectPrefabPath);
                if (gameobject == null) return null;

                var go = UnityEngine.Object.Instantiate(gameobject);
                go.name = objectPrefabPath;
                if (parentTransform != null) go.transform.SetParent(parentTransform);
                objectPool.Add(go);//入池
                return go;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
        }

        protected virtual void HibernateObject(GameObject obj)
        {
            if (objectPool.Contains(obj))
                obj.SetActive(false);
        }
    }
}