using UnityEngine;

namespace imady.NebuUI
{
    /// <summary>
    /// 所有NbuObject都需要实现的一些通用接口
    /// </summary>
    public interface INbuObject
    {
        GameObject gameObject { get; }

        /// <summary>
        /// 设置父对象的transform（影响Hierarchy层级关系）
        /// </summary>
        /// <param name="parent"></param>
        INbuObject SetParent(Transform parent);

        /// <summary>
        /// 隐藏自己（回调UIManager相应的对象池关闭方法，影响objectStack。
        /// </summary>
        void Hide();

        void Show();
    }
}
