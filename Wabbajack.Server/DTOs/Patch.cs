﻿using System;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Wabbajack.Common;
using Wabbajack.Server.DataLayer;
using Wabbajack.Server.EF;

namespace Wabbajack.Server.DTOs
{
    public class Patch
    {
        public ArchiveDownload Src { get; set; }
        public ArchiveDownload Dest { get; set; }
        public long PatchSize { get; set; }
        public DateTime? Finished { get; set; }
        public bool? IsFailed { get; set; }
        public string FailMessage { get; set; }

        public async Task Finish(SqlService sql, long size)
        {
            IsFailed = false;
            Finished = DateTime.UtcNow;
            PatchSize = size;
            await sql.FinializePatch(this);
        }


        public async Task Fail(SqlService sql, string msg)
        {
            IsFailed = true;
            Finished = DateTime.UtcNow;
            FailMessage = msg;
            await sql.FinializePatch(this);
        }

        
        
    }
}
