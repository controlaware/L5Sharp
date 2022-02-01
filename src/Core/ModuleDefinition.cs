﻿using System.Collections.Generic;
using L5Sharp.Enums;

namespace L5Sharp.Core
{
    /// <summary>
    /// A set of properties that defines the Logix <see cref="IModule"/>.  
    /// </summary>
    public class ModuleDefinition
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="catalogNumber"></param>
        /// <param name="vendor"></param>
        /// <param name="productType"></param>
        /// <param name="productCode"></param>
        /// <param name="revisions"></param>
        /// <param name="categories"></param>
        /// <param name="ports"></param>
        /// <param name="description"></param>
        public ModuleDefinition(CatalogNumber catalogNumber, Vendor vendor, ProductType productType,
            ushort productCode,
            IEnumerable<Revision> revisions,
            IEnumerable<ModuleCategory> categories,
            IEnumerable<Port> ports,
            string description)
        {
            CatalogNumber = catalogNumber;
            Vendor = vendor;
            ProductType = productType;
            ProductCode = productCode;
            Revisions = revisions;
            Categories = categories;
            Ports = ports;
            Description = description;
        }

        /// <summary>
        /// Gets the <see cref="CatalogNumber"/> of the current <see cref="ModuleDefinition"/>.
        /// </summary>
        public CatalogNumber CatalogNumber { get; }

        /// <summary>
        /// Gets the description of the <see cref="ModuleDefinition"/>.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the <see cref="Vendor"/> of the current <see cref="ModuleDefinition"/>.
        /// </summary>
        public Vendor Vendor { get; }

        /// <summary>
        /// Gets the <see cref="ProductType"/> of the current <see cref="ModuleDefinition"/>.
        /// </summary>
        public ProductType ProductType { get; }

        /// <summary>
        /// Gets the <see cref="ushort"/> code number that identifies the <see cref="ModuleDefinition"/>. 
        /// </summary>
        public ushort ProductCode { get; }

        /// <summary>
        /// Gets a collection of valid <see cref="Revisions"/> for the <see cref="ModuleDefinition"/>.
        /// </summary>
        public IEnumerable<Revision> Revisions { get; }
        
        /// <summary>
        /// Gets a collection of <see cref="ModuleCategory"/> that the Module.
        /// </summary>
        public IEnumerable<ModuleCategory> Categories { get; }

        /// <summary>
        /// Gets the set of <see cref="Ports"/> that should be defined on the Module.
        /// </summary>
        public IEnumerable<Port> Ports { get; }
    }
}