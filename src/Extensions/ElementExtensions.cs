﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Linq;
using L5Sharp.Common;
using L5Sharp.Serialization.Providers;
using L5Sharp.Types;

namespace L5Sharp.Extensions
{
    internal static class ElementExtensions
    {
        private static readonly Dictionary<string, IXDataTypeProvider> TypeProviders = new()
        {
            { LogixNames.Member, new ArrayElementTypeProvider() },
            { LogixNames.Element, new ArrayElementTypeProvider() },
            { LogixNames.Tag, new ArrayElementTypeProvider() },
        };

        /// <summary>
        /// Gets the component name value from the current <see cref="XElement"/> instance.
        /// </summary>
        /// <param name="element">The current <see cref="XElement"/> instance.</param>
        /// <returns>The string value of the 'Name' attribute.</returns>
        /// <exception cref="InvalidOperationException">When the current element does not have a name value.</exception>
        public static string GetComponentName(this XElement element) =>
            element.Attribute(LogixNames.Name)?.Value
            ?? throw new InvalidOperationException("The current element does not have an attribute with name 'Name'");

        /// <summary>
        /// Gets the component description value from the current <see cref="XElement"/> instance.
        /// </summary>
        /// <param name="element">The current <see cref="XElement"/> instance.</param>
        /// <returns>The string value of the 'Description' element if it exists; otherwise, empty string.</returns>
        public static string GetComponentDescription(this XElement element) =>
            element.Element(LogixNames.Description)?.Value
            ?? string.Empty;

        /// <summary>
        /// Helper for getting the current element's data type instance.
        /// </summary>
        /// <param name="element">The current element.</param>
        /// <returns>
        /// If the data type is known or defined in the current XDocument, then an instance of  <see cref="IDataType"/>
        /// representing that type; otherwise, and instance of <see cref="Types.Undefined"/>.
        /// </returns>
        public static IDataType GetDataType(this XElement element)
        {
            return TypeProviders.TryGetValue(element.Name.ToString(), out var provider)
                ? provider.FindType(element)
                : new Undefined();
        }

        /// <summary>
        /// Gets the value of a specified <see cref="XAttribute"/> from the current <see cref="XElement"/> instance.
        /// </summary>
        /// <param name="element">The current <see cref="XElement"/> instance.</param>
        /// <param name="propertySelector">
        /// An expression that selects the property of the specified type which is the <see cref="XAttribute"/> to retrieve.
        /// </param>
        /// <param name="nameOverride">
        /// A string override to replace the default property name of the <see cref="XAttribute"/> to retrieve.
        /// </param>
        /// <typeparam name="TComplex">The type of the object for which the property is selected.</typeparam>
        /// <typeparam name="TProperty">The type of the property being selected from the <see cref="XElement"/>.</typeparam>
        /// <returns>
        /// The parsed value of the <see cref="XAttribute"/> determined by the property selector of the complex type. 
        /// </returns>
        /// <exception cref="ArgumentException">When propertySelector is not a <see cref="MemberExpression"/>.</exception>
        /// <remarks>
        /// This method acts as a helper for getting and parsing a given <see cref="XAttribute"/> value.
        /// Since (for the most part) the name of our properties match the L5X name, we use expressions to determine the
        /// the name and type to be parsed. The name of the attribute to get can be overriden with
        /// the <see cref="nameOverride"/> argument. 
        /// </remarks>
        /// <seealso cref="GetElement{TComplex,TProperty}"/>
        public static TProperty? GetAttribute<TComplex, TProperty>(this XElement element,
            Expression<Func<TComplex, TProperty>> propertySelector, string? nameOverride = null)
        {
            if (propertySelector.Body is not MemberExpression memberExpression)
                throw new ArgumentException($"PropertySelector expression must be {typeof(MemberExpression)}");

            var name = nameOverride ?? memberExpression.Member.Name;

            var value = element.Attribute(name)?.Value;

            return value is not null ? LogixParser.Parse<TProperty>(value) : default;
        }

        /// <summary>
        /// Gets the value of a specified <see cref="XAttribute"/> from the current <see cref="XElement"/> instance.
        /// </summary>
        /// <param name="element">The current <see cref="XElement"/> instance.</param>
        /// <param name="propertySelector">
        /// An expression that selects the property of the specified type which is the <see cref="XAttribute"/> to retrieve.
        /// </param>
        /// <param name="nameOverride">
        /// A string override to replace the default property name of the <see cref="XAttribute"/> to retrieve.
        /// </param>
        /// <typeparam name="TComplex">The type of the object for which the property is selected.</typeparam>
        /// <typeparam name="TProperty">The type of the property being selected from the <see cref="XElement"/>.</typeparam>
        /// <returns>
        /// The parsed value of the <see cref="XAttribute"/> determined by the property selector of the complex type. 
        /// </returns>
        /// <exception cref="ArgumentException">When propertySelector is not a <see cref="MemberExpression"/>.</exception>
        /// <remarks>
        /// This method acts as a helper for getting and parsing a given <see cref="XAttribute"/> value.
        /// Since (for the most part) the name of our properties match the L5X name, we use expressions to determine the
        /// the name and type to be parsed. The name of the attribute to get can be overriden with
        /// the <c>nameOverride</c> argument. 
        /// </remarks>
        /// <seealso cref="GetElement{TComplex,TProperty}"/>
        public static TProperty? GetElement<TComplex, TProperty>(this XElement element,
            Expression<Func<TComplex, TProperty>> propertySelector, string? nameOverride = null)
        {
            if (propertySelector.Body is not MemberExpression memberExpression)
                throw new ArgumentException($"PropertySelector expression must be {typeof(MemberExpression)}");

            var name = nameOverride ?? memberExpression.Member.Name;

            var value = element.Element(name)?.Value;

            return value is not null ? LogixParser.Parse<TProperty>(value) : default;
        }

