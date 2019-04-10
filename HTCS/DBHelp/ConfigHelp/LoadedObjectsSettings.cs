using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelp.ConfigHelp
{
    /// <summary>
    /// 加载对象配置类
    /// </summary>
    public class LoadedObjectsSettings : ConfigurationSection
    {
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("", IsDefaultCollection = true)]
        public ObjectElementCollection ObjectElements
        {
            get { return (ObjectElementCollection)this[""]; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static LoadedObjectsSettings GetSection()
        {
            return ConfigurationHelper.GetSection("loadedObjectsSettings") as LoadedObjectsSettings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loadedObjectsSettings"></param>
        /// <returns></returns>
        public static LoadedObjectsSettings GetSection(string loadedObjectsSettings)
        {
            return ConfigurationHelper.GetSection(loadedObjectsSettings) as LoadedObjectsSettings;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ObjectElement GetObjectElement(string name)
        {
            foreach (ObjectElement item in this.ObjectElements)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }
            return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ObjectElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ObjectElement();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            ObjectElement objectElement = (ObjectElement)element;
            return objectElement;
        }

    }

    /// <summary>
    /// 对象元素
    /// </summary>
    public class ObjectElement : ConfigurationElement
    {
        /// <summary>
        /// 名称
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// 命名空间
        /// </summary>
        [ConfigurationProperty("assemblyName", IsRequired = true)]
        public string AssemblyName
        {
            get { return (string)this["assemblyName"]; }
            set { this["assemblyName"] = value; }
        }

        /// <summary>
        /// 类型
        /// </summary>
        [ConfigurationProperty("typeName", IsRequired = true)]
        public string TypeName
        {
            get { return (string)this["typeName"]; }
            set { this["typeName"] = value; }
        }
    }
}
