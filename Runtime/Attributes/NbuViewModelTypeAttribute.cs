using System;

namespace imady.NebuUI
{
    /// <summary>
    /// Indicate the type of view for a certain viewmodel
    /// 指示viewmodel所对应的view视图类型
    /// </summary>
    public class NbuViewModelTypeAttribute : System.Attribute
    {
        /// <summary>
        /// Note: this tells which view should the viewmodel be mapped
        /// </summary>
        public Type NbuViewType { get; set; }

        public NbuViewModelTypeAttribute(Type viewtype)
        {
            this.NbuViewType = viewtype;
        }
    }
}
