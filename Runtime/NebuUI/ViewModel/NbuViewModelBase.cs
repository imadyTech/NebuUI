using System.Linq;
using System.Reflection;

namespace imady.NebuUI
{
    public class NbuViewModelBase
    {
        /// <summary>
        /// 数据模型自动匹配字段
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public virtual T TryMap<T>(object viewModel) where T: NbuViewModelBase
        {
            var properties = viewModel.GetType().GetProperties()
                .Where(a => a.GetCustomAttribute<NbuViewPropertyAttribute>() != null);
            var thisProperties = this.GetType().GetProperties()
                .Where(a => a.GetCustomAttribute<NbuViewPropertyAttribute>() != null);
            foreach (var property in properties)
            {
                foreach (var thisProperty in thisProperties)
                {
                    if (thisProperty.Name.Equals(property.Name))
                        thisProperty.SetValue(this, property.GetValue(viewModel));
                }
            }
            return this as T;
        }

    }
}
