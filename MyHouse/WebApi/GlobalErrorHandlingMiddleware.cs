using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Application;
using Contract;
using Core.Const;
using Core.Exceptions;
using Core.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NuGet.Common;

namespace WebApi
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public GlobalErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status = HttpStatusCode.OK;
            var stackTrace = string.Empty;
            string message = "";

            var exceptionType = exception.GetType();

            if (exceptionType == typeof(GlobalException))
            {
                var globalException = (GlobalException) exception;
                message = globalException.Message;
                status = globalException.Status;
                stackTrace = globalException.StackTrace;
            }
            else
            {
                message = HttpMessage.ServerError;
                status = HttpStatusCode.InternalServerError;
                stackTrace = exception.StackTrace;
                
                FileHelper.WriteLog(exception,GlobalSetting.LOG_PATH);
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) status;
            var exceptionResult = JsonSerializer.Serialize(new {message = message, status = status});
            return context.Response.WriteAsync(exceptionResult);
        }
    }
}