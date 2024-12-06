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

        public bool IsSuccessfull { get; private set; }
        public string Message { get; private set; }
        public T Data { get; private set; }
        public string ErrorMessage { get; private set; }

        private OperationResult(bool isSuccessfull, string message, T data, string errorMessage)
        {
            {
                IsSuccessfull = isSuccessfull;
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
    


