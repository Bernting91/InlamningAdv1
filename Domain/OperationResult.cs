using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class OperationResult<T>
    {

        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; }

        private OperationResult(bool success, string message, T data, string errorMessage)
        {
            {
                Success = success;
                Message = message;
                Data = data;
                ErrorMessage = errorMessage ?? string.Empty;
            };
        }
        public static OperationResult<T> SuccessResult(T data, string message = "Operation Successful")
        {
            return new OperationResult<T>(true, message, data, null);
        }
        public static OperationResult<T> FailureResult(string errorMessage, string message = "Operation Failed")
        {
            return new OperationResult<T>(false, message, default(T), errorMessage);
        }
    }
 }
    


