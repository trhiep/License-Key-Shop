using License_Key_Shop_Web.Utils;

namespace License_Key_Shop_Web.MyInterface
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(PaymentInformationModel model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }

    
}
