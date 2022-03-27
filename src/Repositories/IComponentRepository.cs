﻿using L5Sharp.Querying;

namespace L5Sharp.Repositories
{
    /// <summary>
    /// A base for implementing write operations over the L5X.
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    public interface IComponentRepository<TComponent> : IComponentQuery<TComponent> where TComponent : ILogixComponent
    {
        /// <summary>
        /// Adds the provided component to the current logix context.
        /// </summary>
        /// <param name="component">The component to add to the context.</param>
        void Add(TComponent component);

        /// <summary>
        /// Removes the provided component from the current logix context.
        /// </summary>
        /// <param name="component">The component to remove from the context.</param>
        void Remove(TComponent component);

        /// <summary>
        /// Updates an existing component with the provided component in the current logix context.
        /// </summary>
        /// <param name="component">The component to update on the context.</param>
        void Update(TComponent component);
    }
}