﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Wabbajack.Common.Serialization.Json;
using Wabbajack.Lib;
using Wabbajack.Lib.ModListRegistry;

namespace Wabbajack.Server.DTOs
{
    [JsonName("DetailedStatus")]
    public class DetailedStatus
    {
        public string Name { get; set; }
        public DateTime Checked { get; set; } = DateTime.UtcNow;
        public List<DetailedStatusItem> Archives { get; set; }
        public DownloadMetadata DownloadMetaData { get; set; }
        public bool HasFailures { get; set; }
        public string MachineName { get; set; }
    }

    [JsonName("DetailedStatusItem")]
    public class DetailedStatusItem
    {
        public bool IsFailing { get; set; }
        public Archive Archive { get; set; }

        public string Name => string.IsNullOrWhiteSpace(Archive.Name) ? Archive.State.PrimaryKeyString : Archive.Name;
        public string Url => Archive.State.GetManifestURL(Archive);

        [JsonIgnore]
        public bool HasUrl => Url != null;
        public ArchiveStatus ArchiveStatus { get; set; }
    }
}
