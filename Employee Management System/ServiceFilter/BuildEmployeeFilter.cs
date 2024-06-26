﻿using Employee_Management_System.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Employee_Management_System.ServiceFilter
{
    public class BuildEmployeeFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var param = context.ActionArguments.SingleOrDefault(p => p.Value is EmployeeFilterCriteria);
            if (param.Value == null)
            {
                context.Result = new BadRequestObjectResult("Object is null");
                return;
            }

            EmployeeFilterCriteria filterCriteria = (EmployeeFilterCriteria)param.Value;
            var roleFilter = filterCriteria.Filters.Find(a => a.fieldName == "role");

            if (roleFilter == null)
            {
                roleFilter = new FilterCriteria();
                roleFilter.fieldName = "role";
                roleFilter.fieldValue = new List<string>() { "Manager" }; 
                filterCriteria.Filters.Add(roleFilter);
            }

            filterCriteria.Filters.RemoveAll(a => string.IsNullOrEmpty(a.fieldName));

            var result = await next();
        }
    }
}
