using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_WebApp.CustoMiddlewares
{
    /// <summary>
    ///  Clas that will be used to structure the error message
    /// </summary>
    public class ErrorInformation
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// The custome middleware class
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate request;

        public ExceptionMiddleware(RequestDelegate request)
        {
            this.request = request;
        }

        /// <summary>
        /// The method that will contains logic for custom middlewares
        /// This method will be invoked by HttpContext
        /// when registered in Request Pipeline
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // if no exception occures during request processing then
                // go to next middleware in Request Pipeline
                await request(context);
            }
            catch (Exception ex)
            {
                // logic to handle exception
                // set the response statuc code
                context.Response.StatusCode = 500;
                // read the error message
                string message = ex.Message;
                // structure the error message
                var errorInfo = new ErrorInformation()
                {
                    ErrorCode = context.Response.StatusCode,
                    ErrorMessage = message
                };
                // serialize the message in JSON format
                var responseError = JsonConvert.SerializeObject(errorInfo);
                // write the response for the request
                await context.Response.WriteAsync(responseError);
            }
        }
    }

    /// <summary>
    /// The class that isused to register the custom middleware
    /// </summary>
    public static class CustomMiddleware
    {
        /// <summary>
        /// This will be an extension method to the
        /// IApplicationBuilder interface
        /// </summary>
        public static void UseCustomeException(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<ExceptionMiddleware>();
        }
    }

}
