
namespace imady.NebuUI
{
    /// <summary>
    /// Indicate the sub path of a view/object under the AppConfiguration.defaultUITemplatePrefabPath
    /// or AppConfiguration.defaultObjectPrefabPath (usually the "Resources/Prefabs" folder).
    /// </summary>
    public class NbuResourcePathAttribute : System.Attribute
    {
        /// <summary>
        /// Note: Do not include object name!!!
        /// </summary>
        public string PrefabSubPath { get; set; }

        public NbuResourcePathAttribute(string subpath)
        {
            this.PrefabSubPath = subpath;
        }
    }
}
