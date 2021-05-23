using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParkingAbstract.ViewModels.General
{
    public class ActionResult<T>
    {
        public virtual bool Successed { get; }
        public virtual IEnumerable<ActionError> Errors { get; set; }
        public virtual T Data { get; set; }

        private ActionResult(bool successed)
        {
            Successed = successed;
        }

        public static ActionResult<T> Fail(IEnumerable<ActionError> errors)
        {
            return new ActionResult<T>(false)
            {
                Errors = errors
            };
        }

        public static ActionResult<T> Success(T data)
        {
            return new ActionResult<T>(true)
            {
                Data = data,
                Errors = new List<ActionError>()
            };
        }
    }

    public class ActionError
    {
        public virtual int ErrorCode { get; set; }
        public virtual string ErrorMessage { get; set; }
    }
}
