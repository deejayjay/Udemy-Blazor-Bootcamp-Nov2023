using System.ComponentModel.Design;
using Tangy_Models;

namespace TangyWeb_Client.Service.IService
{
    public interface IPaymentService
    {
        Task<SuccessModelDto> Checkout(StripePaymentDto model);
    }
}
