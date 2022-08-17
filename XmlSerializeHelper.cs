using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.IO;

namespace SerializationHelper
{
    public class XmlSerializeHelper
    {
        public static string SerializeObject(object TValue, bool omitNameSpace = false)
        {
            if (TValue is null)
            {
                return null;
            }
            try
            {
                if (omitNameSpace)
                {
                    var emptyNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                    var serializer = new XmlSerializer(TValue.GetType());
                    var settings = new XmlWriterSettings();
                    settings.Indent = true;
                    settings.OmitXmlDeclaration = true;

                    using (var stream = new StringWriter())
                    using (var writer = XmlWriter.Create(stream, settings))
                    {
                        serializer.Serialize(writer, TValue, emptyNamespaces);
                        return stream.ToString();
                    }
                }
                XmlSerializer xmlSerializer = new XmlSerializer(TValue.GetType());
                using (StringWriter stringWriter = new StringWriter())
                {
                    using (XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter)
                    {
                        Formatting = Formatting.Indented,
                        Namespaces = false
                    })
                    {
                        xmlSerializer.Serialize(stringWriter, TValue);
                        return stringWriter.ToString();
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public static T DeserializeObject<T>(string value) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                using (TextReader textReader = new StringReader(value))
                {
                    T t = new T();
                    t = (T)xmlSerializer.Deserialize(textReader);
                    return t;
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
