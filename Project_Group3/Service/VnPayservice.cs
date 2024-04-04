using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Project_Group3.Helper;
using Project_Group3.Models;

namespace Project_Group3.Service
{
    public class VnPayservice : IVnpayService
    {
        private readonly IConfiguration _config;
        public VnPayservice(IConfiguration config)
        {
            _config = config;
        }
        public string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model)
        {
            var tick = DateTime.Now.Ticks.ToString();
            var vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", _config["VNpay:Version"]);
            vnpay.AddRequestData("vnp_Command", _config["VNpay:Command"]);
            vnpay.AddRequestData("vnp_TmnCode", _config["VNpay:TmnCode"]);
            vnpay.AddRequestData("vnp_Amount", (model.Amount/10).ToString());
            //Số tiền thanh toán. Số tiền không 
            // mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND 
            // (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY 
            // là: 10000000

            vnpay.AddRequestData("vnp_CreateDate", model.CreateDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", _config["VNpay:CurrCode"]);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context));
            vnpay.AddRequestData("vnp_Locale", _config["VNpay:Locale"]);
            vnpay.AddRequestData("vnp_OrderInfo", "Payment for this order: " + model.OrderId);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", _config["VNpay:PaymentBackReturnUrl"]);
            vnpay.AddRequestData("vnp_TxnRef", tick); // Mã tham chiếu của giao dịch và không trùng lặp


            var paymentUrl = vnpay.CreateRequestUrl(_config["VNpay:BaseUrl"], _config["VNpay:HashSecret"]);
            return paymentUrl;
        }

        public VnPaymentResponseModel PaymentExcute(IQueryCollection collections)
        {
            var vnpay = new VnPayLibrary();
            foreach (var (key, value) in collections)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddRequestData(key, value.ToString());
                }
            }
            var vnp_orderId = vnpay.GetResponseData("vnp_TxnRef");
            var vnp_TransactionId = vnpay.GetResponseData("vnp_TransactionNo");
            var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value;
            var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
            var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
            bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash, _config["VNpay:HashSecret"]);
            if (!checkSignature)
            {
                return new VnPaymentResponseModel
                {
                    Success = false
                };

            }
            return new VnPaymentResponseModel
            {
                Success = true,
                PaymentMethod = "Vnpay",
                OrderDescription = vnp_OrderInfo,
                OrderId = vnp_orderId.ToString(),
                TransactionId = vnp_TransactionId.ToString(),
                Token = vnp_SecureHash,
                VnPayResponseCode = vnp_ResponseCode
            };


        }
    }
}