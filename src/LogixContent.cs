﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using L5Sharp.Components;
using L5Sharp.Extensions;
using L5Sharp.Serialization;
using L5Sharp.Utilities;

namespace L5Sharp
{
    /// <summary>
    /// A simple wrapper around a given <see cref="XElement"/>, which is expected to be the root RSLogix5000Content element
    /// of the L5X file. This class implements <see cref="ILogixContent"/> API.
    /// </summary>
    public class LogixContent : ILogixContent
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        /// <exception cref="ArgumentNullException"></exception>
        private LogixContent(XElement element)
        {
            if (element is null)
                throw new ArgumentNullException(nameof(element));

            if (element.Name != L5XName.RSLogix5000Content)
                throw new ArgumentException($"Expecting root element name of {L5XName.RSLogix5000Content}.");

            L5X = new L5X(element);
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
        /// Creates a new <see cref="LogixContent"/> with the provided logix component as the exported target type.
        /// </summary>
        /// <param name="component">The logix component to export.</param>
        /// <returns>A <see cref="LogixContent"/> containing the exported component as the target of the L5X.</returns>
        public static LogixContent Export<TComponent>(TComponent component) where TComponent : ILogixComponent =>
            new(L5X.Create(component));

        /// <summary>
        /// The root L5X content containing all raw XML data loaded, parsed, or created under the current <see cref="LogixContent"/>
        /// </summary>
        /// <remarks>
        /// <see cref="L5X"/> inherits from <see cref="XElement"/> and adds some helper properties and methods
        /// for interacting with the root content of the L5X file.
        /// </remarks>
        public L5X L5X { get; }

        /// <inheritdoc />
        public Controller? Controller
        {
            get
            {
                var element = L5X.Element(L5XName.Controller);
                return element is not null ? LogixSerializer.Deserialize<Controller>(element) : null;
            }
        }

        /// <inheritdoc />
        public ILogixComponentCollection<DataType> DataTypes()
        {
            var container = L5X.Descendants(L5XName.DataTypes).FirstOrDefault();
            return new ComponentCollection<DataType>(L5X, container);
        }

        /// <inheritdoc />
        public ILogixComponentCollection<AddOnInstruction> Instructions()
        {
            var container = L5X.Descendants(L5XName.AddOnInstructionDefinitions).FirstOrDefault();
            return new ComponentCollection<AddOnInstruction>(L5X, container);
        }

        /// <inheritdoc />
        public ILogixComponentCollection<Module> Modules()
        {
            var container = L5X.Descendants(L5XName.Modules).FirstOrDefault();
            return new ComponentCollection<Module>(L5X, container);
        }

        /// <inheritdoc />
        public ILogixComponentCollection<Program> Programs()
        {
            var container = L5X.Descendants(L5XName.Programs).FirstOrDefault();
            return new ComponentCollection<Program>(L5X, container);
        }

        /// <inheritdoc />
        public ILogixComponentCollection<Tag> Tags()
        {
            var container = L5X.Element(L5XName.Controller)?.Element(L5XName.Tags);
            return new ComponentCollection<Tag>(L5X, container);
        }

        /// <inheritdoc />
        public ILogixComponentCollection<Tag> Tags(string programName)
        {
            var program = L5X.Descendants(L5XName.Program).FirstOrDefault(e => e.LogixName() == programName);

            if (program is null)
                throw new ArgumentException(
                    $"No program with '{programName}' found in the current L5X. Add the component via Programs() before accessing Tags.");

            var container = program.Descendants(L5XName.Tags).FirstOrDefault();

            return new ComponentCollection<Tag>(L5X, container);
        }

        /// <inheritdoc />
        public ILogixComponentCollection<Routine> Routines(string programName)
        {
            var program = L5X.Descendants(L5XName.Program).FirstOrDefault(e => e.LogixName() == programName);

            if (program is null)
                throw new ArgumentException(
                    $"No program with '{programName}' found in the current L5X. Add the component via Programs() before accessing Routines.");

            var container = program.Descendants(L5XName.Routines).FirstOrDefault();

            return new ComponentCollection<Routine>(L5X, container);
        }

        /// <inheritdoc />
        public ILogixComponentCollection<LogixTask> Tasks()
        {
            var container = L5X.Descendants(L5XName.Tasks).FirstOrDefault();
            return new ComponentCollection<LogixTask>(L5X, container);
        }

        /// <inheritdoc />
        public IEnumerable<TEntity> Query<TEntity>() where TEntity : class
        {
            var name = typeof(TEntity).GetLogixName();
            var serializer = LogixSerializer.GetSerializer<TEntity>();
            return L5X.Descendants(name).Select(e => serializer.Deserialize(e));
        }

        /// <inheritdoc />
        public void Import(LogixContent content, bool overwrite = false)
        {
            if (content is null)
                throw new ArgumentNullException(nameof(content));

            if (L5X.ContainsContext is true)
                throw new InvalidOperationException(
                    "The current L5X is contains context to a specific component type, which means it is not able to import other content." +
                    " Only project level L5X content files support this import feature.");

            DataTypes().Import(content.DataTypes().ToList(), overwrite);
            Instructions().Import(content.Instructions().ToList(), overwrite);
            Modules().Import(content.Modules().ToList(), overwrite);
            //todo This is an issue because programs don't contain the routines and tags. We should just make program contain those components.
            Programs().Import(content.Programs().ToList(), overwrite);
            Tasks().Import(content.Tasks().ToList(), overwrite);
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
}