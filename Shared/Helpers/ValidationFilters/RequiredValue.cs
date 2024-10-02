﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Helpers.ValidationFilters
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredValueAttribute : RequiredAttribute
    {
        private string[] checkingValues { get; set; }
        public RequiredValueAttribute(string[] status)
        {
            checkingValues = status;
        }
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            object instance = context.ObjectInstance;
            Type type = instance.GetType();

            var isExistInValues = checkingValues.FirstOrDefault(s => s == value?.ToString());

            if (isExistInValues != null && (!string.IsNullOrWhiteSpace(value?.ToString()))) {
                return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessage);

        }
    }
}
