﻿using System.Collections.Generic;

namespace Shared.OtherModels.Response
{
    public class ExecutionStatus
    {
        public bool Status { get; set; } = false;
        public string Code { get; set; } = "";
        public string Msg { get; set; }
        public string ErrorMsg { get; set; }
        public string Json { get; set; }
        public string token { get; set; }
        public Dictionary<bool, string> Messages { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public string StatusCode { get; set; }
        public string ItemCount { get; set; }
        public long ItemId { get; set; }
        public string Ids { get; set; }
        public string Action { get; set; }
        public string PresentValue { get; set; }
        public string PreviousValue { get; set; }
    }
}
