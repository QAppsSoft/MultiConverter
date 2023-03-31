using System;
using System.Reactive.Disposables;
using System.Xml;
using System.Xml.Linq;

namespace MultiConverter.Services.Settings;

public static class XmlExtensions
{
    public static string AttributeOrThrow(this XElement source, string elementName)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (elementName == null) throw new ArgumentNullException(nameof(elementName));

        var element = source.Attribute(elementName);

        if (element == null)
            throw new ArgumentNullException($"{elementName} does not exist");

        return element.Value;
    }

    public static XElement ElementOrThrow(this XDocument source, string elementName)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (elementName == null) throw new ArgumentNullException(nameof(elementName));

        var element = source.Element(elementName);

        if (element == null)
            throw new ArgumentNullException($"{elementName} does not exist");

        return element;
    }

    public static string ElementOrThrow(this XElement source, string elementName)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (elementName == null) throw new ArgumentNullException(nameof(elementName));

        var element = source.Element(elementName);

        if (element == null)
            throw new ArgumentNullException($"{elementName} does not exist");
        return element.Value;
    }

    public static IDisposable WriteElement(this XmlTextWriter source, string elementName)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (elementName == null) throw new ArgumentNullException(nameof(elementName));

        source.WriteStartElement(elementName);
        return Disposable.Create(source.WriteEndElement);
    }
}
