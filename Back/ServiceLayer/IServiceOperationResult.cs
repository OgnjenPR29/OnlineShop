using ServiceLayer.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public interface IServiceOperationResult
    {
        bool IsSuccessful { get; set; }

        ServiceOperationErrorCode ErrorCode { get; set; }

        string ErrorMessage { get; set; }

        IDTO Dto { get; set; }
    }
}
