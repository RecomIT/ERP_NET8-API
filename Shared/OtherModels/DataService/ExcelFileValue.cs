﻿using Microsoft.AspNetCore.Http;
using Shared.Helpers.ValidationFilters;
using System.ComponentModel.DataAnnotations;

namespace Shared.OtherModels.DataService
{
    /// <summary>
    /// Use: File value class is used to read to file informations based on the key. It is only for excel reading file
    /// </summary>
    public class ExcelFileValue
    {
        [AllowedExtensions(new string[] { ".xls",".xlsx"})]
        public IFormFile ExcelFile { get; set; }
        [Required,StringLength(200)]
        public string Key { get; set; }
    }
}
