﻿using System;
using System.Collections.Generic;
using L5Sharp.Core;
using L5Sharp.Enums;

namespace L5Sharp
{
    public interface IAddOnDefined : IDataType, IInstruction
    {
        RoutineType Type { get; }
        Revision Revision { get; }
        string RevisionExtension { get; }
        string RevisionNote { get; }
        string Vendor { get; }
        bool ExecutePreScan { get; }
        bool ExecutePostScan { get; }
        bool ExecuteEnableInFalse { get; }
        DateTime CreatedDate { get; }
        string CreatedBy { get; }
        DateTime EditedDate { get; }
        string EditedBy { get; }
        Revision SoftwareRevision { get; }
        string AdditionalHelpText { get; }
        bool IsEncrypted { get; }
        IRoutine Logic { get; }
        IEnumerable<IMember<IDataType>> Members { get; }
        new IParameters Parameters { get; }
        ITags LocalTags { get; }
        IEnumerable<IRoutine> Routines { get; }
        void SetName(string name);
        void SetDescription(string description);
        void SetRevision(Revision revision);
        void SetRevisionExtension(string revisionExtension);
        void SetRevisionNote(string note);
        void SetVendor(string vendor);
        public void AddPreScanRoutine(RoutineType type);
        public void AddPostScanRoutine(RoutineType type);
    }
}