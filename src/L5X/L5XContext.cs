﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using L5Sharp.Core;
using L5Sharp.Querying;
using L5Sharp.Serialization;

namespace L5Sharp.L5X
{
    /// <inheritdoc />
    public class L5XContext : ILogixContext
    {
        private readonly L5XContent _l5X;

        /// <summary>
        /// Creates a new <see cref="L5XContext"/> instance with the provided <see cref="L5XContent"/>.
        /// </summary>
        /// <param name="l5X">The <see cref="L5XContent"/> instance that represents the loaded L5X.</param>
        private L5XContext(L5XContent l5X)
        {
            _l5X = l5X ?? throw new ArgumentNullException(nameof(l5X));
        }

        /// <summary>
        /// Creates a new <see cref="L5XContext"/> by loading the contents of the provide file name.
        /// </summary>
        /// <param name="fileName">The full path, including file name, to the L5X file to load.</param>
        /// <returns>A new <see cref="L5XContext"/> containing the contents of the specified file.</returns>
        /// <exception cref="ArgumentException">The string is null or empty.</exception>
        /// <remarks>
        /// This factory method uses the <see cref="XDocument.Load(string)"/> to load the contents of the xml file into
        /// memory. This means that this method is subject to the same exceptions that could be generated by loading the
        /// XDocument. Once loaded, validation is performed to ensure the content adheres to the specified L5X Schema files.
        /// </remarks>
        public static L5XContext Load(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("Filename can not be null or empty");

            var document = new L5XContent(XElement.Load(fileName));

            return new L5XContext(document);
        }

        /// <summary>
        /// Creates a new <see cref="L5XContext"/> with the provided L5X string content.
        /// </summary>
        /// <param name="content">The string that contains the L5X content to parse.</param>
        /// <returns>A new <see cref="L5XContext"/> containing the contents of the specified string.</returns>
        /// <exception cref="ArgumentException">The string is null or empty.</exception>
        /// <remarks>
        /// This factory method uses the <see cref="XDocument.Parse(string)"/> to load the contents of the xml file into
        /// memory. This means that this method is subject to the same exceptions that could be generated by parsing the
        /// XDocument. Once parsed, validation is performed to ensure the content adheres to the specified L5X Schema files.
        /// </remarks>
        public static L5XContext Parse(string content)
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentException("Content can not be null or empty");

            var l5X = new L5XContent(XElement.Parse(content));

            return new L5XContext(l5X);
        }

        /// <inheritdoc />
        public IController? Controller() => _l5X.Controller is not null
            ? _l5X.Serializers.Get<ControllerSerializer>().Deserialize(_l5X.Controller)
            : null;

        /// <inheritdoc />
        public IComponentQuery<IComplexType> DataTypes()
        {
            var components = _l5X.DataTypes;
            var serializer = _l5X.Serializers.ForComponent<IComplexType>();
            return new ComponentQuery<IComplexType>(components, serializer);
        }

        /// <inheritdoc />
        public IEnumerable<IComplexType> DataTypes(Func<DataTypeQuery, DataTypeQuery> query)
        {
            var source = new DataTypeQuery(_l5X.DataTypes);
            var result = query.Invoke(source);
            return result.Execute(_l5X.Serializers.Get<DataTypeSerializer>());
        }

        /// <inheritdoc />
        public IComponentQuery<IModule> Modules()
        {
            var components = _l5X.Modules;
            var serializer = _l5X.Serializers.ForComponent<IModule>();
            return new ComponentQuery<IModule>(components, serializer);
        }


        /// <inheritdoc />
        public IEnumerable<IModule> Modules(Func<ModuleQuery, ModuleQuery> query)
        {
            var source = new ModuleQuery(_l5X.Modules);
            var result = query.Invoke(source);
            return result.Execute(_l5X.Serializers.Get<ModuleSerializer>());
        }

        /// <inheritdoc />
        public IComponentQuery<IAddOnInstruction> Instructions()
        {
            var components = _l5X.Instructions;
            var serializer = _l5X.Serializers.ForComponent<IAddOnInstruction>();
            return new ComponentQuery<IAddOnInstruction>(components, serializer);
        }

        /// <inheritdoc />
        public IEnumerable<IAddOnInstruction> Instructions(Func<InstructionQuery, InstructionQuery> query)
        {
            var source =
                new InstructionQuery(_l5X.Instructions);
            var result = query.Invoke(source);
            return result.Execute(_l5X.Serializers.Get<AddOnInstructionSerializer>());
        }

        /// <inheritdoc />
        public IComponentQuery<ITag<IDataType>> Tags()
        {
            var components = _l5X.Tags.Where(t => t.Parent?.Parent?.Name == L5XElement.Controller.ToString());
            var serializer = _l5X.Serializers.ForComponent<ITag<IDataType>>();
            return new ComponentQuery<ITag<IDataType>>(components, serializer);
        }

        /// <inheritdoc />
        public IEnumerable<ITag<IDataType>> Tags(Func<TagQuery, TagQuery> query)
        {
            var source = new TagQuery(_l5X.Tags);
            var result = query.Invoke(source);
            return result.Execute(_l5X.Serializers.Get<TagSerializer>());
        }

        /// <inheritdoc />
        public IComponentQuery<IProgram> Programs()
        {
            var components = _l5X.Programs;
            var serializer = _l5X.Serializers.ForComponent<IProgram>();
            return new ComponentQuery<IProgram>(components, serializer);
        }

        /// <inheritdoc />
        public IEnumerable<IProgram> Programs(Func<ProgramQuery, ProgramQuery> query)
        {
            var source = new ProgramQuery(_l5X.Programs);
            var result = query.Invoke(source);
            return result.Execute(_l5X.Serializers.Get<ProgramSerializer>());
        }

        /// <inheritdoc />
        public IComponentQuery<ITask> Tasks()
        {
            var components = _l5X.Tasks;
            var serializer = _l5X.Serializers.ForComponent<ITask>();
            return new ComponentQuery<ITask>(components, serializer);
        }

        /// <inheritdoc />
        public IEnumerable<ITask> Tasks(Func<TaskQuery, TaskQuery> query)
        {
            var source = new TaskQuery(_l5X.Tasks);
            var result = query.Invoke(source);
            return result.Execute(_l5X.Serializers.Get<TaskSerializer>());
        }

        /// <inheritdoc />
        public IEnumerable<Rung> Rungs(Func<RungQuery, RungQuery> query)
        {
            var source = new RungQuery(_l5X.Content.Descendants(L5XElement.Rung.ToString()));
            var result = query.Invoke(source);
            return result.Execute(_l5X.Serializers.Get<RungSerializer>());
        }

        /// <inheritdoc />
        public void Save(string fileName)
        {
            var declaration = new XDeclaration("1.0", "UTF-8", "yes");
            var document = new XDocument(declaration, _l5X.Content);
            document.Save(fileName);
        }

        /// <inheritdoc />
        public override string ToString() => _l5X.Content.ToString();
    }
}