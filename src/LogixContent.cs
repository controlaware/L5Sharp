﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using L5Sharp.Components;
using L5Sharp.Core;
using L5Sharp.Enums;
using L5Sharp.Extensions;

namespace L5Sharp;

/// <summary>
/// A simple wrapper around a given <see cref="XElement"/>, which is expected to be the root RSLogix5000Content element
/// of the L5X file.
/// </summary>
public class LogixContent
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="element"></param>
    /// <exception cref="ArgumentNullException"></exception>
    private LogixContent(XElement element)
    {
        L5X = new L5X(element);

        DataTypes = new LogixContainer<DataType>(L5X.Controller);
        Instructions = new LogixContainer<AddOnInstruction>(L5X.Controller);
        Modules = new LogixContainer<Module>(L5X.Controller);
        Tags = new LogixContainer<Tag>(L5X.Controller);
        Programs = new LogixContainer<Program>(L5X.Controller);
        Tasks = new LogixContainer<LogixTask>(L5X.Controller);
    }

    /// <summary>
    /// Creates a new <see cref="LogixContent"/> by loading the contents of the provide file name.
    /// </summary>
    /// <param name="fileName">The full path, including file name, to the L5X file to load.</param>
    /// <returns>A new <see cref="LogixContent"/> containing the contents of the specified file.</returns>
    /// <exception cref="ArgumentException">The string is null or empty.</exception>
    /// <remarks>
    /// This factory method uses the <see cref="XDocument.Load(string)"/> to load the contents of the xml file into
    /// memory. This means that this method is subject to the same exceptions that could be generated by loading the
    /// XDocument. Once loaded, validation is performed to ensure the content adheres to the specified L5X Schema files.
    /// </remarks>
    public static LogixContent Load(string fileName) => new(XElement.Load(fileName));

    /// <summary>
    /// Creates a new <see cref="LogixContent"/> with the provided L5X string content.
    /// </summary>
    /// <param name="text">The string that contains the L5X content to parse.</param>
    /// <returns>A new <see cref="LogixContent"/> containing the contents of the specified string.</returns>
    /// <exception cref="ArgumentException">The string is null or empty.</exception>
    /// <remarks>
    /// This factory method uses the <see cref="XDocument.Parse(string)"/> to load the contents of the xml file into
    /// memory. This means that this method is subject to the same exceptions that could be generated by parsing the
    /// XDocument. Once parsed, validation is performed to ensure the content adheres to the specified L5X Schema files.
    /// </remarks>
    public static LogixContent Parse(string text) => new(XElement.Parse(text));

    /// <summary>
    /// Creates a new <see cref="LogixContent"/> with the provided logix component as the target type.
    /// </summary>
    /// <param name="target">The L5X target component of the resulting content.</param>
    /// <returns>A <see cref="LogixContent"/> containing the component as the target of the L5X.</returns>
    public static LogixContent Export(ILogixComponent target)
    {
        var content = new XElement(L5XName.RSLogix5000Content);
        content.Add(new XAttribute(L5XName.SchemaRevision, new Revision().ToString()));
        content.Add(new XAttribute(L5XName.TargetName, target.Name));
        content.Add(new XAttribute(L5XName.TargetType, target.GetType().LogixTypeName()));
        content.Add(new XAttribute(L5XName.ContainsContext, target.GetType() != typeof(Controller)));
        content.Add(new XAttribute(L5XName.Owner, Environment.UserName));
        content.Add(new XAttribute(L5XName.ExportDate, DateTime.Now.ToString(L5X.DateTimeFormat)));

        var component = LogixSerializer.Serialize(target);
        component.AddFirst(new XAttribute(L5XName.Use, Use.Target));
        content.Add(component);

        return new LogixContent(content);
    }

    /// <summary>
    /// The root L5X content containing all raw XML data for the <see cref="LogixContent"/>.
    /// </summary>
    /// <remarks>
    /// <see cref="L5X"/> inherits from <see cref="XElement"/> and adds some helper properties and methods
    /// for interacting with the root content of the L5X file.
    /// </remarks>
    public L5X L5X { get; }

    /// <summary>
    /// 
    /// </summary>
    public Controller Controller => LogixSerializer.Deserialize<Controller>(L5X.Controller);

    /// <summary>
    ///  Gets the collection of <see cref="DataType"/> components found in the L5X file.
    /// </summary>
    /// <value>A <see cref="ILogixCollection{TComponent}"/> of <see cref="DataType"/> components.</value>
    public ILogixCollection<DataType> DataTypes { get; }

    /// <summary>
    ///  Gets the collection of <see cref="AddOnInstruction"/> components found in the L5X file.
    /// </summary>
    /// <value>A <see cref="ILogixCollection{TComponent}"/> of <see cref="AddOnInstruction"/> components.</value>
    public ILogixCollection<AddOnInstruction> Instructions { get; }

    /// <summary>
    ///  Gets the collection of <see cref="Module"/> components found in the L5X file.
    /// </summary>
    /// <value>A <see cref="ILogixCollection{TComponent}"/> of <see cref="Module"/> components.</value>
    public ILogixCollection<Module> Modules { get; }

    /// <summary>
    ///  Gets the collection of Controller <see cref="Tags"/> components found in the L5X file.
    /// </summary>
    /// <value>A <see cref="ILogixCollection{TComponent}"/> of <see cref="Tags"/> components.</value>
    /// <remarks>To access program specific tag collection user the <see cref="Programs"/> collection.</remarks>
    public ILogixCollection<Tag> Tags { get; }

    /// <summary>
    ///  Gets the collection of <see cref="Program"/> components found in the L5X file.
    /// </summary>
    /// <value>A <see cref="ILogixCollection{TComponent}"/> of <see cref="Program"/> components.</value>
    public ILogixCollection<Program> Programs { get; }

    /// <summary>
    ///  Gets the collection of <see cref="LogixTask"/> components found in the L5X file.
    /// </summary>
    /// <value>A <see cref="ILogixCollection{TComponent}"/> of <see cref="LogixTask"/> components.</value>
    public ILogixCollection<LogixTask> Tasks { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <returns></returns>
    public IEnumerable<TEntity> Find<TEntity>() where TEntity : class => 
        L5X.Descendants(typeof(TEntity).LogixTypeName()).Select(LogixSerializer.Deserialize<TEntity>);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tagName"></param>
    /// <returns></returns>
    public TagMember FindTag(TagName tagName)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tagName"></param>
    /// <typeparam name="TLogixType"></typeparam>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public TagMember<TLogixType> FindTag<TLogixType>(TagName tagName) where TLogixType : LogixType
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tagName"></param>
    /// <typeparam name="TLogixType"></typeparam>
    /// <returns></returns>
    public TagMember<TLogixType> FindTags<TLogixType>(TagName tagName) where TLogixType : LogixType
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TLogixType"></typeparam>
    /// <returns></returns>
    public IEnumerable<TagMember<TLogixType>> FindTags<TLogixType>() where TLogixType : LogixType
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="overwrite"></param>
    /// <exception cref="ArgumentException"></exception>
    public void Import(string fileName, bool overwrite = false)
    {
        if (string.IsNullOrEmpty(fileName))
            throw new ArgumentException("FileName can not be null or empty.", nameof(fileName));

        var content = Load(fileName);

        Import(content, overwrite);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="content"></param>
    /// <param name="overwrite"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public void Import(LogixContent content, bool overwrite = false)
    {
        if (content is null)
            throw new ArgumentNullException(nameof(content));

        if (L5X.ContainsContext is true)
            throw new InvalidOperationException("The target L5X not a project file which does not support importing.");

        if (content.L5X.ContainsContext is false)
            throw new InvalidOperationException("The source L5X does not contain context to a specific component.");

        L5X.Merge(content.L5X, overwrite);
    }

    /// <summary>
    /// Serialize this <see cref="LogixContent"/> to a file, overwriting an existing file, if it exists.
    /// </summary>
    /// <param name="fileName">A string that contains the name of the file.</param>
    public void Save(string fileName)
    {
        var declaration = new XDeclaration("1.0", "UTF-8", "yes");
        var document = new XDocument(declaration);
        document.Add(L5X);
        document.Save(fileName);
    }

    /// <inheritdoc />
    public override string ToString() => L5X.ToString();
}