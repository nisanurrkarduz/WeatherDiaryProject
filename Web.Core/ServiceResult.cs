using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Web.Core
{
    // Tüm katmanlarda kullanılacak ortak sonuç yapısı
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static ServiceResult<T> Success(T data, string message = "İşlem başarılı.")
            => new ServiceResult<T> { IsSuccess = true, Data = data, Message = message };

        public static ServiceResult<T> Error(string message)
            => new ServiceResult<T> { IsSuccess = false, Message = message };
    }
}