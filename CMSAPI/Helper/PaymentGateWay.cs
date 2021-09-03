//using Stripe;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace CMSAPI.Helper
//{
//    public class PaymentGateWay
//    {
//        public static PaymentIntent payment()
//        {
//            // payment gateway         
//            var paymentIntents = new PaymentIntentService();
//            var paymentIntent = paymentIntents.Create(new PaymentIntentCreateOptions
//            {
//                Amount = CalculateOrderAmount(request.Items),
//                Currency = "usd",
//            });

//            return paymentIntent;

//        }
//    }
//}
