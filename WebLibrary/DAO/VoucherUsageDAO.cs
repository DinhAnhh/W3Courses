using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLibrary.Models;

namespace WebLibrary.DAO
{
    public class VoucherUsageDAO
    {
        private static VoucherUsageDAO instance = null;
        private static readonly object instanceLock = new object();
        public static VoucherUsageDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new VoucherUsageDAO();
                    }
                    return instance;
                }
            }
        }


        public void SaveVoucherUsage(string voucherCode, int userId)
        {
            VoucherDAO dAO = new VoucherDAO();
            var v = dAO.GetVoucherByCode(voucherCode);

            // Tạo một đối tượng VoucherUsage mới
            var voucherUsage = new VoucherUsage
            {
                CodeVoucher = voucherCode,
                LearnerId = userId,
                VoucherId = v.VoucherId,

            };
            using (var context = new DBContext())
            {
                context.VoucherUsages.Add(voucherUsage);

                context.SaveChanges();
            }


        }
        public bool IsVoucherUsedByUser(string voucherCode, int userId)
        {
            using (DBContext dbContext = new DBContext())
            {
                bool isVoucherUsed = dbContext.VoucherUsages.Any(u => u.CodeVoucher == voucherCode && u.LearnerId == userId);
                return !isVoucherUsed;
            }
        }
    }
}