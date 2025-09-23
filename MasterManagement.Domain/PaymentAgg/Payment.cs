//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using _01_FrameWork.Domain;

//namespace MasterManagement.Domain.PaymentAgg
//{

//        public enum PaymentStatus
//        {
//            Pending,
//            Success,
//            Failed,
//            Cancelled
//        }

//        public class Payment : EntityBase
//        {
//            public long OrderId { get; private set; }
//            public double Amount { get; private set; }
//            public PaymentStatus Status { get; private set; }
//            public string TransactionId { get; private set; }
//            public DateTime PaymentDate { get; private set; }
//            public int PaymentMethod { get; private set; }
//            public string FailureMessage { get; private set; }

//            public Payment(long orderId, double amount, int paymentMethod)
//            {
//                OrderId = orderId;
//                Amount = amount;
//                PaymentMethod = paymentMethod;
//                Status = PaymentStatus.Pending;
//                PaymentDate = DateTime.MinValue;
//                TransactionId = string.Empty;
//                FailureMessage = string.Empty;
//            }

//            public void MarkSuccess(string transactionId)
//            {
//                Status = PaymentStatus.Success;
//                TransactionId = transactionId;
//                PaymentDate = DateTime.UtcNow;
//            }

//            public void MarkFailed(string failureMessage)
//            {
//                Status = PaymentStatus.Failed;
//                FailureMessage = failureMessage;
//                PaymentDate = DateTime.UtcNow;
//            }

//            public void Cancel()
//            {
//                Status = PaymentStatus.Cancelled;
//                PaymentDate = DateTime.UtcNow;
//            }
//        }
//    }


