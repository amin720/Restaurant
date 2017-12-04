

using Restaurant.Web.ViewModels;

namespace Restaurant.Web.Services
{
	public interface IGateway
	{
		PaymentResult ProcessPayment(CheckoutViewModel model);
	}
}
