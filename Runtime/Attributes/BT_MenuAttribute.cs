using System;

namespace Common.BehaviourTrees
{
    /// <summary>
    /// An attribute used to control various interfaces implementations visibility within <see cref="BT_Blackboard"/> menu
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class BT_MenuAttribute : Attribute
    {
        public readonly string fileName;
        public readonly string menuPath;
        public readonly int group;

        public BT_MenuAttribute(
            string fileName = null,
            string menuPath = BT_MenuPath.Custom,
            int group = BT_MenuGroup.Custom
        )
        {
            this.fileName = fileName;
            this.menuPath = menuPath;
            this.group = group;
        }
    }

    public static class BT_MenuAttributeExtensions
    {
        public static string GetFileNameOrDefault(this BT_MenuAttribute self, string defaultValue = null)
        {
            return (self == null || self.fileName == null) ? defaultValue : self.fileName;
        }

        public static string GetMenuPathOrDefault(this BT_MenuAttribute self, string defaultValue = BT_MenuPath.Custom)
        {
            return (self == null || self.menuPath == null) ? defaultValue : self.menuPath;
        }

        public static int GetGroupOrDefault(this BT_MenuAttribute self, int defaultValue = BT_MenuGroup.Custom)
        {
            return (self == null) ? defaultValue : self.group;
        }
    }
}