using Microsoft.AspNetCore.OData.Formatter.Serialization;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.OData;
using System.Reflection;

namespace OData.Annotations.Example
{
    public class CustomODataResourceSerializer(IODataSerializerProvider provider) : ODataResourceSerializer(provider)
    {
        public override ODataResource CreateResource(SelectExpandNode selectExpandNode, ResourceContext resourceContext)
        {
            var resource = base.CreateResource(selectExpandNode, resourceContext);
            if (resource != null)
            {
                var resourceContextPropDictionary = GetPropertiesDictionary(resourceContext.ResourceInstance);

                foreach (var prop in resource.Properties)
                {
                    var propNameToLower = prop.Name.ToLower();

                    if (resourceContextPropDictionary.TryGetValue($"{propNameToLower}name", out object lookupNamePropValue))
                    {
                        prop.InstanceAnnotations.Add(new ODataInstanceAnnotation("lookup.name", new ODataPrimitiveValue(lookupNamePropValue)));
                    }
                }
            }
            return resource;
        }

        private Dictionary<string, object> GetPropertiesDictionary(object obj)
        {
            Dictionary<string, object> propertiesDictionary = new Dictionary<string, object>();
            PropertyInfo[] properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                object value = property.GetValue(obj);
                propertiesDictionary.Add(property.Name.ToLower(), value);
            }

            return propertiesDictionary;
        }
    }
}
