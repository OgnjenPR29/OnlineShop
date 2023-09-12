using ServiceLayer.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class ServiceOperationResult : IServiceOperationResult
    {
		public ServiceOperationResult()
		{
		}

		public ServiceOperationResult(bool isSuccessful)
		{
			IsSuccessful = isSuccessful;
		}
		
		public ServiceOperationResult(bool isSuccessful, IDTO dto)
		{
			IsSuccessful = isSuccessful;
			Dto = dto;

		}

		public ServiceOperationResult(bool isSuccessful, ServiceOperationErrorCode errorCode) : this(isSuccessful)
		{
			IsSuccessful = isSuccessful;
			ErrorCode = errorCode;
		}

		public ServiceOperationResult(bool isSuccessful, ServiceOperationErrorCode errorCode, string errorMessage)
		{
			IsSuccessful = isSuccessful;
			ErrorCode = errorCode;
			ErrorMessage = errorMessage;
		}

		public bool IsSuccessful { get; set; }

		public ServiceOperationErrorCode ErrorCode { get; set; }

		public string ErrorMessage { get; set; }

		public IDTO Dto { get; set; }

	}
}