        /// <summary>
        /// Adds the specified property of a given complex object as an <see cref="XAttribute"/>
        /// to the current <see cref="XElement"/> instance. 
        /// </summary>
        /// <param name="element">The current <see cref="XElement"/> instance.</param>
        /// <param name="instance">The instance of the type from which the selected property will be added.</param>
        /// <param name="propertySelector">
        /// An expression that selects the property of the provided type to add to the <see cref="XElement"/>.
        /// </param>
        /// <param name="addCondition">
        /// An optional condition that specifies whether to add the select property to the <see cref="XElement"/>.
        /// Properties that are null or an empty string will not be added.
        /// </param>
        /// <param name="nameOverride">
        /// An optional string name that overrides the name of the content being add  the <see cref="XElement"/> instance.
        /// </param>
        /// <typeparam name="TComplex">The type of the object for which a member property will be added.</typeparam>
        /// <typeparam name="TProperty">The type of the property being added to the <c>XElement</c>.</typeparam>
        /// <exception cref="ArgumentNullException">When instance is null.</exception>
        /// <exception cref="ArgumentException">When propertySelector is not a <see cref="MemberExpression"/>.</exception>
        /// <remarks>
        /// This is a helper method for quickly adding content as an <see cref="XAttribute"/> to a given XElement.
        /// By default, this method will add an attribute with the name of the property specified by <see cref="propertySelector"/>.
        /// This can be overriden with <see cref="nameOverride"/> argument.
        /// An optional predicate <see cref="addCondition"/> can be specified to add the attribute only when the condition is met.
        /// If the property is null or empty, then no content will be added.
        /// </remarks>
        public static void AddAttribute<TComplex, TProperty>(this XElement element, TComplex instance,
            Expression<Func<TComplex, TProperty>> propertySelector,
            Predicate<TComplex>? addCondition = null, string? nameOverride = null)
        {
            if (instance is null)
                throw new ArgumentNullException(nameof(instance));

            if (propertySelector.Body is not MemberExpression memberExpression)
                throw new ArgumentException($"PropertySelector expression must be {typeof(MemberExpression)}");

            if (addCondition is not null && !addCondition(instance)) return;

            var name = nameOverride ?? memberExpression.Member.Name;

            var value = propertySelector.Compile().Invoke(instance);

            if (string.IsNullOrEmpty(value?.ToString())) return;

            element.Add(new XAttribute(name, value));
        }

        /// <summary>
        /// Adds the specified property of a given complex object as a child <see cref="XElement"/>
        /// to the current <see cref="XElement"/> instance. 
        /// </summary>
        /// <param name="element">The current <see cref="XElement"/> instance.</param>
        /// <param name="instance">The instance of the type from which the selected property will be added.</param>
        /// <param name="propertySelector">
        /// An expression that selects the property of the provided type to add to the <see cref="XElement"/>.
        /// </param>
        /// <param name="addCondition">
        /// An optional condition that specifies whether to add the select property to the <see cref="XElement"/>.
        /// Properties that are null or an empty string will not be added.
        /// </param>
        /// <param name="nameOverride">
        /// An optional string name that overrides the name of the content being add  the <see cref="XElement"/> instance.
        /// </param>
        /// <typeparam name="TComplex">The type of the object for which a member property will be added.</typeparam>
        /// <typeparam name="TProperty">The type of the property being added to the <c>XElement</c>.</typeparam>
        /// <exception cref="ArgumentNullException">When instance is null.</exception>
        /// <exception cref="ArgumentException">When propertySelector is not a <see cref="MemberExpression"/>.</exception>
        /// <remarks>
        /// This is a helper method for quickly adding content as an <see cref="XElement"/> to a given <see cref="XElement"/>.
        /// By default, this method will add an element with the name of the property specified by <see cref="propertySelector"/>.
        /// This can be overriden with <see cref="nameOverride"/> argument.
        /// An optional predicate <see cref="addCondition"/> can be specified to add the attribute only when the condition is met.
        /// If the property is null or empty, then no content will be added.
        /// </remarks>
        public static void AddElement<TComplex, TProperty>(this XElement element, TComplex instance,
            Expression<Func<TComplex, TProperty>> propertySelector,
            Func<TComplex, bool>? addCondition = null, string? nameOverride = null)
        {
            if (instance is null)
                throw new ArgumentNullException(nameof(instance));

            if (propertySelector.Body is not MemberExpression memberExpression)
                throw new ArgumentException($"PropertySelector expression must be {typeof(MemberExpression)}");

            if (addCondition is not null && !addCondition(instance)) return;

            var name = nameOverride ?? memberExpression.Member.Name;

            var value = propertySelector.Compile().Invoke(instance);

            if (string.IsNullOrEmpty(value?.ToString())) return;

            element.Add(new XElement(name, new XCData(value.ToString())));
        }
    }
}