using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.General
{
    public class ServiceResponse<T>
    {
        public virtual bool Successed { get; }
        public virtual IEnumerable<ServiceError> Errors { get; set; }
        public virtual T Data { get; set; }

        private ServiceResponse(bool successed)
        {
            Successed = successed;
        }

        public static ServiceResponse<T> Fail(params ServiceError[] errors)
        {
            return new ServiceResponse<T>(false)
            {
                Errors = errors
            };
        }

        public static ServiceResponse<T> Fail(IEnumerable<ServiceError> errors)
        {
            return new ServiceResponse<T>(false)
            {
                Errors = errors
            };
        }

        public static ServiceResponse<T> Success(T data)
        {
            return new ServiceResponse<T>(true)
            {
                Data = data,
                Errors = new List<ServiceError>()
            };
        }
    }

    public class ServiceError
    {
        public virtual string ErrorCode { get; set; }
        public virtual string ErrorMessage { get; set; }
    }
}
