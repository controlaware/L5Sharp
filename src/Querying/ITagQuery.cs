﻿namespace L5Sharp.Querying
{
    /// <summary>
    /// A fluent <see cref="IComponentQuery{TResult}"/> that adds advanced querying for <see cref="ITag{TDataType}"/>
    /// elements within the L5X context.  
    /// </summary>
    public interface ITagQuery : IComponentQuery<ITag<IDataType>>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDataType"></typeparam>
        /// <returns></returns>
        public ITagQuery WithType<TDataType>() where TDataType : IDataType;
    }
}